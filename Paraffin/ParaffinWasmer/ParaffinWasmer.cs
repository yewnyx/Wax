using System.Runtime.InteropServices;
using System.Security;

// TODO: Audit vec_new(..., ref /* ... */IntPtr array) function signatures for correctness

namespace Wax {
    namespace Paraffin {
        [SuppressUnmanagedCodeSecurity]
        public static unsafe class __wasmer {
            const string WASM_LIB = "wasmer";

            [DllImport(WASM_LIB, EntryPoint = "wasmer_last_error_length", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasmer_last_error_length();

            [DllImport(WASM_LIB, EntryPoint = "wasmer_last_error_message", CallingConvention = CallingConvention.Cdecl)]
            public static extern ulong wasmer_last_error_message(ref byte buffer, ulong length);

            [DllImport(WASM_LIB, EntryPoint = "wat2wasm", CallingConvention = CallingConvention.Cdecl)]
            public static extern void wat2wasm(ref wasm_byte_vec_t wat, ref wasm_byte_vec_t binary);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_canonicalize_nans", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_canonicalize_nans(IntPtr config, bool enable);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_push_middleware", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_push_middleware(IntPtr config, IntPtr middleware);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_set_compiler", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_compiler(IntPtr config, global::Wasm.WasmerCompilerT compiler);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_set_engine", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_engine(IntPtr config, global::Wasm.WasmerEngineT engine);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_set_features", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_features(IntPtr config, IntPtr features);

            //[NotYetTested]
            //[DllImport(WASM_LIB, EntryPoint = "wasm_config_set_target", CallingConvention = CallingConvention.Cdecl)]
            //public static extern void config_set_target(IntPtr config, IntPtr target);
        }
    }
}