﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.Examples {
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
}