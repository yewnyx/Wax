using System;
using System.Runtime.InteropServices;
using Wax.Imports;

namespace Wax.WaxExample {
    class WaxExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<char> error_str = stackalloc char[error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_str), error_len);
        }

        static void Main(string[] args) {
            var engine = __wasm.engine_new();
            var store = __wasm.store_new(engine);
        }
    }
}
