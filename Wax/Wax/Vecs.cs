using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Paraffin;
using static Wax.Paraffin.__wasm;

namespace Wax {
    public interface IWasmVec<T> : IWasmObject where T : IWasmObject {
        public int Length { get; }
        public T this[int index] { get; }
    }

    public sealed class WasmByteVec : IWasmObject {
        #region Memory management
        internal IWasmObject _owner;
        internal wasm_byte_vec_t _vec;

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
        WasmByteVec(ReadOnlySpan<byte> buffer) { wasm_byte_vec_new(ref _vec, (ulong)buffer.Length, ref MemoryMarshal.GetReference(buffer)); }
        WasmByteVec(bool _empty) { wasm_byte_vec_new_empty(ref _vec); }
        WasmByteVec(int capacity) { wasm_byte_vec_new_uninitialized(ref _vec, (ulong)capacity); }
        WasmByteVec(WasmByteVec vec) { wasm_byte_vec_copy(ref RawVec, ref vec.RawVec); }

        public static WasmByteVec New(ReadOnlySpan<byte> buffer) => new WasmByteVec(buffer);
        public static WasmByteVec NewEmpty() => new WasmByteVec(_empty: true);
        public static WasmByteVec NewUninitialized(int capacity) => new WasmByteVec(capacity);
        public static WasmByteVec Copy(WasmByteVec vec) => new WasmByteVec(vec);
        #endregion
    }

    public sealed class WasmExternVec : IWasmObject, IWasmVec<WasmExtern> {
        #region Memory management
        internal IWasmObject _owner;
        internal wasm_extern_vec_t _vec;

        // USE EXTERNALLY AT YOUR OWN RISK
        public unsafe IWasmObject Owner => _owner;
        public unsafe ref wasm_extern_vec_t RawVec => ref _vec;

        ~WasmExternVec() {
            Console.WriteLine("deleting extern vec");
            wasm_extern_vec_delete(ref _vec);
        }

        internal WasmExternVec() { }
        WasmExternVec(ReadOnlySpan<IntPtr> list) { wasm_extern_vec_new(ref _vec, (ulong)list.Length, ref MemoryMarshal.GetReference(list)); }
        WasmExternVec(bool _empty) { wasm_extern_vec_new_empty(ref _vec); }
        WasmExternVec(int capacity) { wasm_extern_vec_new_uninitialized(ref _vec, (ulong)capacity); }
        WasmExternVec(WasmExternVec vec) { wasm_extern_vec_copy(ref RawVec, ref vec.RawVec); }

        public static WasmExternVec New(ReadOnlySpan<IntPtr> list) => new WasmExternVec(list);
        public static WasmExternVec NewEmpty() => new WasmExternVec(_empty: true);
        public static WasmExternVec NewUninitialized(int capacity) => new WasmExternVec(capacity);
        public static WasmExternVec Copy(WasmExternVec vec) => new WasmExternVec(vec);
        #endregion

        public int Length => (int)_vec.size;
        public WasmExtern this[int index] {
            get {
                if (index >= (int)_vec.size) { return null; }
                unsafe { return WasmExtern.Wrap(this, ((IntPtr*)_vec.data)[index]); }
            }
        }
    }

}
