using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Wax.Paraffin;
using static Wax.Paraffin.__wasm;

namespace Wax {
    // Boxes, fix?
    public interface IWasmConfig {
        // USE EXTERNALLY AT YOUR OWN RISK
        unsafe ref IntPtr RawPointer { get; }
    }

    public interface IWasmObject {
        IWasmObject Owner { get; }
    }

    public sealed class WasmExtern : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        public unsafe IWasmObject Owner => _owner;

        ~WasmExtern() {
            if (_owner == null) {
                Console.WriteLine("deleting extern");
                wasm_extern_delete(_rawPointer);
            }
        }
        internal WasmExtern() { }
        internal WasmExtern(IntPtr rawPointer) { _rawPointer = rawPointer; }
        private WasmExtern(WasmExtern original) { _rawPointer = wasm_extern_copy(original._rawPointer); }

        public static WasmExtern New(WasmExtern original) { return new WasmExtern(original); }
        #endregion
    }
    public sealed class WasmFunc : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        public unsafe IWasmObject Owner => _owner;


        ~WasmFunc() {
            if (_owner == null) {
                Console.WriteLine("deleting func");
                wasm_func_delete(_rawPointer);
            }
        }
        internal WasmFunc() { }
        internal WasmFunc(IntPtr rawPointer) { _rawPointer = rawPointer; }
        private WasmFunc(WasmFunc original) { _rawPointer = wasm_func_copy(original._rawPointer); }

        public static WasmFunc New(WasmFunc original) { return new WasmFunc(original); }
        #endregion
    }

    public sealed class WasmByteVec : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        wasm_byte_vec_t _vec;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref wasm_byte_vec_t RawVec => ref _vec;

        ~WasmByteVec() {
            if (_owner == null) {
                Console.WriteLine("deleting byte vec");
                wasm_byte_vec_delete(ref _vec);
            }
        }
        internal WasmByteVec() { }
        private WasmByteVec(ReadOnlySpan<byte> buffer) { wasm_byte_vec_new(ref _vec, (ulong)buffer.Length, ref MemoryMarshal.GetReference(buffer)); }
        private WasmByteVec(bool _empty) { wasm_byte_vec_new_empty(ref _vec); }
        private WasmByteVec(int capacity) { wasm_byte_vec_new_uninitialized(ref _vec, (ulong)capacity); }
        private WasmByteVec(WasmByteVec vec) { wasm_byte_vec_copy(ref RawVec, ref vec.RawVec); }

        public static WasmByteVec New(ReadOnlySpan<byte> buffer) { return new WasmByteVec(buffer); }
        public static WasmByteVec New() { return new WasmByteVec(true); }
        public static WasmByteVec NewUninitialized(int capacity) { return new WasmByteVec(capacity); }
        public static WasmByteVec New(WasmByteVec vec) { return new WasmByteVec(vec); }
        #endregion
    }

    public sealed class WasmExternVec : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        wasm_extern_vec_t _vec;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref wasm_extern_vec_t RawVec => ref _vec;

        ~WasmExternVec() {
            Console.WriteLine("deleting extern vec");
            wasm_extern_vec_delete(ref _vec);
        }
        internal WasmExternVec() { }
        private WasmExternVec(ReadOnlySpan<IntPtr> list) { wasm_extern_vec_new(ref _vec, (ulong)list.Length, ref MemoryMarshal.GetReference(list)); }
        private WasmExternVec(bool _empty) { wasm_extern_vec_new_empty(ref _vec); }
        private WasmExternVec(int capacity) { wasm_extern_vec_new_uninitialized(ref _vec, (ulong)capacity); }
        private WasmExternVec(WasmExternVec vec) { wasm_extern_vec_copy(ref RawVec, ref vec.RawVec); }

        public static WasmExternVec New() { return new WasmExternVec(true); }
        public static WasmExternVec New(ReadOnlySpan<IntPtr> list) { return new WasmExternVec(list); }
        public static WasmExternVec NewUninitialized(int capacity) { return new WasmExternVec(capacity); }
        public static WasmExternVec New(WasmExternVec vec) { return new WasmExternVec(vec); }
        #endregion
    }

    public sealed class WasmEngine : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmEngine() {
            Console.WriteLine("deleting engine");
            wasm_engine_delete(_rawPointer);
        }
        internal WasmEngine() { }
        private WasmEngine(IntPtr rawPointer) { _rawPointer = rawPointer; }

        public static WasmEngine New() { return new WasmEngine(wasm_engine_new()); }
        public static WasmEngine NewWithConfig(IWasmConfig config) {
            var configPtr = config.RawPointer;
            if (configPtr == null) { configPtr = IntPtr.Zero; }
            return new WasmEngine(wasm_engine_new_with_config(configPtr));
        }
        #endregion
    }

    public sealed class WasmStore : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmStore() {
            Console.WriteLine("deleting store");
            wasm_store_delete(_rawPointer);
        }
        internal WasmStore() { }
        private WasmStore(IntPtr rawPointer) { _rawPointer = rawPointer; }

        public static WasmStore New(WasmEngine engine) {
            var enginePtr = engine.RawPointer;
            if (enginePtr == null) { enginePtr = IntPtr.Zero; }
            return new WasmStore(wasm_store_new(enginePtr));
        }
        #endregion
    }

    public sealed class WasmModule : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmModule() {
            Console.WriteLine("deleting module");
            wasm_module_delete(_rawPointer);
        }
        internal WasmModule() { }
        private WasmModule(IntPtr rawPointer) { _rawPointer = rawPointer; }

        public static WasmModule New(WasmStore store, WasmByteVec binary) {
            var storePtr = store.RawPointer;
            if (storePtr == null) { storePtr = IntPtr.Zero; }
            return new WasmModule(wasm_module_new(storePtr, ref binary.RawVec));
        }
        #endregion
    }

    public sealed class WasmInstance : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmInstance() {
            Console.WriteLine("deleting instance");
            wasm_instance_delete(_rawPointer);
        }
        internal WasmInstance() { }
        private WasmInstance(IntPtr rawPointer) { _rawPointer = rawPointer; }

        public static WasmInstance New(WasmStore store, WasmModule module, WasmExternVec imports, out IntPtr trap) {
            var storePtr = store.RawPointer;
            var modulePtr = module.RawPointer;
            Span<IntPtr> trapSpan = stackalloc IntPtr[1] { IntPtr.Zero };
            var instancePtr = wasm_instance_new(storePtr, modulePtr, ref imports.RawVec, ref MemoryMarshal.GetReference(trapSpan));
            trap = trapSpan[0];
            return new WasmInstance(instancePtr);
        }
        #endregion

        public WasmExternVec Exports() {
            var exports = new WasmExternVec();
            wasm_instance_exports(RawPointer, ref exports.RawVec);
            return exports;
        }
    }
}