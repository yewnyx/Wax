using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.WaxExample {
    public class WaxExample {
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
            // wasm_val_t results_val[1] = { WASM_INIT_VAL };
            Span<val_t> results_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.ANYREF, of = { @ref = IntPtr.Zero } },
            };

            // QUESTION: Is this the best way? This strikes me as possibly non-ideal.
            // wasm_val_vec_t args = WASM_ARRAY_VEC(args_val);
            vec_t/*wasm_val_vec_t*/ args_vec = default;
            args_vec.size = (ulong)args_val.Length;
            unsafe {
                fixed (void* arg0 = &args_val[0]) {
                    args_vec.data = (IntPtr)arg0;
                }
            }

            // QUESTION: Same as above
            // wasm_val_vec_t results = WASM_ARRAY_VEC(results_val);
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
