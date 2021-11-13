using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: InternalsVisibleTo("WaxExample")]

namespace Wax {
    namespace Imports {
        internal unsafe partial struct __wasm {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new();


            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr store_new(IntPtr engine);
        }

        internal unsafe partial struct __wasmer {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            internal static extern int last_error_length();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            internal static extern int last_error_message(ref char buffer, int length);
        }

        internal unsafe partial struct __wasi { }
    }
    namespace Bindings { }
}