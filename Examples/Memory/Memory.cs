using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.Examples {
    public class MemoryExample {
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
                new val_t { kind = (byte)valkind_t.ANYREF, of = { @ref = IntPtr.Zero } },
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
}
