using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Wax.Paraffin;
using static Wax.Paraffin.__wasm;

namespace Wax {
    // Boxes, fix?
    public interface IWasmConfig : IDisposable {
        // USE EXTERNALLY AT YOUR OWN RISK
        unsafe ref IntPtr RawPointer { get; }
    }

    public interface IWasmObject : IDisposable { }

    public sealed class WasmExtern : IWasmObject {
        #region Memory management
        internal IWasmObject? _owner;

        private IntPtr _rawPointer;
        private int _disposedValue;

        private WasmExtern(IntPtr rawPointer) {
            _rawPointer = rawPointer;
        }

        private WasmExtern(WasmExtern original) {
            _rawPointer = wasm_extern_copy(original._rawPointer);
        }

        internal WasmExtern() { }

        internal static WasmExtern Wrap(IntPtr rawPointer) {
            return new WasmExtern(rawPointer);
        }

        public static WasmExtern New(WasmExtern original) {
            return new WasmExtern(original);
        }

        private void Dispose(bool disposing) {
            if (Interlocked.Exchange(ref _disposedValue, 1) == 0) {
                // IF the object is owned, skip deleting it - something else will handle it.
                if (_owner != null) {
                    _owner = null;
                    return;
                }
                wasm_extern_delete(_rawPointer);
            }
        }

        ~WasmExtern() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public sealed class WasmFunc : IWasmObject {
        #region Memory management
        internal IWasmObject? _owner;

        private IntPtr _rawPointer;
        private int _disposedValue;

        private WasmFunc(IntPtr rawPointer) {
            _rawPointer = rawPointer;
        }

        private WasmFunc(WasmFunc original) {
            _rawPointer = wasm_func_copy(original._rawPointer);
        }

        internal WasmFunc() { }

        internal static WasmFunc Wrap(IntPtr rawPointer) {
            return new WasmFunc(rawPointer);
        }

        public static WasmFunc New(WasmFunc original) {
            return new WasmFunc(original);
        }

        private void Dispose(bool disposing) {
            if (Interlocked.Exchange(ref _disposedValue, 1) == 0) {
                // IF the object is owned, skip deleting it - something else will handle it.
                if (_owner != null) {
                    _owner = null;
                    return;
                }
                wasm_func_delete(_rawPointer);
            }
        }

        ~WasmFunc() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmByteVec : IWasmObject {
        #region Memory management
        private wasm_byte_vec_t _vec;
        private int _disposedValue;

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

        private void Dispose(bool disposing) {
            if (Interlocked.Exchange(ref _disposedValue, 1) == 0) {
                Console.WriteLine("Disposing byte vec");
                wasm_byte_vec_delete(ref _vec);
            }
        }

        ~WasmByteVec() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmExternVec : IWasmObject {
        #region Memory management
        private wasm_extern_vec_t _vec;
        private int _disposedValue;

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

        internal WasmExternVec() { }

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

#pragma warning disable IDE0060 // Remove unused parameter
        private void Dispose(bool disposing) {
            if (Interlocked.Exchange(ref _disposedValue, 1) == 0) {
                Console.WriteLine("Disposing extern vec");
                wasm_extern_vec_delete(ref _vec);
            }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        ~WasmExternVec() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmEngine : IWasmObject {
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

#pragma warning disable IDE0060 // Remove unused parameter
        private void Dispose(bool disposing) {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                Console.WriteLine("Disposing engine");
                wasm_engine_delete(saved);
            }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        ~WasmEngine() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmStore : IWasmObject {
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

#pragma warning disable IDE0060 // Remove unused parameter
        private void Dispose(bool disposing) {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                Console.WriteLine("Disposing store");
                wasm_store_delete(saved);
            }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        ~WasmStore() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmModule : IWasmObject {
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

        private void Dispose(bool disposing) {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                Console.WriteLine("Disposing module");
                wasm_module_delete(saved);
            }
        }

        ~WasmModule() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class WasmInstance : IWasmObject {
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

#pragma warning disable IDE0060 // Remove unused parameter
        private void Dispose(bool disposing) {
            IntPtr saved = Interlocked.Exchange(ref _rawPointer, IntPtr.Zero);
            if (saved != IntPtr.Zero) {
                ptrToStruct.TryRemove(saved, out _);
                Console.WriteLine("Disposing instance");
                wasm_instance_delete(saved);
            }
        }
#pragma warning restore IDE0060 // Remove unused parameter

        ~WasmInstance() {
            Dispose(disposing: false);
        }

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        public WasmExternVec Exports() {
            var exports = new WasmExternVec();
            wasm_instance_exports(RawPointer, ref exports.RawVec);
            return exports;
        }
    }
}