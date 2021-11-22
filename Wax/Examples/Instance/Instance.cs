using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Paraffin;

using static Wax.Paraffin.__wasm;
using static Wax.Paraffin.__wasmer;

namespace Wax.Examples {
    public class InstanceExample {
        static void print_wasmer_error() {
            var error_len = wasmer_last_error_length();
            Span<byte> error_str = stackalloc byte[(int)error_len];
            wasmer_last_error_message(ref MemoryMarshal.GetReference(error_str), error_len);
            Console.Error.WriteLine(error_str.ToString());
        }

        public static void Main(string[] argv) {
            var wat_string = @"
(module
  (type $add_one_t (func (param i32) (result i32)))
  (func $add_one_f (type $add_one_t) (param $value i32) (result i32)
    local.get $value
    i32.const 1
    i32.add)
  (export ""add_one"" (func $add_one_f)))
";
            var wat = WasmByteVec.New(Encoding.UTF8.GetBytes(wat_string));
            var wasm_bytes = WasmByteVec.New();
            
            // TODO: port wat2wasm
            wat2wasm(ref wat.RawVec, ref wasm_bytes.RawVec);

            Console.WriteLine("Creating the store...");
            var engine = WasmEngine.New();
            var store = WasmStore.New(engine);

            Console.WriteLine("Compiling module...");
            var module = WasmModule.New(store, wasm_bytes);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            wat.Dispose();

            Console.WriteLine("Creating imports...");
            var imports = WasmExternVec.New();

            Console.WriteLine("Instantiating module...");
            var instance = WasmInstance.New(store, module, imports, out _);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            var exports = instance.Exports();
            // TODO: hide RawVec
            if (exports.RawVec.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            IntPtr add_one_func;
            unsafe {
                var realptr = (IntPtr*)exports.RawVec.data;
                // NOTE: YIKES
                add_one_func = wasm_extern_as_func(*realptr);
                if (add_one_func == IntPtr.Zero) {
                    Console.Error.WriteLine("> Error accessing export!");
                    print_wasmer_error();
                    Environment.Exit(1);
                }
            }

            module.Dispose();
            instance.Dispose();

            Console.WriteLine("Calling `add_one` function...");
            Span<wasm_val_t> args_val = stackalloc wasm_val_t[1] { WASM_I32_VAL(1) };
            Span<wasm_val_t> results_val = stackalloc wasm_val_t[1] { WASM_INIT_VAL() };
            wasm_val_vec_t args = WASM_ARRAY_VEC(args_val);
            wasm_val_vec_t results = WASM_ARRAY_VEC(results_val);

            if (wasm_func_call(add_one_func, ref args, ref results) != IntPtr.Zero) {
                Console.Error.WriteLine("> Error calling function!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Results of `add_one`: {results_val[0].of.i32}");

            exports.Dispose();
            store.Dispose();
            engine.Dispose();
        }
    }
}
