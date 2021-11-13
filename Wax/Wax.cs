using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

[assembly: InternalsVisibleTo("WaxExample")]

namespace Wax.Wasm.Bindings {
    internal unsafe partial struct __wasm {
        [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr engine_new();
    }

    internal unsafe partial struct __wasi {}
}