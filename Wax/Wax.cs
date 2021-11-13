using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Wax.Imports.Data;

[assembly: InternalsVisibleTo("WaxExample")]

namespace Wax {
    namespace Imports {
        namespace Data {
            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct byte_vec_t {
                public ulong size;
                public IntPtr data;
            }

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct extern_vec_t {
                public ulong size;
                public IntPtr data;
            }
        }

        internal unsafe partial struct __wasm {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr store_new(IntPtr engine);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_new(IntPtr store, ref byte_vec_t binary);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_new(IntPtr store, IntPtr module, ref extern_vec_t imports, IntPtr _);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new(ref byte_vec_t vec, ulong length, ref byte buffer);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new_uninitialized(ref byte_vec_t vec, ulong capacity);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_delete(ref byte_vec_t vec);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_copy(ref byte_vec_t vec, ref byte_vec_t vec2);
        }

        internal unsafe partial struct __wasmer {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_length();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_message(ref byte buffer, ulong length);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wat2wasm", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void wat2wasm(ref byte_vec_t wat, ref byte_vec_t @out);

        }

        internal unsafe partial struct __wasi { }
    }
    namespace Bindings { }
}