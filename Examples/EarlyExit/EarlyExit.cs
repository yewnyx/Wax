using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Wax.Imports;

using static Wax.Imports.__wasm;

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
            var instance = wasm_frame_instance(frame);
            var module_offset = wasm_frame_module_offset(frame);
            var func_index = wasm_frame_func_index(frame);
            var func_offset = wasm_frame_func_offset(frame);
            Console.Error.WriteLine($"> {instance:X} @ 0x{module_offset:X} = {func_index}:{func_offset}");
        }

        static IntPtr store;

        static IntPtr early_exit(ref wasm_val_vec_t args, ref wasm_val_vec_t results) {
            var trap_string = "trapping from a host import\0";
            var trap_string_utf8 = Encoding.UTF8.GetBytes(trap_string);
            wasm_byte_vec_t trap_message = default;
            wasm_byte_vec_new(ref trap_message, (ulong)trap_string_utf8.Length, ref MemoryMarshal.GetReference(trap_string_utf8.AsSpan()));
            var trap = wasm_trap_new(store, ref trap_message);
            wasm_byte_vec_delete(ref trap_message);
            return trap;
        }

        public static void Main(string[] args) {
            Console.WriteLine("Initializing...");
            var engine = wasm_engine_new();
            store = wasm_store_new(engine);

            Console.WriteLine("Loading binary...");
            byte[] file;
            try {
                file = File.ReadAllBytes("call_trap.wasm");
            } catch (Exception) {
                Console.Error.WriteLine("> Error loading module!");
                Environment.Exit(1);
                return;
            }

            wasm_byte_vec_t binary = default;
            wasm_byte_vec_new_uninitialized(ref binary, (ulong)file.Length);
            Marshal.Copy(file, 0, binary.data, file.Length);

            Console.WriteLine("Compiling module...");
            var module = wasm_module_new(store, ref binary);
            if (module == null) {
                Console.Error.WriteLine("> Error compiling module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            wasm_byte_vec_delete(ref binary);

            var host_func_type = wasm_functype_new_0_0();
            var fp4del = Marshal.GetFunctionPointerForDelegate<func_callback>(early_exit);
            var host_func = wasm_func_new(store, host_func_type, fp4del);
            wasm_functype_delete(host_func_type);

            //wasm_extern_vec_t imports = default;
            // NOTE: Modified from extern_vec_new_uninitialized in C source.
            // Fix after doing a pointer style pass on the code
            //extern_vec_new_uninitialized(ref imports, 1);

            Span<IntPtr> imports_span = stackalloc IntPtr[1] { wasm_func_as_extern(host_func) };
            wasm_extern_vec_t imports = default;
            imports.size = (ulong)imports_span.Length;
            unsafe {
                fixed (void* arg0 = &imports_span[0]) {
                    imports.data = (IntPtr)arg0;
                }
            }

            var instance = wasm_instance_new(store, module, ref imports, IntPtr.Zero);
            if (instance == null) {
                Console.Error.WriteLine("> Error instantiating module!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Extracting export...");
            wasm_extern_vec_t exports = default;
            wasm_instance_exports(instance, ref exports);
            if (exports.size == 0) {
                Console.Error.WriteLine("> Error accessing exports!");
                print_wasmer_error();
                Environment.Exit(1);
            }
            Console.WriteLine($"Found {exports.size} exports!");

            wasm_module_delete(module);
            wasm_instance_delete(instance);

            IntPtr run_func;
            unsafe {
                var realptr = (IntPtr*)exports.data;
                run_func = wasm_extern_as_func(*realptr);
                if (run_func == IntPtr.Zero) {
                    Console.Error.WriteLine("> Error accessing export!");
                    print_wasmer_error();
                    Environment.Exit(1);
                }
            }

            Console.WriteLine("Calling export...");
            Span<wasm_val_t> args_val = stackalloc wasm_val_t[2] {
                new wasm_val_t { kind = (byte)wasm_valkind_enum.I32, of = { i32 = 1 } },
                new wasm_val_t { kind = (byte)wasm_valkind_enum.I32, of = { i32 = 7 } },
            };
            Span<wasm_val_t> results_val = stackalloc wasm_val_t[1] {
                new wasm_val_t { kind = (byte)wasm_valkind_enum.ANYREF, of = { @ref = IntPtr.Zero } },
            };

            wasm_val_vec_t args_vec = default;
            args_vec.size = (ulong)args_val.Length;
            unsafe {
                fixed (void* arg0 = &args_val[0]) {
                    args_vec.data = (IntPtr)arg0;
                }
            }

            wasm_val_vec_t results_vec = default;
            results_vec.size = (ulong)results_val.Length;
            unsafe {
                fixed (void* result0 = &results_val[0]) {
                    results_vec.data = (IntPtr)result0;
                }
            }

            var trap = wasm_func_call(run_func, ref args_vec, ref results_vec);
            if (trap == IntPtr.Zero) {
                Console.Error.WriteLine("> Error calling function: expected trap!");
                print_wasmer_error();
                Environment.Exit(1);
            }

            Console.WriteLine("Printing message...");
            wasm_byte_vec_t message = default;
            wasm_trap_message(trap, ref message);
            string message_str;
            unsafe {
                message_str = Encoding.UTF8.GetString((byte*)message.data, (int)message.size);
            }
            Console.Error.WriteLine($"> {message_str}");

            Console.WriteLine("Printing origin...");
            var frame = wasm_trap_origin(trap);
            if (frame != IntPtr.Zero) {
                print_frame(frame);
                wasm_frame_delete(frame);
            } else {
                Console.Error.WriteLine("> Empty origin.");
            }

            Console.WriteLine("Printing trace...");
            wasm_frame_vec_t trace = default;
            wasm_trap_trace(trap, ref trace);
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

            wasm_frame_vec_delete(ref trace);
            wasm_trap_delete(trap);
            wasm_byte_vec_delete(ref message);

            wasm_extern_vec_delete(ref exports);
            wasm_extern_vec_delete(ref imports);


            Console.WriteLine("Shutting down...");
            wasm_store_delete(store);
            wasm_engine_delete(engine);

            Console.WriteLine("Done.");
        }
    }
}
