using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;

using static Wax.Imports.__wasm;

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
            wasm_byte_vec_t wat = default;
            wasm_byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            wasm_byte_vec_t wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);
            wasm_byte_vec_delete(ref wat);

            Console.WriteLine("Creating the store...");
            var engine = wasm_engine_new();
            var store = wasm_store_new(engine);

            Console.WriteLine("Compiling module...");
            var module = wasm_module_new(store, ref wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            wasm_byte_vec_delete(ref wasm_bytes);

            Console.WriteLine("Creating imports...");
            wasm_extern_vec_t import_object = WASM_EMPTY_EXTERN_VEC();

            Console.WriteLine("Instantiating module...");
            var instance = wasm_instance_new(store, module, ref import_object, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            wasm_extern_vec_t exports = default;
            wasm_instance_exports(instance, ref exports); // <-- CRASHES HERE
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Span<IntPtr> exports_span;
            unsafe {
                exports_span = new Span<IntPtr>((IntPtr*)exports.data, (int)exports.size);
            }

            var get_at = wasm_extern_as_func(exports_span[0]);
            var set_at = wasm_extern_as_func(exports_span[1]);
            var mem_size = wasm_extern_as_func(exports_span[2]);
            var memory = wasm_extern_as_memory(exports_span[3]);

            Console.WriteLine("Querying memory size...");
            var pages = wasm_memory_size(memory);
            var data_size = wasm_memory_data_size(memory);
            Console.WriteLine($"Memory size (pages): {pages}");
            Console.WriteLine($"Memory size (bytes): {data_size}");

            Console.WriteLine("Growing memory...\n");
            if (!wasm_memory_grow(memory, 2)) {
                Console.WriteLine("> Error growing memory!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            var new_pages = wasm_memory_size(memory);
            Console.WriteLine($"New memory size (pages): {new_pages}");

            int mem_addr = 0x2220;
            int val = 0xFEFEFFE;

            Span<wasm_val_t> set_at_args_val = stackalloc wasm_val_t[2] {
                WASM_I32_VAL(mem_addr),
                WASM_I32_VAL(val),
            };
            wasm_val_vec_t set_at_args = WASM_ARRAY_VEC(set_at_args_val);
            wasm_val_vec_t set_at_results = WASM_EMPTY_VAL_VEC();
            wasm_func_call(set_at, ref set_at_args, ref set_at_results);

            Span<wasm_val_t> get_at_args_val = stackalloc wasm_val_t[1] { WASM_I32_VAL(mem_addr) };
            wasm_val_vec_t get_at_args = WASM_ARRAY_VEC(get_at_args_val);
            Span<wasm_val_t> get_at_results_val = stackalloc wasm_val_t[1] { WASM_INIT_VAL(), };
            wasm_val_vec_t get_at_results = WASM_ARRAY_VEC(get_at_results_val);
            wasm_func_call(get_at, ref get_at_args, ref get_at_results);

            Console.WriteLine($"Value at {mem_addr:X}: {get_at_results_val[0].of.i32:X}");

            wasm_extern_vec_delete(ref exports);
            wasm_module_delete(module);
            wasm_instance_delete(instance);
            wasm_store_delete(store);
            wasm_engine_delete(engine);
        }
    }
}
