using System;
using System.Runtime.InteropServices;
using Wax.Paraffin;
using static Wax.Paraffin.__wasm;

namespace Wax {
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
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmExtern() {
            if (_owner == null) {
                Console.WriteLine("deleting extern");
                wasm_extern_delete(_rawPointer);
            }
        }
        internal WasmExtern() { }
        WasmExtern(WasmExtern original) { _rawPointer = wasm_extern_copy(original._rawPointer); }

        internal static WasmExtern Wrap(IWasmObject owner, IntPtr rawPointer) {
            if (rawPointer == IntPtr.Zero) { return null; }
            return new WasmExtern() { _owner = owner, _rawPointer = rawPointer };
        }

        public static WasmExtern Copy(WasmExtern original) { return new WasmExtern(original); }
        #endregion
    }

    public sealed class WasmFunc : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmFunc() {
            if (_owner == null) {
                Console.WriteLine("deleting func");
                wasm_func_delete(_rawPointer);
            }
        }
        internal WasmFunc() { }
        WasmFunc(WasmFunc original) { _rawPointer = wasm_func_copy(original._rawPointer); }

        internal static WasmFunc Wrap(IWasmObject owner, IntPtr rawPointer) {
            if (rawPointer == IntPtr.Zero) { return null; }
            return new WasmFunc() { _owner = owner, _rawPointer = rawPointer };
        }

        public static explicit operator WasmFunc(WasmExtern @extern) =>
            Wrap(@extern, wasm_extern_as_func(@extern._rawPointer));
        public static WasmFunc Copy(WasmFunc original) { return new WasmFunc(original); }
        #endregion

        // TODO: val_vec interface cleanup
        public IntPtr/*trap*/ Call(ref wasm_val_vec_t args, ref wasm_val_vec_t results) {
            return wasm_func_call(_rawPointer, ref args, ref results);
        }
    }

    public sealed class WasmEngine : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmEngine() {
            Console.WriteLine("deleting engine");
            wasm_engine_delete(_rawPointer);
        }
        internal WasmEngine() { }
        WasmEngine(IntPtr rawPointer) { _rawPointer = rawPointer; }

        public static WasmEngine New() => new WasmEngine(wasm_engine_new());
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
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmStore() {
            Console.WriteLine("deleting store");
            wasm_store_delete(_rawPointer);
        }
        internal WasmStore() { }
        WasmStore(IntPtr rawPointer) { _rawPointer = rawPointer; }

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
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmModule() {
            Console.WriteLine("deleting module");
            wasm_module_delete(_rawPointer);
        }
        internal WasmModule() { }
        WasmModule(IntPtr rawPointer) { _rawPointer = rawPointer; }

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
        internal IntPtr _rawPointer;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref IntPtr RawPointer => ref _rawPointer;

        ~WasmInstance() {
            Console.WriteLine("deleting instance");
            wasm_instance_delete(_rawPointer);
        }
        internal WasmInstance() { }
        WasmInstance(IntPtr rawPointer) { _rawPointer = rawPointer; }

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