using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using Wax.Imports.Data;

[assembly: InternalsVisibleTo("EarlyExit")]
[assembly: InternalsVisibleTo("ExportsGlobal")]
[assembly: InternalsVisibleTo("ImportExport")]
[assembly: InternalsVisibleTo("Instance")]
[assembly: InternalsVisibleTo("Memory")]

namespace Wax {
    namespace Imports {
        [System.AttributeUsage(System.AttributeTargets.Method)]
        class NotYetTestedAttribute : Attribute { }

        namespace Data {
            public enum mutability_t {
                CONST = 0,
                VAR = 1,
            };

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

            [StructLayout(LayoutKind.Sequential, Size = 8)]
            public struct limits_t {
                public uint min;
                public uint max;

                public const uint max_default = 0xffffffff;
            }

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct vec_t {
                public ulong size;
                public IntPtr data;
            }

            [StructLayout(LayoutKind.Sequential, Size = 16)]
            public struct val_t {
                public /* valkind_t */byte kind;
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

        [SuppressUnmanagedCodeSecurity]
        internal static class __wasm {
            #region Byte, Name
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new(ref vec_t/* byte_vec_t */ vec, ulong length, ref byte buffer);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new_empty(ref vec_t/* byte_vec_t */ vec);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_new_uninitialized(ref vec_t/* byte_vec_t */ vec, ulong capacity);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_copy(ref vec_t/* byte_vec_t */ vec, ref vec_t/* byte_vec_t */ vec2);

