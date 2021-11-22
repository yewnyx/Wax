using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Wax.Paraffin;
using static Wax.Paraffin.__wasm;

namespace Wax {
    public sealed class WasmByteVec {
        private wasm_byte_vec_t _vec;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref wasm_byte_vec_t RawVec => ref _vec;

        static readonly ConcurrentDictionary<IntPtr, WasmByteVec> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmByteVec>();

        private WasmByteVec(ReadOnlySpan<byte> buffer) {
            wasm_byte_vec_new(ref _vec, (ulong)buffer.Length, ref MemoryMarshal.GetReference(buffer));
        }

        private WasmByteVec(bool _empty) {
            wasm_byte_vec_new_empty(ref _vec);
        }

        private WasmByteVec(int capacity) {
            wasm_byte_vec_new_uninitialized(ref _vec, (ulong)capacity);
        }

        private WasmByteVec(WasmByteVec vec) {
            wasm_byte_vec_copy(ref RawVec, ref vec.RawVec);
        }

        internal WasmByteVec() { }

        public static WasmByteVec New(ReadOnlySpan<byte> buffer) {
            return new WasmByteVec(buffer);
        }

        public static WasmByteVec New() {
            return new WasmByteVec(true);
        }

        public static WasmByteVec NewUninitialized(int capacity) {
            return new WasmByteVec(capacity);
        }

        public static WasmByteVec New(WasmByteVec vec) {
            return new WasmByteVec(vec);
        }

        public void Dispose() {
            wasm_byte_vec_delete(ref _vec);
        }
    }

    public sealed class WasmExternVec {
        private wasm_extern_vec_t _vec;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref wasm_extern_vec_t RawVec => ref _vec;

        static readonly ConcurrentDictionary<IntPtr, WasmByteVec> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmByteVec>();

        private WasmExternVec(ReadOnlySpan<IntPtr> list) {
            wasm_extern_vec_new(ref _vec, (ulong)list.Length, ref MemoryMarshal.GetReference(list));
        }

        private WasmExternVec(bool _empty) {
            wasm_extern_vec_new_empty(ref _vec);
        }

        private WasmExternVec(int capacity) {
            wasm_extern_vec_new_uninitialized(ref _vec, (ulong)capacity);
        }

        private WasmExternVec(WasmExternVec vec) {
            wasm_extern_vec_copy(ref RawVec, ref vec.RawVec);
        }

        internal WasmExternVec() {}

        public static WasmExternVec New(ReadOnlySpan<IntPtr> list) {
            return new WasmExternVec(list);
        }

        public static WasmExternVec New() {
            return new WasmExternVec(true);
        }

        public static WasmExternVec NewUninitialized(int capacity) {
            return new WasmExternVec(capacity);
        }

        public static WasmExternVec New(WasmExternVec vec) {
            return new WasmExternVec(vec);
        }

        public void Dispose() {
            wasm_extern_vec_delete(ref _vec);
        }
    }

    public interface IWasmConfig : IDisposable {
        // USE EXTERNALLY AT YOUR OWN RISK
        unsafe ref IntPtr RawPointer { get; }
    }

    public sealed class WasmEngine : IDisposable {
        #region Memory management
        private IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        static readonly ConcurrentDictionary<IntPtr, WasmEngine> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmEngine>();

        private WasmEngine() { }

        public static WasmEngine? New() {
            var instancePtr = wasm_engine_new();
            return _getOrCreateInstance(instancePtr);
        }

        public static WasmEngine? NewWithConfig(IWasmConfig config) {
            var configPtr = config.RawPointer;
            if (configPtr == null) { configPtr = IntPtr.Zero; }
            var instancePtr = wasm_engine_new_with_config(configPtr);
            return _getOrCreateInstance(instancePtr);
        }

        private static WasmEngine? _getOrCreateInstance(IntPtr ptr) {
            if (ptr == null) {
                return null;
            } else if (ptrToStruct.TryGetValue(ptr, out var instance)) {
                return instance;
            } else {
                instance = new WasmEngine { _rawPointer = ptr };
                ptrToStruct[ptr] = instance;
                return instance;
            }
        }

        public void Dispose() {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                wasm_engine_delete(saved);
            }
        }
        #endregion
    }

    public sealed class WasmStore : IDisposable {
        #region Memory management
        private IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        static readonly ConcurrentDictionary<IntPtr, WasmStore> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmStore>();

        private WasmStore() { }

        public static WasmStore? New(WasmEngine engine) {
            var enginePtr = engine.RawPointer;
            var instancePtr = wasm_store_new(enginePtr);
            return _getOrCreateInstance(instancePtr);
        }

        private static WasmStore? _getOrCreateInstance(IntPtr ptr) {
            if (ptr == null) {
                return null;
            } else if (ptrToStruct.TryGetValue(ptr, out var instance)) {
                return instance;
            } else {
                instance = new WasmStore { _rawPointer = ptr };
                ptrToStruct[ptr] = instance;
                return instance;
            }
        }

        public void Dispose() {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                wasm_store_delete(saved);
            }
        }
        #endregion
    }

    public sealed class WasmModule : IDisposable {
        #region Memory management
        private IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        static readonly ConcurrentDictionary<IntPtr, WasmModule> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmModule>();

        private WasmModule() { }

        public static WasmModule? New(WasmStore store, WasmByteVec binary) {
            var storePtr = store.RawPointer;
            var instancePtr = wasm_module_new(storePtr, ref binary.RawVec);
            return _getOrCreateInstance(instancePtr);
        }

        private static WasmModule? _getOrCreateInstance(IntPtr ptr) {
            if (ptr == null) {
                return null;
            } else if (ptrToStruct.TryGetValue(ptr, out var instance)) {
                return instance;
            } else {
                instance = new WasmModule { _rawPointer = ptr };
                ptrToStruct[ptr] = instance;
                return instance;
            }
        }

        public void Dispose() {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                wasm_module_delete(saved);
            }
        }
        #endregion
    }

    public sealed class WasmInstance : IDisposable {
        #region Memory management
        private IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        static readonly ConcurrentDictionary<IntPtr, WasmInstance> ptrToStruct = new ConcurrentDictionary<IntPtr, WasmInstance>();

        private WasmInstance() { }

        public static WasmInstance? New(WasmStore store, WasmModule module, WasmExternVec imports, out IntPtr trap) {
            var storePtr = store.RawPointer;
            var modulePtr = module.RawPointer;
            Span<IntPtr> trapSpan = stackalloc IntPtr[1] { IntPtr.Zero };
            var instancePtr = wasm_instance_new(storePtr, modulePtr, ref imports.RawVec, ref MemoryMarshal.GetReference(trapSpan));
            trap = trapSpan[0];
            return _getOrCreateInstance(instancePtr);
        }

        private static WasmInstance? _getOrCreateInstance(IntPtr ptr) {
            if (ptr == null) {
                return null;
            } else if (ptrToStruct.TryGetValue(ptr, out var instance)) {
                return instance;
            } else {
                instance = new WasmInstance { _rawPointer = ptr };
                ptrToStruct[ptr] = instance;
                return instance;
            }
        }

        public void Dispose() {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                wasm_instance_delete(saved);
            }
        }
        #endregion

        public WasmExternVec Exports() {
            var exports = new WasmExternVec();
            wasm_instance_exports(RawPointer, ref exports.RawVec);
            return exports;
        }
    }
}