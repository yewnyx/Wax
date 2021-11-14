﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Wax.Imports.Data;

[assembly: InternalsVisibleTo("WaxExample")]

namespace Wax {
    namespace Imports {
        [System.AttributeUsage(System.AttributeTargets.Method)]
        class NotYetTestedAttribute : Attribute { }

        namespace Data {
            public enum valkind_t {
                I32 = 0,
                I64 = 1,
                F32 = 2,
                F64 = 3,
                ANYREF = 128,
                FUNCREF = 129
            }

            public enum externkind_t {
                FUNC = 0,
                GLOBAL = 1,
                TABLE = 2,
                MEMORY = 3
            }

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct vec_t {
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
            #region Engine
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void engine_delete(IntPtr engine);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_engine_new_with_config", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new_with_config(IntPtr config);
            #endregion
            #region Store
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr store_new(IntPtr engine);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_store_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void store_delete(IntPtr store);
            #endregion
            #region Module
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_new(IntPtr store, ref vec_t/*wasm_byte_vec_t*/ bytes);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_delete(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_copy(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool module_same(IntPtr module, IntPtr other);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_get_host_info(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_set_host_info(IntPtr module, IntPtr info);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_set_host_info_with_finalizer(IntPtr module, IntPtr info, IntPtr finalizer);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_as_ref(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_module", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_module(IntPtr @ref);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_as_ref_const(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_module_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_module_const(IntPtr @ref);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_shared_module_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void shared_module_delete(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_share", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_share(IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_obtain", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_obtain(IntPtr store, IntPtr module);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_new(IntPtr store, IntPtr binary);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_validate", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool module_validate(IntPtr store, ref vec_t/*wasm_byte_vec_t*/ binary);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_imports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_imports(IntPtr module, IntPtr @out);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_exports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_exports(IntPtr module, IntPtr @out);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_serialize", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_serialize(IntPtr store, ref vec_t/*wasm_byte_vec_t*/ @out);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_module_deserialize", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_deserialize(IntPtr store, ref vec_t/*wasm_byte_vec_t*/ binary);
            #endregion
            #region Instance
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_new(IntPtr store, IntPtr module, ref vec_t/*wasm_extern_vec_t*/ imports, IntPtr _/* TODO: trap** */);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_exports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_exports(IntPtr instance, ref vec_t/*wasm_extern_vec_t*/ exports);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_delete(IntPtr instance);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_copy(IntPtr instance);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool instance_same(IntPtr instance, IntPtr other);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_get_host_info(IntPtr instance);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_set_host_info(IntPtr instance, IntPtr info);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_set_host_info_with_finalizer(IntPtr instance, IntPtr info, IntPtr finalizer);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_as_ref(IntPtr instance);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_instance", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_instance(IntPtr @ref);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_instance_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_as_ref_const(IntPtr instance);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_instance_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_instance_const(IntPtr @ref);
            #endregion
            #region Byte, Name (WIP)
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new(ref vec_t/*wasm_byte_vec_t*/ vec, ulong length, ref byte buffer);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new_uninitialized(ref vec_t/*wasm_byte_vec_t*/ vec, ulong capacity);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_delete(ref vec_t/*wasm_byte_vec_t*/ vec);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_byte_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_copy(ref vec_t/*wasm_byte_vec_t*/ vec, ref vec_t/*wasm_byte_vec_t*/ vec2);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_new(ref vec_t/*wasm_byte_vec_t*/ vec, ulong length, ref byte buffer) {
                byte_vec_new(ref vec, length, ref buffer);
            }

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_new_uninitialized(ref vec_t/*wasm_byte_vec_t*/ vec, ulong capacity) {
                byte_vec_new_uninitialized(ref vec, capacity);
            }

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_delete(ref vec_t/*wasm_byte_vec_t*/ vec) {
                byte_vec_delete(ref vec);
            }

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_copy(ref vec_t/*wasm_byte_vec_t*/ vec, ref vec_t/*wasm_byte_vec_t*/ vec2) {
                byte_vec_copy(ref vec, ref vec2);
            }
            #endregion
            #region Extern
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_delete(ref vec_t/*wasm_extern_vec_t*/ vec);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_delete(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_copy(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool extern_same(IntPtr @extern, IntPtr other);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_get_host_info(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_set_host_info(IntPtr @extern, IntPtr info);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_set_host_info_with_finalizer(IntPtr @extern, IntPtr info, IntPtr finalizer);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_ref(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_extern(IntPtr @ref);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_ref_const(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_ref_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_extern_const(IntPtr @ref);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new_empty(ref vec_t/*wasm_extern_vec_t*/ vec);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new_uninitialized(ref vec_t/*wasm_extern_vec_t*/ vec, ulong capacity);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new(ref vec_t/*wasm_extern_vec_t*/ vec, ulong capacity, ref byte buffer);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_copy(ref vec_t/*wasm_extern_vec_t*/ vec, ref vec_t/*wasm_extern_vec_t*/ vec2);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_kind", CallingConvention = CallingConvention.Cdecl)]
            internal static extern /*externkind_t*/byte extern_kind(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_type(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_func_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_extern(IntPtr func);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_global_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_extern(IntPtr global);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_table_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_extern(IntPtr table);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_memory_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_extern(IntPtr memory);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_func", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_func(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_global", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_global(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_table", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_table(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_memory", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_memory(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_func_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_extern_const(IntPtr func);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_global_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_extern_const(IntPtr global);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_table_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_extern_const(IntPtr table);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_memory_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_extern_const(IntPtr memory);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_func_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_func_const(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_global_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_global_const(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_table_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_table_const(IntPtr @extern);

            [NotYetTested]
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_extern_as_memory_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_memory_const(IntPtr @extern);
            #endregion
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasm_func_call", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_call(IntPtr func, ref vec_t/*wasm_val_vec_t*/ vec, ref vec_t/*wasm_val_vec_t*/ results);
        }

        internal unsafe partial struct __wasmer {
            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_length();

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_message(ref byte buffer, ulong length);

            [SuppressUnmanagedCodeSecurity, DllImport("wasmer", EntryPoint = "wat2wasm", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void wat2wasm(ref vec_t/*wasm_byte_vec_t*/ wat, ref vec_t/*wasm_byte_vec_t*/ @out);
        }

        internal unsafe partial struct __wasi { }
    }
    namespace Bindings { }
}