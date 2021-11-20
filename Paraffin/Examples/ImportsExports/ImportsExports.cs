using System;
using System.Runtime.InteropServices;
using System.Text;

using static Wax.Paraffin.__wasm;
using static Wax.Paraffin.__wasmer;

namespace Wax.Paraffin.Examples {
    public class ImportsExportsExample {
        static void print_wasmer_error() {
            var error_len = wasmer_last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            wasmer_last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
            var error_str = Encoding.UTF8.GetString(error_message.ToArray());
            Console.Error.WriteLine(error_str.ToString());
        }

        static IntPtr host_func_callback(ref wasm_val_vec_t args, ref wasm_val_vec_t results) {
            Console.WriteLine("Calling back...");
            Console.Write("> ");

            var val = WASM_I32_VAL(42);
            unsafe { wasm_val_copy((wasm_val_t*)results.data, &val); }
            wasm_val_delete(ref val);

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
            wasm_byte_vec_t wat = default;
            wasm_byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            wasm_byte_vec_t wasm_bytes = default;
            wat2wasm(ref wat, ref wasm_bytes);
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

            Console.WriteLine("Creating the imported function...");
            var i32 = wasm_valtype_new_i32();
            var host_func_type = wasm_functype_new_0_1(ref i32);
            var fp4del = Marshal.GetFunctionPointerForDelegate<func_callback>(host_func_callback);
            var host_func = wasm_func_new(store, host_func_type, fp4del);
            wasm_functype_delete(host_func_type);

            Console.WriteLine("Creating the imported global...");
            var host_global_type = wasm_globaltype_new(wasm_valtype_new_f32(), (byte)wasm_mutability_enum.CONST);
            var host_global_val = WASM_I32_VAL(42);
            var host_global = wasm_global_new(store, host_global_type, ref host_global_val);
            wasm_globaltype_delete(host_global_type);

            Span<IntPtr> externs = stackalloc IntPtr[2] {
                wasm_func_as_extern(host_func),
                wasm_global_as_extern(host_global),
            };

            wasm_extern_vec_t import_object = WASM_ARRAY_EXTERN_VEC(externs);

            Console.WriteLine("Instantiating module...");
            var instance = wasm_instance_new(store, module, ref import_object, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Retrieving exports...");
            wasm_extern_vec_t exports = default;
            wasm_instance_exports(instance, ref exports);
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
            var func = wasm_extern_as_func(exports_span[0]);
            if (func == null) {
                Console.WriteLine("> Failed to get the exported function!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported function: {func:X}");

            Console.WriteLine("Retrieving the exported global...");
            var global = wasm_extern_as_global(exports_span[1]);
            if (global == null) {
                Console.WriteLine("> Failed to get the exported global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported global: {global:X}");

            Console.WriteLine("Retrieving the exported table...");
            var table = wasm_extern_as_table(exports_span[2]);
            if (table == null) {
                Console.WriteLine("> Failed to get the exported table!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported table: {table:X}");

            Console.WriteLine("Retrieving the exported memory...");
            var memory = wasm_extern_as_memory(exports_span[3]);
            if (memory == null) {
                Console.WriteLine("> Failed to get the exported memory!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine($"Got the exported memory: {memory:X}");

            wasm_module_delete(module);
            wasm_extern_vec_delete(ref exports);
            wasm_instance_delete(instance);
            wasm_store_delete(store);
            wasm_engine_delete(engine);
        }
    }
}
