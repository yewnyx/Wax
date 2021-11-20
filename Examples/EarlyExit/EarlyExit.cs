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

        public static void Main(string[] argv) {
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
            // Fix after doing a pointer type pass on the code (i.e. SafeHandleZeroOrMinusOneIsInvalid)
            //extern_vec_new_uninitialized(ref imports, 1);
            wasm_extern_vec_t imports = WASM_ARRAY_EXTERN_VEC(stackalloc IntPtr[1] { wasm_func_as_extern(host_func) });

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
            Span<wasm_val_t> values = stackalloc wasm_val_t[2] {
                WASM_I32_VAL(1),
                WASM_I32_VAL(7),
            };
            wasm_val_vec_t args = WASM_ARRAY_VEC(values);
            Span<wasm_val_t> result = stackalloc wasm_val_t[1] { WASM_INIT_VAL() };
            wasm_val_vec_t rets = WASM_ARRAY_VEC(result);

            var trap = wasm_func_call(run_func, ref args, ref rets);
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
