using System;
using System.Runtime.InteropServices;
using System.Text;

using static Wax.Paraffin.__wasm;
using static Wax.Paraffin.__wasmer;

namespace Wax.Paraffin.Examples {
    public class ExportsGlobalExample {
        static void print_wasmer_error() {
            var error_len = wasmer_last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            wasmer_last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
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

            var one = wasm_extern_as_global(exports_span[0]);
            if (one == null) {
                Console.WriteLine("> Failed to get the `one` global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            var some = wasm_extern_as_global(exports_span[1]);
            if (some == null) {
                Console.WriteLine("> Failed to get the `some` global!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Getting globals types information...");
            var one_type = wasm_global_type(one);
            var some_type = wasm_global_type(some);

            var one_mutability = wasm_globaltype_mutability(one_type);
            var one_content = wasm_globaltype_content(one_type);
            var one_kind = wasm_valtype_kind(one_content);

            var some_mutability = wasm_globaltype_mutability(some_type);
            var some_content = wasm_globaltype_content(some_type);
            var some_kind = wasm_valtype_kind(some_content);

            var one_const = one_mutability == (byte)wasm_mutability_enum.CONST ? "const" : "";
            var some_const = some_mutability == (byte)wasm_mutability_enum.CONST ? "const" : "";
            Console.WriteLine($"`one` type: {one_const} {one_kind}"); // NOTE: f32 == "2"
            Console.WriteLine($"`some` type: {some_const} {some_kind}");

            Console.WriteLine("Getting global values...");
            wasm_val_t one_value;
            unsafe { wasm_global_get(one, &one_value); }
            Console.WriteLine($"`one` value: {one_value.of.f32}");

            wasm_val_t some_value;
            unsafe { wasm_global_get(some, &some_value); }
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            Console.WriteLine("Setting global values...\n");
            var one_set_value = WASM_F32_VAL(42);
            unsafe { wasm_global_set(one, &one_set_value); }

            var error_length = wasmer_last_error_length();
            if (error_length > 0) {
                Span<byte> error_message = stackalloc byte[(int)error_length];
                wasmer_last_error_message(ref MemoryMarshal.GetReference(error_message), error_length);
                var error_str = Encoding.UTF8.GetString(error_message.ToArray());
                Console.WriteLine($"Attempted to set an immutable global: `{error_str}`");
            }

            var some_set_value = WASM_F32_VAL(21);
            unsafe { wasm_global_set(some, &some_set_value); }

            // I think this is a bug in the C example and it means to either
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            // a) print some_set_value, or
            Console.WriteLine($"`some_set` value: {some_set_value.of.f32}");

            // b) re-get into some_value then print some_value
            unsafe { wasm_global_get(some, &some_value); }
            Console.WriteLine($"`some` value: {some_value.of.f32}");

            wasm_module_delete(module);
            wasm_extern_vec_delete(ref exports);
            wasm_instance_delete(instance);
            wasm_store_delete(store);
            wasm_engine_delete(engine);
        }
    }
}
