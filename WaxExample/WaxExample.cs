using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.WaxExample {
    public class WaxExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
            var error_str = Encoding.UTF8.GetString(error_message.ToArray());
            Console.Error.WriteLine(error_str.ToString());
        }

        public static void Main(string[] args) {
            var wat_string = @"
(module
   (type $mem_size_t (func (result i32)))
   (type $get_at_t (func (param i32) (result i32)))
   (type $set_at_t (func (param i32) (param i32)))
   (memory $mem 1)
   (func $get_at (type $get_at_t) (param $idx i32) (result i32)
     (i32.load (local.get $idx)))
   (func $set_at (type $set_at_t) (param $idx i32) (param $val i32)
     (i32.store (local.get $idx) (local.get $val)))
   (func $mem_size (type $mem_size_t) (result i32)
     (memory.size))
   (export ""get_at"" (func $get_at))
   (export ""set_at"" (func $set_at))
   (export ""mem_size"" (func $mem_size))
   (export ""memory"" (memory $mem)))
";
            var wat_string_utf8 = Encoding.UTF8.GetBytes(wat_string);
            vec_t/*wasm_byte_vec_t*/ wat = default;
            __wasm.byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            vec_t/*wasm_byte_vec_t*/ wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);
            __wasm.byte_vec_delete(ref wat);

            Console.WriteLine("Creating the store...");
            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);

            Console.WriteLine("Compiling module...");
            var module = __wasm.module_new(store, ref wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            __wasm.byte_vec_delete(ref wasm_bytes);

            Console.WriteLine("Creating imports...");
            vec_t/*wasm_extern_vec_t*/ imports = default;
            __wasm.extern_vec_new_empty(ref imports);

            Console.WriteLine("Instantiating module...");
            var instance = __wasm.instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            vec_t/*wasm_extern_vec_t*/ exports = default;
            __wasm.instance_exports(instance, ref exports); // <-- CRASHES HERE
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Span<IntPtr> exports_span;
            unsafe {
                exports_span = new Span<IntPtr>((IntPtr*)exports.data, (int)exports.size);
            }

            var get_at = __wasm.extern_as_func(exports_span[0]);
            var set_at = __wasm.extern_as_func(exports_span[1]);
            var mem_size = __wasm.extern_as_func(exports_span[2]);
            var memory = __wasm.extern_as_memory(exports_span[3]);

            Console.WriteLine("Querying memory size...");
            var pages = __wasm.memory_size(memory);
            var data_size = __wasm.memory_data_size(memory);
            Console.WriteLine($"Memory size (pages): {pages}");
            Console.WriteLine($"Memory size (bytes): {data_size}");

            Console.WriteLine("Growing memory...\n");
            if (!__wasm.memory_grow(memory, 2)) {
                Console.WriteLine("> Error growing memory!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            var new_pages = __wasm.memory_size(memory);
            Console.WriteLine($"New memory size (pages): {new_pages}");

            int mem_addr = 0x2220;
            int val = 0xFEFEFFE;

            Span<val_t> set_at_args_val = stackalloc val_t[2] {
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = mem_addr } },
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = val } },
            };

            vec_t/*wasm_val_vec_t*/ set_at_args = default;
            set_at_args.size = (ulong)set_at_args_val.Length;
            unsafe {
                fixed (void* arg0 = &set_at_args_val[0]) {
                    set_at_args.data = (IntPtr)arg0;
                }
            }

            vec_t/*wasm_val_vec_t*/ set_at_results = default;
            __wasm.valtype_vec_new_empty(ref set_at_results);

            __wasm.func_call(set_at, ref set_at_args, ref set_at_results);

            Span<val_t> get_at_args_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = mem_addr } },
            };

            vec_t/*wasm_val_vec_t*/ get_at_args = default;
            get_at_args.size = (ulong)get_at_args_val.Length;
            unsafe {
                fixed (void* arg0 = &get_at_args_val[0]) {
                    get_at_args.data = (IntPtr)arg0;
                }
            }

            Span<val_t> get_at_results_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.ANYREF, of = { @ref = IntPtr.Zero} },
            };

            vec_t/*wasm_val_vec_t*/ get_at_results = default;
            get_at_results.size = (ulong)get_at_results_val.Length;
            unsafe {
                fixed (void* arg0 = &get_at_results_val[0]) {
                    get_at_results.data = (IntPtr)arg0;
                }
            }

            __wasm.func_call(get_at, ref get_at_args, ref get_at_results);

            Console.WriteLine($"Value at {mem_addr:X}: {get_at_results_val[0].of.i32:X}");

            __wasm.extern_vec_delete(ref exports);
            __wasm.module_delete(module);
            __wasm.instance_delete(instance);
            __wasm.store_delete(store);
            __wasm.engine_delete(engine);
        }
    }

    public class ExportsGlobalExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
            var error_str = Encoding.UTF8.GetString(error_message.ToArray());
            Console.Error.WriteLine(error_str.ToString());
        }

        public static void Main(string[] args) {
            var wat_string = @"
(module
  (global $one (export ""one"") f32 (f32.const 1))
  (global $some(export ""some"")(mut f32)(f32.const 0))
  (func(export ""get_one"")(result f32)(global.get $one))
  (func(export ""get_some"")(result f32)(global.get $some))
  (func(export ""set_some"")(param f32)(global.set $some(local.get 0))))
";
            var wat_string_utf8 = Encoding.UTF8.GetBytes(wat_string);
            vec_t/*wasm_byte_vec_t*/ wat = default;
            __wasm.byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            vec_t/*wasm_byte_vec_t*/ wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);
            __wasm.byte_vec_delete(ref wat);

            Console.WriteLine("Creating the store...");
            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);

            Console.WriteLine("Compiling module...");
            var module = __wasm.module_new(store, ref wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            __wasm.byte_vec_delete(ref wasm_bytes);


            Console.WriteLine("Creating imports...");
            vec_t/*wasm_extern_vec_t*/ imports = default;
            __wasm.extern_vec_new_empty(ref imports);

            Console.WriteLine("Instantiating module...");
            var instance = __wasm.instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            vec_t/*wasm_extern_vec_t*/ exports = default;
            __wasm.instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Span<IntPtr> exports_span;
            unsafe {
                exports_span = new Span<IntPtr>((IntPtr*)exports.data, (int)exports.size);
            }

            var one = __wasm.extern_as_global(exports_span[0]);
            if (one == null) {
                Console.WriteLine("> Failed to get the `one` global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            var some = __wasm.extern_as_global(exports_span[1]);
            if (some == null) {
                Console.WriteLine("> Failed to get the `some` global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Getting globals types information...");
            var one_type = __wasm.global_type(one);
            var some_type = __wasm.global_type(some);

            var one_mutability = __wasm.globaltype_mutability(one_type);
            var one_content = __wasm.globaltype_content(one_type);
            var one_kind = __wasm.valtype_kind(one_content);

            var some_mutability = __wasm.globaltype_mutability(some_type);
            var some_content = __wasm.globaltype_content(some_type);
            var some_kind = __wasm.valtype_kind(some_content);

            var one_const = one_mutability == (byte)mutability_t.CONST ? "const" : "";
            var some_const = some_mutability == (byte)mutability_t.CONST ? "const" : "";
            Console.WriteLine($"`one` type: {one_const} {one_kind}"); // NOTE: f32 == "2"
            Console.WriteLine($"`some` type: {some_const} {some_kind}");

            Console.WriteLine("Getting global values...");
            val_t one_value;
            unsafe { __wasm.global_get(one, &one_value); }
            Console.WriteLine($"`one` value: {one_value.of.f32}");

            val_t some_value;
            unsafe { __wasm.global_get(some, &some_value); }
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            Console.WriteLine("Setting global values...\n");
            var one_set_value = new val_t { kind = (byte)valkind_t.F32, of = { f32 = 42 } };
            unsafe { __wasm.global_set(one, &one_set_value); }

            var error_length = __wasmer.last_error_length();
            if (error_length > 0) {
                Span<byte> error_message = stackalloc byte[(int)error_length];
                __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_message), error_length);
                var error_str = Encoding.UTF8.GetString(error_message.ToArray());
                Console.WriteLine($"Attempted to set an immutable global: `{error_str}`");
            }

            var some_set_value = new val_t { kind = (byte)valkind_t.F32, of = { f32 = 21 } };
            unsafe { __wasm.global_set(some, &some_set_value); }

            // I think this is a bug in the C example and it means to either
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            // a) print some_set_value, or
            Console.WriteLine($"`some_set` value: {some_set_value.of.f32}");

            // b) re-get into some_value then print some_value
            unsafe { __wasm.global_get(some, &some_value); }
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            __wasm.module_delete(module);
            __wasm.extern_vec_delete(ref exports);
            __wasm.instance_delete(instance);
            __wasm.store_delete(store);
            __wasm.engine_delete(engine);
        }
    }

    public class ImportExportExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
            var error_str = Encoding.UTF8.GetString(error_message.ToArray());
            Console.Error.WriteLine(error_str.ToString());
        }

        static IntPtr host_func_callback(ref vec_t/* val_vec_t */args, ref vec_t/* val_vec_t */results) {
            Console.WriteLine("Calling back...");
            Console.Write("> ");

            var val = new val_t { kind = (byte)valkind_t.I32, of = { i32 = 42 } };
            unsafe { __wasm.val_copy((val_t*)results.data, &val); }
            __wasm.val_delete(ref val);

            return IntPtr.Zero;
        }

        public static void Main(string[] args) {
            var wat_string = @"
(module
  (func $host_function (import """" ""host_function"") (result i32))
  (global $host_global (import ""env"" ""host_global"") i32)
  (func $function (export ""guest_function"") (result i32) (global.get $global))
  (global $global (export ""guest_global"") i32 (i32.const 42))
  (table $table (export ""guest_table"") 1 1 funcref)
  (memory $memory (export ""guest_memory"") 1))
";
            var wat_string_utf8 = Encoding.UTF8.GetBytes(wat_string);
            vec_t/*wasm_byte_vec_t*/ wat = default;
            __wasm.byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            vec_t/*wasm_byte_vec_t*/ wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);
            __wasm.byte_vec_delete(ref wat);

            Console.WriteLine("Creating the store...");
            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);

            Console.WriteLine("Compiling module...");
            var module = __wasm.module_new(store, ref wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            __wasm.byte_vec_delete(ref wasm_bytes);

            Console.WriteLine("Creating the imported function...");
            var i32 = __wasm.valtype_new_i32();
            var host_func_type = __wasm.functype_new_0_1(ref i32);
            var fp4del = Marshal.GetFunctionPointerForDelegate<__wasm.func_callback>(host_func_callback);
            var host_func = __wasm.func_new(store, host_func_type, fp4del);
            __wasm.functype_delete(host_func_type);

            Console.WriteLine("Creating the imported global...");
            var host_global_type = __wasm.globaltype_new(__wasm.valtype_new_f32(), (byte)mutability_t.CONST);
            var host_global_val = new val_t { kind = (byte)valkind_t.I32, of = { i32 = 42 } };
            var host_global = __wasm.global_new(store, host_global_type, ref host_global_val);
            __wasm.globaltype_delete(host_global_type);

            Span<IntPtr> imports = stackalloc IntPtr[2] {
                __wasm.func_as_extern(host_func),
                __wasm.global_as_extern(host_global),
            };

            vec_t/* extern_vec_t */ import_object = default;
            import_object.size = (ulong)imports.Length;
            unsafe {
                fixed (void* arg0 = &imports[0]) {
                    import_object.data = (IntPtr)arg0;
                }
            }

            Console.WriteLine("Instantiating module...");
            var instance = __wasm.instance_new(store, module, ref import_object, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            vec_t/*wasm_extern_vec_t*/ exports = default;
            __wasm.instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Span<IntPtr> exports_span;
            unsafe {
                exports_span = new Span<IntPtr>((IntPtr*)exports.data, (int)exports.size);
            }

            Console.WriteLine("Retrieving the exported function...");
            var func = __wasm.extern_as_func(exports_span[0]);
            if (func == null) {
                Console.WriteLine("> Failed to get the exported function!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported function: {func:X}");

            Console.WriteLine("Retrieving the exported global...");
            var global = __wasm.extern_as_global(exports_span[1]);
            if (global == null) {
                Console.WriteLine("> Failed to get the exported global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported global: {global:X}");

            Console.WriteLine("Retrieving the exported table...");
            var table = __wasm.extern_as_table(exports_span[2]);
            if (table == null) {
                Console.WriteLine("> Failed to get the exported table!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported table: {table:X}");

            Console.WriteLine("Retrieving the exported memory...");
            var memory = __wasm.extern_as_memory(exports_span[3]);
            if (memory == null) {
                Console.WriteLine("> Failed to get the exported memory!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported memory: {memory:X}");

            __wasm.module_delete(module);
            __wasm.extern_vec_delete(ref exports);
            __wasm.instance_delete(instance);
            __wasm.store_delete(store);
            __wasm.engine_delete(engine);
        }
    }

    public class InstanceExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_str = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_str), error_len);
            Console.Error.WriteLine(error_str.ToString());
        }

        public static void Main(string[] args) {
            var wat_string = @"
(module
  (type $add_one_t (func (param i32) (result i32)))
  (func $add_one_f (type $add_one_t) (param $value i32) (result i32)
    local.get $value
    i32.const 1
    i32.add)
  (export ""add_one"" (func $add_one_f)))
";
            var wat_string_utf8 = Encoding.UTF8.GetBytes(wat_string);
            vec_t/*wasm_byte_vec_t*/ wat = default;
            __wasm.byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            vec_t/*wasm_byte_vec_t*/ wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);

            Console.WriteLine("Creating the store...");
            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);

            Console.WriteLine("Compiling module...");
            var module = __wasm.module_new(store, ref wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            __wasm.byte_vec_delete(ref wat);

            Console.WriteLine("Creating imports...");
            vec_t/*wasm_extern_vec_t*/ imports = default;

            Console.WriteLine("Instantiating module...");
            var instance = __wasm.instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            vec_t/*wasm_extern_vec_t*/ exports = default;
            __wasm.instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            IntPtr add_one_func;
            unsafe {
                var realptr = (IntPtr*)exports.data;
                add_one_func = __wasm.extern_as_func(*realptr);
                if (add_one_func == IntPtr.Zero) {
                    Console.Error.WriteLine("> Error accessing export!");
                    print_wasmer_error();
                    Environment.Exit(1);
                }
            }

            __wasm.module_delete(module);
            __wasm.instance_delete(instance);

            Console.WriteLine("Calling `add_one` function...");
            Span<val_t> args_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = 1 } },
            };
            Span<val_t> results_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.ANYREF, of = { @ref = IntPtr.Zero } },
            };

            vec_t/*wasm_val_vec_t*/ args_vec = default;
            args_vec.size = (ulong)args_val.Length;
            unsafe {
                fixed (void* arg0 = &args_val[0]) {
                    args_vec.data = (IntPtr)arg0;
                }
            }

            vec_t/*wasm_val_vec_t*/ results_vec = default;
            results_vec.size = (ulong)results_val.Length;
            unsafe {
                fixed (void* result0 = &results_val[0]) {
                    results_vec.data = (IntPtr)result0;
                }
            }

            if (__wasm.func_call(add_one_func, ref args_vec, ref results_vec) != IntPtr.Zero) {
                Console.Error.WriteLine("> Error calling function!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Results of `add_one`: {results_val[0].of.i32}");

            __wasm.extern_vec_delete(ref exports);
            __wasm.store_delete(store);
            __wasm.engine_delete(engine);
        }
    }
}
