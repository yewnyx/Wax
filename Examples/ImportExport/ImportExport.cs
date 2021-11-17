using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.Examples {
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
}
