using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Wax.Imports.Data;

[assembly: InternalsVisibleTo("WaxExample")]

namespace Wax {
    namespace Imports {
        namespace Data {
            public enum valkind_t {
                I32 = 0,
                I64 = 1,
                F32 = 2,
                F64 = 3,
                ANYREF = 128,
                FUNCREF = 129
            }

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

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct val_vec_t {
                public ulong size;
                public IntPtr data;
            }

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct val_t {
                public /*valkind_t*/ byte kind;
                public val_union of;

                [StructLayout(LayoutKind.Explicit, Size = 8)]
                public struct val_union {
                    [FieldOffset(0)]
                    public int i32;

                    [FieldOffset(0)]
                    public long i64;

                    [FieldOffset(0)]
                    public float f32;

                    [FieldOffset(0)]
                    public double f64;

                    [FieldOffset(0)]
                    public IntPtr @ref;
                }
            }
        }

        internal unsafe partial struct __wasm {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void engine_delete(IntPtr engine);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr store_new(IntPtr engine);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void store_delete(IntPtr store);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_new(IntPtr store, ref byte_vec_t binary);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_delete(IntPtr module);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_new(IntPtr store, IntPtr module, ref extern_vec_t imports, IntPtr _);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_delete(IntPtr instance);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_exports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_exports(IntPtr instance, ref extern_vec_t exports);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new(ref byte_vec_t vec, ulong length, ref byte buffer);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new_uninitialized(ref byte_vec_t vec, ulong capacity);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_delete(ref byte_vec_t vec);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_copy(ref byte_vec_t vec, ref byte_vec_t vec2);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_func", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_func(IntPtr @extern);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_delete(ref extern_vec_t vec);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_func_call", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_call(IntPtr func, ref val_vec_t vec, ref val_vec_t results);
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