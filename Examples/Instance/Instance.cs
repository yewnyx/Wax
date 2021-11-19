using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;

using static Wax.Imports.__wasm;

namespace Wax.Examples {
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
            wasm_byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            vec_t/*wasm_byte_vec_t*/ wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);

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

            wasm_byte_vec_delete(ref wat);

            Console.WriteLine("Creating imports...");
            vec_t/*wasm_extern_vec_t*/ imports = default;

            Console.WriteLine("Instantiating module...");
            var instance = wasm_instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            vec_t/*wasm_extern_vec_t*/ exports = default;
            wasm_instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            IntPtr add_one_func;
            unsafe {
                var realptr = (IntPtr*)exports.data;
                add_one_func = wasm_extern_as_func(*realptr);
                if (add_one_func == IntPtr.Zero) {
                    Console.Error.WriteLine("> Error accessing export!");
                    print_wasmer_error();
                    Environment.Exit(1);
                }
            }

            wasm_module_delete(module);
            wasm_instance_delete(instance);

            Console.WriteLine("Calling `add_one` function...");
            Span<wasm_val_t> args_val = stackalloc wasm_val_t[1] {
                new wasm_val_t { kind = (byte)wasm_valkind_enum.I32, of = { i32 = 1 } },
            };
            Span<wasm_val_t> results_val = stackalloc wasm_val_t[1] {
                new wasm_val_t { kind = (byte)wasm_valkind_enum.ANYREF, of = { @ref = IntPtr.Zero } },
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

            if (wasm_func_call(add_one_func, ref args_vec, ref results_vec) != IntPtr.Zero) {
                Console.Error.WriteLine("> Error calling function!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Results of `add_one`: {results_val[0].of.i32}");

            wasm_extern_vec_delete(ref exports);
            wasm_store_delete(store);
            wasm_engine_delete(engine);
        }
    }
}