            [DllImport("wasmer", EntryPoint = "wasm_byte_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void byte_vec_delete(ref vec_t/* byte_vec_t */ vec);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_new(ref vec_t/* byte_vec_t */ vec, ulong length, ref byte buffer) {
                byte_vec_new(ref vec, length, ref buffer);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_new_empty(ref vec_t/* byte_vec_t */ vec) {
                byte_vec_new_empty(ref vec);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_new_uninitialized(ref vec_t/* byte_vec_t */ vec, ulong capacity) {
                byte_vec_new_uninitialized(ref vec, capacity);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_delete(ref vec_t/* byte_vec_t */ vec) {
                byte_vec_delete(ref vec);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static void name_copy(ref vec_t/* byte_vec_t */ vec, ref vec_t/* byte_vec_t */ vec2) {
                byte_vec_copy(ref vec, ref vec2);
            }
            #endregion
            #endregion
            #region Config
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_config_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr config_new();

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_config_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void config_delete(IntPtr config);
            #endregion
            #region Engine
            [DllImport("wasmer", EntryPoint = "wasm_engine_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new();

            [DllImport("wasmer", EntryPoint = "wasm_engine_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void engine_delete(IntPtr engine);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_engine_new_with_config", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr engine_new_with_config(IntPtr config);
            #endregion
            #region Store
            [DllImport("wasmer", EntryPoint = "wasm_store_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr store_new(IntPtr engine);

            [DllImport("wasmer", EntryPoint = "wasm_store_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void store_delete(IntPtr store);
            #endregion
            #region Valtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr valtype_new(/*valkind_t*/byte valkind);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr valtype_copy(IntPtr valtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_delete(IntPtr valtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_kind", CallingConvention = CallingConvention.Cdecl)]
            internal static extern byte valtype_kind(IntPtr valtype);

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool valkind_is_num(valkind_t k) { return k < valkind_t.ANYREF; }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool valkind_is_ref(valkind_t k) { return k >= valkind_t.ANYREF; }

            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_vec_new(ref vec_t/* valtype_vec_t */ vec, ulong length, ref /* valtype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_vec_new_empty(ref vec_t/* valtype_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_vec_new_uninitialized(ref vec_t/* valtype_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_vec_copy(ref vec_t/* valtype_vec_t */ vec, ref vec_t/* valtype_vec_t */ vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_valtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void valtype_vec_delete(ref vec_t/* valtype_vec_t */ vec);
            #endregion
            #endregion
            #region Functype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_new(ref vec_t/* valtype_vec_t */ @params, ref vec_t/* valtype_vec_t */ results);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_copy(IntPtr functype);

            [DllImport("wasmer", EntryPoint = "wasm_functype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_delete(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_params", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_params(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_results", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_results(IntPtr functype);
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_vec_new(ref vec_t/* functype_vec_t */ vec, ulong length, ref /* functype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_vec_new_empty(ref vec_t/* functype_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_vec_new_uninitialized(ref vec_t/* functype_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_vec_copy(ref vec_t/* functype_vec_t */ vec, ref vec_t/* functype_vec_t */ vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void functype_vec_delete(ref vec_t/* functype_vec_t */ vec);
            #endregion
            #endregion
            #region Globaltype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_content", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr globaltype_content(IntPtr globaltype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_mutability", CallingConvention = CallingConvention.Cdecl)]
            internal static extern byte globaltype_mutability(IntPtr globaltype);
            #region new, copy, delete
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr globaltype_new(IntPtr valtype, byte mutability);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr globaltype_copy(IntPtr globaltype);

            [DllImport("wasmer", EntryPoint = "wasm_globaltype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_delete(IntPtr globaltype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_vec_new(ref vec_t/* globaltype_vec_t */ vec, ulong length, ref /* globaltype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_vec_new_empty(ref vec_t/* globaltype_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_vec_new_uninitialized(ref vec_t/* globaltype_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_vec_copy(ref vec_t/* globaltype_vec_t */ vec, ref vec_t/* globaltype_vec_t */ vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void globaltype_vec_delete(ref vec_t/* globaltype_vec_t */ vec);
            #endregion
            #endregion
            #region Tabletype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_element", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr tabletype_element(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_limits", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern limits_t* tabletype_limits(IntPtr tabletype);

            [NotYetTested]
            #region new, copy, delete
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr tabletype_new(IntPtr valtype, ref limits_t limits);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr tabletype_copy(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_delete(IntPtr tabletype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_vec_new(ref vec_t/* tabletype_vec_t */ vec, ulong length, ref /* tabletype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_vec_new_empty(ref vec_t/* tabletype_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_vec_new_uninitialized(ref vec_t/* tabletype_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_vec_copy(ref vec_t/* tabletype_vec_t */ vec, ref vec_t/* tabletype_vec_t */ vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void tabletype_vec_delete(ref vec_t/* tabletype_vec_t */ vec);
            #endregion
            #endregion
            #region Memorytype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_limits", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern limits_t* memorytype_limits(IntPtr memorytype);
            #region new, copy, delete
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memorytype_new(ref limits_t limits);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memorytype_copy(IntPtr memorytype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_delete(IntPtr memorytype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_vec_new(ref vec_t/* memorytype_vec_t */ vec, ulong length, ref /* memorytype_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_vec_new_empty(ref vec_t/* memorytype_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_vec_new_uninitialized(ref vec_t/* memorytype_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_vec_copy(ref vec_t/* memorytype_vec_t */ vec, ref vec_t/* memorytype_vec_t */ vec2);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memorytype_vec_delete(ref vec_t/* memorytype_vec_t */ vec);
            #endregion
            #endregion
            #region Externtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_kind", CallingConvention = CallingConvention.Cdecl)]
            internal static extern byte externtype_kind(IntPtr externtype);
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_delete(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_copy(IntPtr _0);
            #endregion
            #region Casts (externtype_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_functype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_functype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_globaltype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_globaltype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_tabletype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_tabletype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_memorytype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_memorytype(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_functype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_functype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_globaltype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_globaltype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_tabletype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_tabletype_const(IntPtr externtype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_as_memorytype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr externtype_as_memorytype_const(IntPtr externtype);
            #endregion
            #region Casts (as_externtype)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_as_externtype(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr globaltype_as_externtype(IntPtr globaltype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr tabletype_as_externtype(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_as_externtype", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memorytype_as_externtype(IntPtr memorytype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_functype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr functype_as_externtype_const(IntPtr functype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_globaltype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr globaltype_as_externtype_const(IntPtr globaltype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_tabletype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr tabletype_as_externtype_const(IntPtr tabletype);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memorytype_as_externtype_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memorytype_as_externtype_const(IntPtr memorytype);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_vec_new(IntPtr @out, ulong _0, IntPtr[] _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_vec_new_empty(IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_vec_new_uninitialized(IntPtr @out, ulong _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_vec_copy(IntPtr @out, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_externtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void externtype_vec_delete(IntPtr _0);
            #endregion
            #endregion
            #region Importtype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr importtype_new(IntPtr module, IntPtr name, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_module", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr importtype_module(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_name", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr importtype_name(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr importtype_type(IntPtr _0);
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_delete(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr importtype_copy(IntPtr _0);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_vec_new(IntPtr @out, ulong _0, IntPtr[] _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_vec_new_empty(IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_vec_new_uninitialized(IntPtr @out, ulong _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_vec_copy(IntPtr @out, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_importtype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void importtype_vec_delete(IntPtr _0);
            #endregion
            #endregion
            #region Exporttype
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr exporttype_new(IntPtr _0, IntPtr _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_name", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr exporttype_name(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr exporttype_type(IntPtr _0);

            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_delete(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr exporttype_copy(IntPtr _0);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_vec_new_empty(IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_vec_new_uninitialized(IntPtr @out, ulong _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_vec_new(IntPtr @out, ulong _0, IntPtr[] _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_vec_copy(IntPtr @out, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_exporttype_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void exporttype_vec_delete(IntPtr _0);
            #endregion
            #endregion
            #region Val
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr val_delete(ref val_t val);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern ulong val_copy(val_t* val, val_t* val2);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void val_vec_new_empty(IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void val_vec_new_uninitialized(IntPtr @out, ulong _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern void val_vec_new(IntPtr @out, ulong _0, val_t* _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void val_vec_copy(IntPtr @out, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_val_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void val_vec_delete(IntPtr _0);
            #endregion
            #endregion
            #region Ref
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ref_delete(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_copy(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool ref_same(IntPtr _0, IntPtr _1);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_get_host_info(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ref_set_host_info(IntPtr @ref, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void ref_set_host_info_with_finalizer(IntPtr @ref, IntPtr info, IntPtr finalizer);
            #endregion
            #endregion
            #region Frame
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_instance", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr frame_instance(IntPtr frame);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_func_index", CallingConvention = CallingConvention.Cdecl)]
            internal static extern uint frame_func_index(IntPtr frame);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_func_offset", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong frame_func_offset(IntPtr frame);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_module_offset", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong frame_module_offset(IntPtr frame);
            #region delete, copy
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_delete(IntPtr frame);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr frame_copy(IntPtr frame);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_vec_new(IntPtr @out, ulong _0, IntPtr[] _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_vec_new_empty(IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_vec_new_uninitialized(IntPtr @out, ulong _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_vec_copy(IntPtr @out, IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_frame_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void frame_vec_delete(ref vec_t/* frame_vec_t */ vec);
            #endregion
            #endregion
            #region Trap
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_new(IntPtr store, ref vec_t/* byte_vec_t */ message);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_message", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void trap_message(IntPtr trap, ref vec_t/* byte_vec_t */ message);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_origin", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_origin(IntPtr trap);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_trace", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void trap_trace(IntPtr trap, ref vec_t/* frame_vec_t */ vec);
            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_trap_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void trap_delete(IntPtr trap);

            [DllImport("wasmer", EntryPoint = "wasm_trap_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_copy(IntPtr trap);

            [DllImport("wasmer", EntryPoint = "wasm_trap_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool trap_same(IntPtr trap, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_get_host_info(IntPtr trap);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void trap_set_host_info(IntPtr trap, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void trap_set_host_info_with_finalizer(IntPtr trap, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (trap_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_as_ref(IntPtr trap);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_trap_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr trap_as_ref_const(IntPtr trap);
            #endregion
            #region Casts (as_trap)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_trap", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_trap(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_trap_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_trap_const(IntPtr @ref);
            #endregion
            #endregion
            #region Foreign
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_foreign_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr foreign_new(IntPtr _0);
            #region delete, copy, same
            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void foreign_delete(IntPtr _0);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr foreign_copy(IntPtr _0);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool foreign_same(IntPtr _0, IntPtr _1);
            #endregion
            #region host info
            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr foreign_get_host_info(IntPtr _0);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void foreign_set_host_info(IntPtr _0, IntPtr _1);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void foreign_set_host_info_with_finalizer(IntPtr _0, IntPtr _1, IntPtr _2);
            #endregion
            #region Casts (foreign_as)
            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr foreign_as_ref(IntPtr _0);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_foreign_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr foreign_as_ref_const(IntPtr _0);
            #endregion
            #region Casts (as_foreign)
            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_ref_as_foreign", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_foreign(IntPtr _0);

            [NotYetTested][DllImport("wasmer", EntryPoint = "wasm_ref_as_foreign_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_foreign_const(IntPtr _0);
            #endregion
            #endregion
            #region Module
            [DllImport("wasmer", EntryPoint = "wasm_module_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_new(IntPtr store, ref vec_t/* byte_vec_t */ bytes);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_shared_module_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void shared_module_delete(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_share", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_share(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_obtain", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_obtain(IntPtr store, IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_validate", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool module_validate(IntPtr store, ref vec_t/* byte_vec_t */ binary);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_imports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_imports(IntPtr module, IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_exports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_exports(IntPtr module, IntPtr @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_serialize", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_serialize(IntPtr store, ref vec_t/* byte_vec_t */ @out);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_deserialize", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_deserialize(IntPtr store, ref vec_t/* byte_vec_t */ binary);
            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_module_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_delete(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_copy(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool module_same(IntPtr module, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_get_host_info(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_set_host_info(IntPtr module, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void module_set_host_info_with_finalizer(IntPtr module, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (module_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_as_ref(IntPtr module);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_module_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr module_as_ref_const(IntPtr module);
            #endregion
            #region Casts (as_module)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_module", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_module(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_module_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_module_const(IntPtr @ref);
            #endregion

            #endregion
            #region Func
            [DllImport("wasmer", EntryPoint = "wasm_func_call", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_call(IntPtr func, ref vec_t/* val_vec_t */ vec, ref vec_t/* val_vec_t */ results);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_type(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_param_arity", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong func_param_arity(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_result_arity", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong func_result_arity(IntPtr func);

            public unsafe delegate IntPtr/* trap_t */ func_callback(ref vec_t/* val_vec_t */ args, ref vec_t/* val_vec_t */ results);
            public unsafe delegate IntPtr/* trap_t */ func_callback_with_env(void* env, ref vec_t/* val_vec_t */ args, vec_t/* val_vec_t */ results);

            [DllImport("wasmer", EntryPoint = "wasm_func_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_new(IntPtr store, IntPtr type, IntPtr func_callback);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_new_with_env", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_new_with_env(IntPtr store, IntPtr type, IntPtr func_callback_with_env, IntPtr env, IntPtr finalizer);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void func_delete(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_copy(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool func_same(IntPtr func, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_get_host_info(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void func_set_host_info(IntPtr func, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void func_set_host_info_with_finalizer(IntPtr func, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (func_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_ref(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_ref_const(IntPtr func);
            #endregion
            #region Casts (as_func)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_func", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_func(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_func_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_func_const(IntPtr @ref);
            #endregion
            #endregion
            #region Global
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_new(IntPtr store, IntPtr globaltype, ref val_t val); // TODO: AUDIT

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_type(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_get", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern void global_get(IntPtr global, val_t* val);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_set", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern void global_set(IntPtr global, val_t* val);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void global_delete(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_copy(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool global_same(IntPtr global, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_get_host_info(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void global_set_host_info(IntPtr global, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void global_set_host_info_with_finalizer(IntPtr global, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (global_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_ref(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_ref_const(IntPtr global);
            #endregion
            #region Casts (as_global)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_global", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_global(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_global_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_global_const(IntPtr @ref);
            #endregion
            #endregion
            #region Table
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_new(IntPtr _0, IntPtr _1, IntPtr init);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_type(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_get", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_get(IntPtr _0, uint index);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool table_set(IntPtr _0, uint index, IntPtr _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_size", CallingConvention = CallingConvention.Cdecl)]
            internal static extern uint table_size(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_grow", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool table_grow(IntPtr _0, uint delta, IntPtr init);
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void table_delete(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_copy(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool table_same(IntPtr table, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_get_host_info(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void table_set_host_info(IntPtr table, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void table_set_host_info_with_finalizer(IntPtr table, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (table_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_ref(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_ref_const(IntPtr table);
            #endregion
            #region Casts (as_table)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_table", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_table(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_table_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_table_const(IntPtr @ref);
            #endregion
            #endregion
            #region Memory
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_new(IntPtr _0, IntPtr _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_type(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_data", CallingConvention = CallingConvention.Cdecl)]
            internal static unsafe extern sbyte* memory_data(IntPtr _0); // TODO: AUDIT

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_data_size", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong memory_data_size(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_size", CallingConvention = CallingConvention.Cdecl)]
            internal static extern uint memory_size(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_grow", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool memory_grow(IntPtr _0, uint delta);
            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memory_delete(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_copy(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool memory_same(IntPtr _0, IntPtr _1);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_get_host_info(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memory_set_host_info(IntPtr _0, IntPtr _1);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void memory_set_host_info_with_finalizer(IntPtr _0, IntPtr _1, IntPtr _2);
            #endregion
            #region Casts (memory_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_ref(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_ref_const(IntPtr _0);
            #endregion
            #region Casts (as_memory)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_memory", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_memory(IntPtr _0);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_memory_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_memory_const(IntPtr _0);
            #endregion
            #endregion
            #region Extern
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_kind", CallingConvention = CallingConvention.Cdecl)]
            internal static extern /*externkind_t*/byte extern_kind(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_type", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_type(IntPtr @extern);

            #region delete, copy, same
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_delete(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_copy(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool extern_same(IntPtr @extern, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_get_host_info(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_set_host_info(IntPtr @extern, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_set_host_info_with_finalizer(IntPtr @extern, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (extern_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_ref(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_ref_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_func", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_func(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_func_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_func_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_global", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_global(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_global_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_global_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_table", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_table(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_table_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_table_const(IntPtr @extern);

            [DllImport("wasmer", EntryPoint = "wasm_extern_as_memory", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_memory(IntPtr @extern);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_as_memory_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr extern_as_memory_const(IntPtr @extern);
            #endregion
            #region Casts (as_extern)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_extern(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_extern_const(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_extern(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_func_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr func_as_extern_const(IntPtr func);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_extern(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_global_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr global_as_extern_const(IntPtr global);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_extern(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_table_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr table_as_extern_const(IntPtr table);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_extern", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_extern(IntPtr memory);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_memory_as_extern_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr memory_as_extern_const(IntPtr memory);
            #endregion
            #region vec_{new,new_empty,new_uninitialized,copy,delete}
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new(ref vec_t/* extern_vec_t */ vec, ulong capacity, ref /* extern_t* */IntPtr array);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_empty", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new_empty(ref vec_t/* extern_vec_t */ vec);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_new_uninitialized", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_new_uninitialized(ref vec_t/* extern_vec_t */ vec, ulong capacity);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_copy(ref vec_t/* extern_vec_t */ vec, ref vec_t/* extern_vec_t */ vec2);

            [DllImport("wasmer", EntryPoint = "wasm_extern_vec_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void extern_vec_delete(ref vec_t/* extern_vec_t */ vec);
            #endregion
            #endregion
            #region Instance
            [DllImport("wasmer", EntryPoint = "wasm_instance_new", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_new(IntPtr store, IntPtr module, ref vec_t/* extern_vec_t */ imports, IntPtr _/* TODO: trap** */);

            [DllImport("wasmer", EntryPoint = "wasm_instance_exports", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_exports(IntPtr instance, ref vec_t/* extern_vec_t */ exports);

            #region delete, copy, same
            [DllImport("wasmer", EntryPoint = "wasm_instance_delete", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_delete(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_copy", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_copy(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_same", CallingConvention = CallingConvention.Cdecl)]
            [return: MarshalAs(UnmanagedType.I1)]
            internal static extern bool instance_same(IntPtr instance, IntPtr other);
            #endregion
            #region host info
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_get_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_get_host_info(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_set_host_info(IntPtr instance, IntPtr info);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_set_host_info_with_finalizer", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void instance_set_host_info_with_finalizer(IntPtr instance, IntPtr info, IntPtr finalizer);
            #endregion
            #region Casts (instance_as)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_as_ref", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_as_ref(IntPtr instance);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_instance_as_ref_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr instance_as_ref_const(IntPtr instance);
            #endregion
            #region Casts (as_instance)
            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_instance", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_instance(IntPtr @ref);

            [NotYetTested]
            [DllImport("wasmer", EntryPoint = "wasm_ref_as_instance_const", CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr ref_as_instance_const(IntPtr @ref);
            #endregion
            #endregion
            #region Val helpers
            #region Valtype helpers
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_i32() {
                return valtype_new((byte)valkind_t.I32);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_i64() {
                return valtype_new((byte)valkind_t.I64);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_f32() {
                return valtype_new((byte)valkind_t.F32);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_f64() {
                return valtype_new((byte)valkind_t.F64);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_anyref() {
                return valtype_new((byte)valkind_t.ANYREF);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr valtype_new_funcref() {
                return valtype_new((byte)valkind_t.FUNCREF);
            }
            #endregion
            #region Functype helpers
            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_0_0() {
                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new_empty(ref @params);

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new_empty(ref results);

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_1_0(
              ref /* own valtype_t* */IntPtr p) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new_empty(ref results);

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_2_0(
              ref /* own valtype_t* */IntPtr p1,
              ref /* own valtype_t* */IntPtr p2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new_empty(ref results);

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_3_0(
              ref /* own valtype_t* */IntPtr p1,
              ref /* own valtype_t* */IntPtr p2,
              ref /* own valtype_t* */IntPtr p3) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new_empty(ref results);

                return functype_new(ref @params, ref results);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_0_1(
              ref /* own valtype_t* */IntPtr r
            ) {
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new_empty(ref @params);

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_1_1(
              ref /* own valtype_t* */IntPtr p,
              ref /* own valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_2_1(
                ref /* own valtype_t* */IntPtr p1,
                ref /* own valtype_t* */IntPtr p2,
                ref /* own valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_3_1(
              ref /* own valtype_t* */IntPtr p1,
              ref /* own valtype_t* */IntPtr p2,
              ref /* own valtype_t* */IntPtr p3,
              ref /* own valtype_t* */IntPtr r) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[1] { r };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_0_2(
              ref /* own valtype_t* */IntPtr r1,
              ref /* own valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new_empty(ref @params);

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);

            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_1_2(
              ref /* own valtype_t* */IntPtr p,
              ref /* own valtype_t* */IntPtr r1,
              ref /* own valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[1] { p };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_2_2(
              ref /* own valtype_t* */IntPtr p1,
              ref /* own valtype_t* */IntPtr p2,
              ref /* own valtype_t* */IntPtr r1,
              ref /* own valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[2] { p1, p2 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }

            [NotYetTested]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static IntPtr/* functype_t* */ functype_new_3_2(
              ref /* own valtype_t* */IntPtr p1,
              ref /* own valtype_t* */IntPtr p2,
              ref /* own valtype_t* */IntPtr p3,
              ref /* own valtype_t* */IntPtr r1,
              ref /* own valtype_t* */IntPtr r2) {
                Span</* valtype_t* */IntPtr> ps = stackalloc IntPtr[3] { p1, p2, p3 };
                Span</* valtype_t* */IntPtr> rs = stackalloc IntPtr[2] { r1, r2 };

                vec_t/* valtype_vec_t*/ @params = default;
                valtype_vec_new(ref @params, (ulong)ps.Length, ref MemoryMarshal.GetReference(ps));

                vec_t/* valtype_vec_t*/ results = default;
                valtype_vec_new(ref results, (ulong)rs.Length, ref MemoryMarshal.GetReference(rs));

                return functype_new(ref @params, ref results);
            }
            #endregion
            #endregion
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class __wasmer {
            [DllImport("wasmer", EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_length();

            [DllImport("wasmer", EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            internal static extern ulong last_error_message(ref byte buffer, ulong length);

            [DllImport("wasmer", EntryPoint = "wat2wasm", CallingConvention = CallingConvention.Cdecl)]
            internal static extern void wat2wasm(ref vec_t/* byte_vec_t */ wat, ref vec_t/* byte_vec_t */ @out);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_canonicalize_nans", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_canonicalize_nans(IntPtr config, bool enable);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_push_middleware", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_push_middleware(IntPtr config, IntPtr middleware);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_compiler", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_set_compiler(IntPtr config, global::Wasm.WasmerCompilerT compiler);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_engine", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_set_engine(IntPtr config, global::Wasm.WasmerEngineT engine);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_features", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_set_features(IntPtr config, IntPtr features);

            //[NotYetTested]
            //[DllImport("wasmer", EntryPoint = "wasm_config_set_target", CallingConvention = CallingConvention.Cdecl)]
            //internal static extern void config_set_target(IntPtr config, IntPtr target);

        }

        [SuppressUnmanagedCodeSecurity]
        internal static class __wasi { }
    }
    namespace Bindings { }
}