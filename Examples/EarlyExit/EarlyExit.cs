using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;
using Wax.Imports.Data;

namespace Wax.Examples {
    public class EarlyExitExample {
        static void print_wasmer_error() {
            var error_len = __wasmer.last_error_length();
            Span<byte> error_message = stackalloc byte[(int)error_len];
            __wasmer.last_error_message(ref MemoryMarshal.GetReference(error_message), error_len);
            var error_str = Encoding.UTF8.GetString(error_message.ToArray());
            Console.Error.WriteLine(error_str.ToString());
        }

        static void print_frame(IntPtr frame) {
            var instance = __wasm.frame_instance(frame);
            var module_offset = __wasm.frame_module_offset(frame);
            var func_index = __wasm.frame_func_index(frame);
            var func_offset = __wasm.frame_func_offset(frame);
            Console.Error.WriteLine($"> {instance:X} @ 0x{module_offset:X} = {func_index}:{func_offset}");
        }

        static IntPtr store;

        static IntPtr early_exit(ref vec_t/* val_vec_t */args, ref vec_t/* val_vec_t */results) {
            var trap_string = "trapping from a host import\0";
            var trap_string_utf8 = Encoding.UTF8.GetBytes(trap_string);
            vec_t/* byte_vec_t */ trap_message = default;
            __wasm.byte_vec_new(ref trap_message, (ulong)trap_string_utf8.Length, ref MemoryMarshal.GetReference(trap_string_utf8.AsSpan()));
            var trap = __wasm.trap_new(store, ref trap_message);
            __wasm.byte_vec_delete(ref trap_message);
            return trap;
        }



        public static void Main(string[] args) {
            Console.WriteLine("Initializing...");
            var engine = __wasm.engine_new();
            store = __wasm.store_new(engine);

            Console.WriteLine("Loading binary...");
            byte[] file;
            try {
                file = File.ReadAllBytes("call_trap.wasm");
            } catch (Exception e) {
                Console.Error.WriteLine("> Error loading module!");
                Environment.Exit(1);
                return;
            }

            vec_t/* byte_vec_t */ binary = default;
            __wasm.byte_vec_new_uninitialized(ref binary, (ulong)file.Length);
            Marshal.Copy(file, 0, binary.data, file.Length);

            Console.WriteLine("Compiling module...");
            var module = __wasm.module_new(store, ref binary);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            __wasm.byte_vec_delete(ref binary);

            var host_func_type = __wasm.functype_new_0_0();
            var fp4del = Marshal.GetFunctionPointerForDelegate<__wasm.func_callback>(early_exit);
            var host_func = __wasm.func_new(store, host_func_type, fp4del);
            __wasm.functype_delete(host_func_type);

            //vec_t/* extern_vec_t */ imports = default;
            // NOTE: Modified from extern_vec_new_uninitialized in C source.
            // Fix after doing a pointer style pass on the code
            //__wasm.extern_vec_new_uninitialized(ref imports, 1);

            Span<IntPtr> imports_span = stackalloc IntPtr[1] { __wasm.func_as_extern(host_func) };
            vec_t/* extern_vec_t */ imports = default;
            imports.size = (ulong)imports_span.Length;
            unsafe {
                fixed (void* arg0 = &imports_span[0]) {
                    imports.data = (IntPtr)arg0;
                }
            }

            var instance = __wasm.instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Extracting export...");
            vec_t/* extern_vec_t */ exports = default;
            __wasm.instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }
            Console.WriteLine($"Found {exports.size} exports!");

            __wasm.module_delete(module);
            __wasm.instance_delete(instance);

            IntPtr run_func;
            unsafe {
                var realptr = (IntPtr*)exports.data;
                run_func = __wasm.extern_as_func(*realptr);
                if (run_func == IntPtr.Zero) {
                    Console.Error.WriteLine("> Error accessing export!");
                    print_wasmer_error();
                    Environment.Exit(1);
                }
            }

            Console.WriteLine("Calling export...");
            Span<val_t> args_val = stackalloc val_t[2] {
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = 1 } },
                new val_t { kind = (byte)valkind_t.I32, of = { i32 = 7 } },
            };
            Span<val_t> results_val = stackalloc val_t[1] {
                new val_t { kind = (byte)valkind_t.ANYREF, of = { @ref = IntPtr.Zero } },
            };

            vec_t/* val_vec_t */ args_vec = default;
            args_vec.size = (ulong)args_val.Length;
            unsafe {
                fixed (void* arg0 = &args_val[0]) {
                    args_vec.data = (IntPtr)arg0;
                }
            }

            vec_t/* val_vec_t */ results_vec = default;
            results_vec.size = (ulong)results_val.Length;
            unsafe {
                fixed (void* result0 = &results_val[0]) {
                    results_vec.data = (IntPtr)result0;
                }
            }

            var trap = __wasm.func_call(run_func, ref args_vec, ref results_vec);
            if (trap == IntPtr.Zero) {
                Console.Error.WriteLine("> Error calling function: expected trap!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Printing message...");
            vec_t/* byte_vec_t */ message = default;
            __wasm.trap_message(trap, ref message);
            string message_str;
            unsafe {
                message_str = Encoding.UTF8.GetString((byte*)message.data, (int)message.size);
            }
            Console.Error.WriteLine($"> {message_str}");

            Console.WriteLine("Printing origin...");
            var frame = __wasm.trap_origin(trap);
            if (frame != IntPtr.Zero) {
                print_frame(frame);
                __wasm.frame_delete(frame);
            } else {
                Console.Error.WriteLine("> Empty origin.");
            }

            Console.WriteLine("Printing trace...");
            vec_t/* frame_vec_t */ trace = default;
            __wasm.trap_trace(trap, ref trace);
            if (trace.size > 0) {
                Span<IntPtr> trace_span;
                unsafe {
                    trace_span = new Span<IntPtr>((IntPtr*)trace.data, (int)trace.size);
                }
                for (ulong i = 0; i < trace.size; i++) {
                    print_frame(trace_span[(int)i]);
                }
            } else {
                Console.Error.WriteLine("> Empty trace.");
            }

            __wasm.frame_vec_delete(ref trace);
            __wasm.trap_delete(trap);
            __wasm.byte_vec_delete(ref message);

            __wasm.extern_vec_delete(ref exports);
            __wasm.extern_vec_delete(ref imports);


            Console.WriteLine("Shutting down...");
            __wasm.store_delete(store);
            __wasm.engine_delete(engine);

            Console.WriteLine("Done.");
        }
    }
}
