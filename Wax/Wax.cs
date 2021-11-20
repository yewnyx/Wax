using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// TODO: Audit vec_new(..., ref /* ... */IntPtr array) function signatures for correctness

namespace Wax {
    namespace Imports {
        [System.AttributeUsage(System.AttributeTargets.Method)]
        class NotYetTestedAttribute : Attribute { }

        public enum wasm_mutability_enum {
            CONST = 0,
            VAR = 1,
        };

        public enum wasm_valkind_enum {
            I32 = 0,
            I64 = 1,
            F32 = 2,
            F64 = 3,
            ANYREF = 128,
            FUNCREF = 129
        }

        public enum wasm_externkind_enum {
            FUNC = 0,
            GLOBAL = 1,
            TABLE = 2,
            MEMORY = 3
        }

        [StructLayout(LayoutKind.Sequential, Size = 8)]
        public unsafe struct wasm_limits_t {
            public uint min;
            public uint max;

            public const uint max_default = 0xffffffff;
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_byte_vec_t {
            public ulong size;
            public IntPtr data;
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_valtype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_functype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_globaltype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_tabletype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_memorytype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_externtype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_importtype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_exporttype_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_val_vec_t {
            public ulong size;
            public IntPtr data;
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_frame_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_extern_vec_t {
            public ulong size;
            public IntPtr data; // *
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        public unsafe struct wasm_val_t {
            public /* valkind_t */byte kind;
            public val_union of;

            [StructLayout(LayoutKind.Explicit, Size = 8)]
            public unsafe struct val_union {
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

        [SuppressUnmanagedCodeSecurity]
        public static unsafe class __wasm {
            #region Byte, Name
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_byte_vec_new(ref wasm_byte_vec_t vec, ulong length, ref byte buffer);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_byte_vec_new_empty(ref wasm_byte_vec_t vec);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_byte_vec_new_uninitialized(ref wasm_byte_vec_t vec, ulong capacity);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_byte_vec_copy(ref wasm_byte_vec_t vec, ref wasm_byte_vec_t vec2);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_byte_vec_delete(ref wasm_byte_vec_t vec);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_name_new(ref wasm_byte_vec_t vec, ulong length, ref byte buffer) {
                wasm_byte_vec_new(ref vec, length, ref buffer);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_name_new_empty(ref wasm_byte_vec_t vec) {
                wasm_byte_vec_new_empty(ref vec);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_name_new_uninitialized(ref wasm_byte_vec_t vec, ulong capacity) {
                wasm_byte_vec_new_uninitialized(ref vec, capacity);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_name_delete(ref wasm_byte_vec_t vec) {
                wasm_byte_vec_delete(ref vec);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_name_copy(ref wasm_byte_vec_t vec, ref wasm_byte_vec_t vec2) {
                wasm_byte_vec_copy(ref vec, ref vec2);
            }
            #endregion
            #endregion
            #region Config
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_config_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_config_new();

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_config_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_config_delete(IntPtr config);
            #endregion
            #region Engine
            [DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_engine_new();

            [DllImport("wasmer", EntryPoint = "wasm_engine_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_engine_delete(IntPtr engine);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_engine_new_with_config", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_engine_new_with_config(IntPtr config);
            #endregion
            #region Store
            [DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_store_new(IntPtr engine);

            [DllImport("wasmer", EntryPoint = "wasm_store_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_store_delete(IntPtr store);
            #endregion
            #region Valtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_valtype_new(/*valkind_t*/byte valkind);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_valtype_copy(IntPtr valtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_delete(IntPtr valtype);

            [DllImport("wasmer", EntryPoint = "wasm_valtype_kind", CallingConvention = CallingConvention.Cdecl)]
            public static extern byte wasm_valtype_kind(IntPtr valtype);

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool wasm_valkind_is_num(wasm_valkind_enum k) { return k < wasm_valkind_enum.ANYREF; }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool wasm_valkind_is_ref(wasm_valkind_enum k) { return k >= wasm_valkind_enum.ANYREF; }

            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_vec_new(ref wasm_valtype_vec_t vec, ulong length, ref /* valtype_t* */IntPtr array);

            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_vec_new_empty(ref wasm_valtype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_vec_new_uninitialized(ref wasm_valtype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_vec_copy(ref wasm_valtype_vec_t vec, ref wasm_valtype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_valtype_vec_delete(ref wasm_valtype_vec_t vec);
            #endregion
            #endregion
            #region Functype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_new(ref wasm_valtype_vec_t @params, ref wasm_valtype_vec_t results);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_copy(IntPtr functype);

            [DllImport("wasmer", EntryPoint = "wasm_functype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_delete(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_params", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_params(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_results", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_results(IntPtr functype);
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_vec_new(ref wasm_functype_vec_t vec, ulong length, ref /* functype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_vec_new_empty(ref wasm_functype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_vec_new_uninitialized(ref wasm_functype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_vec_copy(ref wasm_functype_vec_t vec, ref wasm_functype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_functype_vec_delete(ref wasm_functype_vec_t vec);
            #endregion
            #endregion
            #region Globaltype
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_content", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_globaltype_content(IntPtr globaltype);

            [DllImport("wasmer", EntryPoint = "wasm_globaltype_mutability", CallingConvention = CallingConvention.Cdecl)]
            public static extern byte wasm_globaltype_mutability(IntPtr globaltype);
            #region new, copy, delete
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_globaltype_new(IntPtr valtype, byte mutability);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_globaltype_copy(IntPtr globaltype);

            [DllImport("wasmer", EntryPoint = "wasm_globaltype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_delete(IntPtr globaltype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_vec_new(ref wasm_globaltype_vec_t vec, ulong length, ref /* globaltype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_vec_new_empty(ref wasm_globaltype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_vec_new_uninitialized(ref wasm_globaltype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_vec_copy(ref wasm_globaltype_vec_t vec, ref wasm_globaltype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_globaltype_vec_delete(ref wasm_globaltype_vec_t vec);
            #endregion
            #endregion
            #region Tabletype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_element", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_tabletype_element(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_limits", CallingConvention = CallingConvention.Cdecl)]
            public static extern wasm_limits_t* wasm_tabletype_limits(IntPtr tabletype);

            [NotYetTested]
            #region new, copy, delete
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_tabletype_new(IntPtr valtype, ref wasm_limits_t limits);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_tabletype_copy(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_delete(IntPtr tabletype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_vec_new(ref wasm_tabletype_vec_t vec, ulong length, ref /* tabletype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_vec_new_empty(ref wasm_tabletype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_vec_new_uninitialized(ref wasm_tabletype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_vec_copy(ref wasm_tabletype_vec_t vec, ref wasm_tabletype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_tabletype_vec_delete(ref wasm_tabletype_vec_t vec);
            #endregion
            #endregion
            #region Memorytype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_limits", CallingConvention = CallingConvention.Cdecl)]
            public static extern wasm_limits_t* wasm_memorytype_limits(IntPtr memorytype);
            #region new, copy, delete
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memorytype_new(ref wasm_limits_t limits);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memorytype_copy(IntPtr memorytype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_delete(IntPtr memorytype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_vec_new(ref wasm_memorytype_vec_t vec, ulong length, ref /* memorytype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_vec_new_empty(ref wasm_memorytype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_vec_new_uninitialized(ref wasm_memorytype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_vec_copy(ref wasm_memorytype_vec_t vec, ref wasm_memorytype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memorytype_vec_delete(ref wasm_memorytype_vec_t vec);
            #endregion
            #endregion
            #region Externtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_kind", CallingConvention = CallingConvention.Cdecl)]
            public static extern byte wasm_externtype_kind(IntPtr externtype);
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_delete(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_copy(IntPtr externtype);
            #endregion
            #region Casts (externtype_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_functype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_functype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_globaltype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_globaltype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_tabletype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_tabletype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_memorytype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_memorytype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_functype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_functype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_globaltype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_globaltype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_tabletype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_tabletype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_memorytype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_externtype_as_memorytype_const(IntPtr externtype);
            #endregion
            #region Casts (as_externtype)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_as_externtype(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_globaltype_as_externtype(IntPtr globaltype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_tabletype_as_externtype(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memorytype_as_externtype(IntPtr memorytype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_functype_as_externtype_const(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_globaltype_as_externtype_const(IntPtr globaltype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_tabletype_as_externtype_const(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memorytype_as_externtype_const(IntPtr memorytype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_vec_new(ref wasm_externtype_vec_t vec, ulong length, ref /* externtype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_vec_new_empty(ref wasm_externtype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_vec_new_uninitialized(ref wasm_externtype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_vec_copy(ref wasm_externtype_vec_t vec, ref wasm_externtype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_externtype_vec_delete(ref wasm_externtype_vec_t vec);
            #endregion
            #endregion
            #region Importtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_importtype_new(IntPtr module, IntPtr name, IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_module", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_importtype_module(IntPtr importtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_name", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_importtype_name(IntPtr importtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_importtype_type(IntPtr importtype);
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void importtype_delete(IntPtr importtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_importtype_copy(IntPtr importtype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_importtype_vec_new(ref wasm_importtype_vec_t vec, ulong length, ref /* importtype* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_importtype_vec_new_empty(ref wasm_importtype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_importtype_vec_new_uninitialized(ref wasm_importtype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_importtype_vec_copy(ref wasm_importtype_vec_t vec, ref wasm_importtype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_importtype_vec_delete(ref wasm_importtype_vec_t vec);
            #endregion
            #endregion
            #region Exporttype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_exporttype_new(ref wasm_byte_vec_t name, IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_name", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_exporttype_name(IntPtr exporttype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_exporttype_type(IntPtr exporttype);

            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void exporttype_delete(IntPtr exporttype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_exporttype_copy(IntPtr exporttype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_exporttype_vec_new_empty(ref wasm_exporttype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_exporttype_vec_new_uninitialized(ref wasm_exporttype_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_exporttype_vec_new(ref wasm_exporttype_vec_t vec, ulong length, ref /* exporttype* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_exporttype_vec_copy(ref wasm_exporttype_vec_t vec, ref wasm_exporttype_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_exporttype_vec_delete(ref wasm_exporttype_vec_t vec);
            #endregion
            #endregion
            #region Val
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_val_delete(ref wasm_val_t val);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_val_copy(wasm_val_t* val, wasm_val_t* val2);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_val_vec_new_empty(ref wasm_val_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_val_vec_new_uninitialized(ref wasm_val_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_val_vec_new(ref wasm_val_vec_t vec, ulong length, wasm_val_t* array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_val_vec_copy(ref wasm_val_vec_t vec, ref wasm_val_vec_t vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_val_vec_delete(ref wasm_val_vec_t vec);
            #endregion
            #endregion
            #region Ref
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_ref_delete(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_copy(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_ref_same(IntPtr @ref, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_get_host_info(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_ref_set_host_info(IntPtr @ref, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_ref_set_host_info_with_finalizer(IntPtr @ref, IntPtr info, IntPtr finalizer);
            #endregion
            #endregion
            #region Frame
            [DllImport("wasmer", EntryPoint = "wasm_frame_instance", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_frame_instance(IntPtr frame);

            [DllImport("wasmer", EntryPoint = "wasm_frame_func_index", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint wasm_frame_func_index(IntPtr frame);

            [DllImport("wasmer", EntryPoint = "wasm_frame_func_offset", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_frame_func_offset(IntPtr frame);

            [DllImport("wasmer", EntryPoint = "wasm_frame_module_offset", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_frame_module_offset(IntPtr frame);
            #region delete, copy
            [DllImport("wasmer", EntryPoint = "wasm_frame_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_delete(IntPtr frame);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_frame_copy(IntPtr frame);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_vec_new(ref wasm_frame_vec_t vec, ulong length, ref /* frame* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_vec_new_empty(ref wasm_frame_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_vec_new_uninitialized(ref wasm_frame_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_vec_copy(ref wasm_frame_vec_t vec, ref wasm_frame_vec_t vec2);

            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_frame_vec_delete(ref wasm_frame_vec_t vec);
            #endregion
            #endregion
            #region Trap
            [DllImport("wasmer", EntryPoint = "wasm_trap_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_new(IntPtr store, ref wasm_byte_vec_t message);

            [DllImport("wasmer", EntryPoint = "wasm_trap_message", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_trap_message(IntPtr trap, ref wasm_byte_vec_t message);

            [DllImport("wasmer", EntryPoint = "wasm_trap_origin", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_origin(IntPtr trap);

            [DllImport("wasmer", EntryPoint = "wasm_trap_trace", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_trap_trace(IntPtr trap, ref wasm_frame_vec_t vec);
            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_trap_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_trap_delete(IntPtr trap);

            [DllImport("wasmer", EntryPoint = "wasm_trap_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_copy(IntPtr trap);

            [DllImport("wasmer", EntryPoint = "wasm_trap_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_trap_same(IntPtr trap, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_get_host_info(IntPtr trap);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_trap_set_host_info(IntPtr trap, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_trap_set_host_info_with_finalizer(IntPtr trap, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (trap_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_as_ref(IntPtr trap);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_trap_as_ref_const(IntPtr trap);
            #endregion
            #region Casts (as_trap)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_trap", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_trap(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_trap_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_trap_const(IntPtr @ref);
            #endregion
            #endregion
            #region Foreign
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_foreign_new(IntPtr store);
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_foreign_delete(IntPtr foreign);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_foreign_copy(IntPtr foreign);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_foreign_same(IntPtr foreign, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_foreign_get_host_info(IntPtr foreign);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_foreign_set_host_info(IntPtr foreign, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_foreign_set_host_info_with_finalizer(IntPtr foreign, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (foreign_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_foreign_as_ref(IntPtr foreign);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_foreign_as_ref_const(IntPtr foreign);
            #endregion
            #region Casts (as_foreign)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_foreign", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_foreign(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_foreign_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_foreign_const(IntPtr @ref);
            #endregion
            #endregion
            #region Module
            [DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_new(IntPtr store, ref wasm_byte_vec_t bytes);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_shared_module_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_shared_module_delete(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_share", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_share(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_obtain", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_obtain(IntPtr store, IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_validate", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_module_validate(IntPtr store, ref wasm_byte_vec_t binary);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_imports", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_imports(IntPtr module, ref wasm_importtype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_exports", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_exports(IntPtr module, ref wasm_exporttype_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_serialize", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_serialize(IntPtr store, ref wasm_byte_vec_t binary);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_deserialize", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_deserialize(IntPtr store, ref wasm_byte_vec_t binary);
            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_module_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_delete(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_copy(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_module_same(IntPtr module, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_get_host_info(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_set_host_info(IntPtr module, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_module_set_host_info_with_finalizer(IntPtr module, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (module_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_as_ref(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_module_as_ref_const(IntPtr module);
            #endregion
            #region Casts (as_module)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_module", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_module(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_module_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_module_const(IntPtr @ref);
            #endregion

            #endregion
            #region Func
            [DllImport("wasmer", EntryPoint = "wasm_func_call", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr/* wasm_trap_t */ wasm_func_call(IntPtr func, ref wasm_val_vec_t args, ref wasm_val_vec_t results);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_type(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_param_arity", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_func_param_arity(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_result_arity", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_func_result_arity(IntPtr func);

            public delegate IntPtr/* wasm_trap_t */ func_callback(ref wasm_val_vec_t args, ref wasm_val_vec_t results);
            public delegate IntPtr/* wasm_trap_t */ func_callback_with_env(void* env, ref wasm_val_vec_t args, wasm_val_vec_t results);

            [DllImport("wasmer", EntryPoint = "wasm_func_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_new(IntPtr store, IntPtr type, IntPtr func_callback);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_new_with_env", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_new_with_env(IntPtr store, IntPtr type, IntPtr func_callback_with_env, IntPtr env, IntPtr finalizer);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_func_delete(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_copy(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_func_same(IntPtr func, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_get_host_info(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_func_set_host_info(IntPtr func, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_func_set_host_info_with_finalizer(IntPtr func, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (func_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_as_ref(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_as_ref_const(IntPtr func);
            #endregion
            #region Casts (as_func)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_func", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_func(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_func_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_func_const(IntPtr @ref);
            #endregion
            #endregion
            #region Global
            [DllImport("wasmer", EntryPoint = "wasm_global_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_new(IntPtr store, IntPtr globaltype, ref wasm_val_t val); // TODO: AUDIT

            [DllImport("wasmer", EntryPoint = "wasm_global_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_type(IntPtr global);

            [DllImport("wasmer", EntryPoint = "wasm_global_get", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_global_get(IntPtr global, wasm_val_t* val);

            [DllImport("wasmer", EntryPoint = "wasm_global_set", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_global_set(IntPtr global, wasm_val_t* val);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_global_delete(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_copy(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_global_same(IntPtr global, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_get_host_info(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_global_set_host_info(IntPtr global, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_global_set_host_info_with_finalizer(IntPtr global, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (global_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_as_ref(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_as_ref_const(IntPtr global);
            #endregion
            #region Casts (as_global)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_global", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_global(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_global_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_global_const(IntPtr @ref);
            #endregion
            #endregion
            #region Table
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_new(IntPtr store, IntPtr tableType, IntPtr init);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_type(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_get", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_get(IntPtr table, uint index);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_table_set(IntPtr table, uint index, IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_size", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint wasm_table_size(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_grow", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_table_grow(IntPtr table, uint delta, IntPtr init);
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_table_delete(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_copy(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_table_same(IntPtr table, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_get_host_info(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_table_set_host_info(IntPtr table, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_table_set_host_info_with_finalizer(IntPtr table, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (table_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_as_ref(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_as_ref_const(IntPtr table);
            #endregion
            #region Casts (as_table)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_table", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_table(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_table_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_table_const(IntPtr @ref);
            #endregion
            #endregion
            #region Memory
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_new(IntPtr store, IntPtr memorytype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_type(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_data", CallingConvention = CallingConvention.Cdecl)]
            public static extern sbyte* wasm_memory_data(IntPtr memory); // TODO: AUDIT

            [DllImport("wasmer", EntryPoint = "wasm_memory_data_size", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasm_memory_data_size(IntPtr memory);

            [DllImport("wasmer", EntryPoint = "wasm_memory_size", CallingConvention = CallingConvention.Cdecl)]
            public static extern uint wasm_memory_size(IntPtr memory);

            [DllImport("wasmer", EntryPoint = "wasm_memory_grow", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_memory_grow(IntPtr memory, uint delta);
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memory_delete(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_copy(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_memory_same(IntPtr memory, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_get_host_info(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memory_set_host_info(IntPtr memory, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_memory_set_host_info_with_finalizer(IntPtr memory, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (memory_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_as_ref(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_as_ref_const(IntPtr memory);
            #endregion
            #region Casts (as_memory)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_memory", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_memory(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_memory_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_memory_const(IntPtr @ref);
            #endregion
            #endregion
            #region Extern
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_kind", CallingConvention = CallingConvention.Cdecl)]
            public static extern /* wasm_externkind_enum */byte wasm_extern_kind(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_type", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_type(IntPtr @extern);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_delete(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_copy(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_extern_same(IntPtr @extern, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_get_host_info(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_set_host_info(IntPtr @extern, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_set_host_info_with_finalizer(IntPtr @extern, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (extern_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_ref(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_ref_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_func", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_func(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_func_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_func_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_global", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_global(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_global_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_global_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_table", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_table(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_table_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_table_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_memory", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_memory(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_memory_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_extern_as_memory_const(IntPtr @extern);
            #endregion
            #region Casts (as_extern)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_extern", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_extern(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_extern_const(IntPtr @ref);

            [DllImport("wasmer", EntryPoint = "wasm_func_as_extern", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_as_extern(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_func_as_extern_const(IntPtr func);

            [DllImport("wasmer", EntryPoint = "wasm_global_as_extern", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_as_extern(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_global_as_extern_const(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_extern", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_as_extern(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_table_as_extern_const(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_extern", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_as_extern(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_memory_as_extern_const(IntPtr memory);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_vec_new(ref wasm_extern_vec_t vec, ulong capacity, ref /* extern_t* */IntPtr array);

            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_vec_new_empty(ref wasm_extern_vec_t vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_vec_new_uninitialized(ref wasm_extern_vec_t vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_vec_copy(ref wasm_extern_vec_t vec, ref wasm_extern_vec_t vec2);

            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_extern_vec_delete(ref wasm_extern_vec_t vec);
            #endregion
            #endregion
            #region Instance
            [DllImport("wasmer", EntryPoint = "wasm_instance_new", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_new(IntPtr store, IntPtr module, ref wasm_extern_vec_t imports, IntPtr _/* TODO: trap** */);

            [DllImport("wasmer", EntryPoint = "wasm_instance_exports", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_exports(IntPtr instance, ref wasm_extern_vec_t exports);

            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_instance_delete", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_instance_delete(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_copy", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_copy(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            public static extern bool wasm_instance_same(IntPtr instance, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_get_host_info(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_instance_set_host_info(IntPtr instance, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wasm_instance_set_host_info_with_finalizer(IntPtr instance, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (instance_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_as_ref", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_as_ref(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_instance_as_ref_const(IntPtr instance);
            #endregion
            #region Casts (as_instance)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_instance", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_instance(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_instance_const", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr wasm_ref_as_instance_const(IntPtr @ref);
            #endregion
            #endregion
            #region Valtype helpers
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr wasm_valtype_new_i32() {
                return wasm_valtype_new((byte)wasm_valkind_enum.I32);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr valtype_new_i64() {
                return wasm_valtype_new((byte)wasm_valkind_enum.I64);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr wasm_valtype_new_f32() {
                return wasm_valtype_new((byte)wasm_valkind_enum.F32);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr valtype_new_f64() {
                return wasm_valtype_new((byte)wasm_valkind_enum.F64);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr valtype_new_anyref() {
                return wasm_valtype_new((byte)wasm_valkind_enum.ANYREF);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr valtype_new_funcref() {
                return wasm_valtype_new((byte)wasm_valkind_enum.FUNCREF);
            }
            #endregion
            #region Functype helpers
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_0_0() {
                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new_empty(ref @params);

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new_empty(ref results);

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_1_0(
              ref /* valtype_t* */IntPtr p) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new_empty(ref results);

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_2_0(
              ref /* valtype_t* */IntPtr p1,
              ref /* valtype_t* */IntPtr p2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new_empty(ref results);

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_3_0(
              ref /* valtype_t* */IntPtr p1,
              ref /* valtype_t* */IntPtr p2,
              ref /* valtype_t* */IntPtr p3) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new_empty(ref results);

                return wasm_functype_new(ref @params, ref results);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_0_1(
              ref /* valtype_t* */IntPtr r
            ) {
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new_empty(ref @params);

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_1_1(
              ref /* valtype_t* */IntPtr p,
              ref /* valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_2_1(
                ref /* valtype_t* */IntPtr p1,
                ref /* valtype_t* */IntPtr p2,
                ref /* valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_3_1(
              ref /* valtype_t* */IntPtr p1,
              ref /* valtype_t* */IntPtr p2,
              ref /* valtype_t* */IntPtr p3,
              ref /* valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_0_2(
              ref /* valtype_t* */IntPtr r1,
              ref /* valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new_empty(ref @params);

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);

            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_1_2(
              ref /* valtype_t* */IntPtr p,
              ref /* valtype_t* */IntPtr r1,
              ref /* valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_2_2(
              ref /* valtype_t* */IntPtr p1,
              ref /* valtype_t* */IntPtr p2,
              ref /* valtype_t* */IntPtr r1,
              ref /* valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr/* functype_t* */ wasm_functype_new_3_2(
              ref /* valtype_t* */IntPtr p1,
              ref /* valtype_t* */IntPtr p2,
              ref /* valtype_t* */IntPtr p3,
              ref /* valtype_t* */IntPtr r1,
              ref /* valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                wasm_valtype_vec_t @params = default;
                wasm_valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                wasm_valtype_vec_t results = default;
                wasm_valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return wasm_functype_new(ref @params, ref results);
            }
            #endregion
            #region Val helpers
            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void wasm_val_init_ptr(ref wasm_val_t @out, IntPtr p) {
                // I don't care about 32-bit, but here's where you'd fix the conditional if you wanted to.
#if false
                @out.kind = (byte)wasm_valkind_enum.I32;
                @out.of.i64 = p.ToInt32();
#else
                @out.kind = (byte)wasm_valkind_enum.I64;
                @out.of.i64 = p.ToInt64();
#endif
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static IntPtr wasm_val_ptr(ref wasm_val_t val) {
#if false
                return (IntPtr)val.of.i32;
#else
                return (IntPtr)val.of.i64;
#endif
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_I32_VAL(int i) {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.I32,
                    of = new wasm_val_t.val_union { i32 = i }
                };
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_I64_VAL(long i) {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.I64,
                    of = new wasm_val_t.val_union { i64 = i }
                };
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_F32_VAL(float z) {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.F32,
                    of = new wasm_val_t.val_union { f32 = z }
                };
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_F64_VAL(double z) {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.F64,
                    of = new wasm_val_t.val_union { f64 = z }
                };
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_REF_VAL(IntPtr r) {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.ANYREF,
                    of = new wasm_val_t.val_union { @ref = r }
                };
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_t WASM_INIT_VAL() {
                return new wasm_val_t {
                    kind = (byte)wasm_valkind_enum.ANYREF,
                    of = new wasm_val_t.val_union { @ref = IntPtr.Zero }
                };
            }
            #endregion
            #region Vec helpers
            #region Array initializers (Add as needed)
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_vec_t WASM_ARRAY_VEC(Span<wasm_val_t> span) {
                wasm_val_vec_t vec = default;
                vec.size = (ulong)span.Length;
                unsafe {
                    fixed (void* arg0 = &span[0]) {
                        vec.data = (IntPtr)arg0;
                    }
                }
                return vec;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_extern_vec_t WASM_ARRAY_EXTERN_VEC(Span<IntPtr> span) {
                wasm_extern_vec_t vec = default;
                vec.size = (ulong)span.Length;
                unsafe {
                    fixed (void* arg0 = &span[0]) {
                        vec.data = (IntPtr)arg0;
                    }
                }
                return vec;
            }
            #endregion
            #region Empty Vecs
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_exporttype_vec_t WASM_EMPTY_EXPORTTYPE_VEC() {
                return new wasm_exporttype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_extern_vec_t WASM_EMPTY_EXTERN_VEC() {
                return new wasm_extern_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_externtype_vec_t WASM_EMPTY_EXTERNTYPE_VEC() {
                return new wasm_externtype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_frame_vec_t WASM_EMPTY_FRAME_VEC() {
                return new wasm_frame_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_functype_vec_t WASM_EMPTY_FUNCTYPE_VEC() {
                return new wasm_functype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_globaltype_vec_t WASM_EMPTY_GLOBALTYPE_VEC() {
                return new wasm_globaltype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_importtype_vec_t WASM_EMPTY_IMPORTTYPE_VEC() {
                return new wasm_importtype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_memorytype_vec_t WASM_EMPTY_MEMORYTYPE_VEC() {
                return new wasm_memorytype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_tabletype_vec_t WASM_EMPTY_TABLETYPE_VEC() {
                return new wasm_tabletype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_val_vec_t WASM_EMPTY_VAL_VEC() {
                return new wasm_val_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_valtype_vec_t WASM_EMPTY_VEC_VALTYPE() {
                return new wasm_valtype_vec_t { size = 0, data = IntPtr.Zero };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static wasm_byte_vec_t WASM_EMPTY_BYTE_VEC() {
                return new wasm_byte_vec_t { size = 0, data = IntPtr.Zero };
            }
            #endregion
            #endregion
        }

        [SuppressUnmanagedCodeSecurity]
        public static unsafe class __wasmer {
            [DllImport("wasmer", EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong last_error_length();

            [DllImport("wasmer", EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong last_error_message(ref byte buffer, ulong length);

            [DllImport("wasmer", EntryPoint = "wat2wasm", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wat2wasm(ref wasm_byte_vec_t wat, ref wasm_byte_vec_t binary);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_canonicalize_nans", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_canonicalize_nans(IntPtr config, bool enable);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_push_middleware", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_push_middleware(IntPtr config, IntPtr middleware);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_compiler", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_compiler(IntPtr config, global::Wasm.WasmerCompilerT compiler);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_engine", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_engine(IntPtr config, global::Wasm.WasmerEngineT engine);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_features", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_features(IntPtr config, IntPtr features);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_target", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_target(IntPtr config, IntPtr target);

        }

        [SuppressUnmanagedCodeSecurity]
        public static unsafe class __wasi { }
    }
    namespace Bindings { }
}