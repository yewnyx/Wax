using System;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.WaxExample {
    class WaxExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_str = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_str), error_len);
            Console.Error.WriteLine(error_str.ToString());
        }

        static void Main(string[] args) {
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
            byte_vec_t wat = default;
            __wasm.byte_vec_new(ref wat, (ulong)wat_string_utf8.Length, ref MemoryMarshal.GetReference(wat_string_utf8.AsSpan()));
            byte_vec_t wasm_bytes = default;
            __wasmer.wat2wasm(ref wat, ref wasm_bytes);


            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);
            var module = __wasm.module_new(store, ref wasm_bytes);
            if (module == null) {
                print_wasmer_error();
                Environment.Exit(1);
            }

            extern_vec_t imports = new extern_vec_t { size = 0, data = IntPtr.Zero };
            var instance = __wasm.instance_new(store, module, ref imports, IntPtr.Zero);

            // Pick work back up here


            // Delete this in next commit, just leaving it here for one commit for posterity, to be moved into a unit test or something later
            {
                byte_vec_t bv = default;
                __wasm.byte_vec_new_uninitialized(ref bv, 10);
                byte_vec_t bv2 = default;
                byte_vec_t bv3 = default;
                __wasm.byte_vec_copy(ref bv2, ref bv);
                __wasm.byte_vec_delete(ref bv);
                __wasm.byte_vec_copy(ref bv3, ref bv2);
                __wasm.byte_vec_delete(ref bv3);
                __wasm.byte_vec_delete(ref bv2);
            }
        }
    }
}
