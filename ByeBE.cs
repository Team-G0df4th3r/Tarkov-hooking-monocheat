using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MonoSecurity
{
    public unsafe static class ByeBE
    {
        public static Module mainModule;
        public static Assembly refAssembly;
        public static HashSet<IntPtr> asmList = new HashSet<IntPtr>();
        //static IntPtr asm = default;
        public const string DLLMONO = "mono-2.0-bdwgc";
        public static IntPtr Mono;


        public struct GSList
        {
            public void* data;
            public GSList* next;
            public GSList* prev;
        };

        // [DllImport(DLLMONO)]
        // public extern static IntPtr Mono_get_root_domain();

        // [DllImport(DLLMONO)]
        // public extern static IntPtr Mono_assembly_get_main();

        //[DllImport(DLLMONO)]
        //public extern static IntPtr Mono_assembly_loaded([MarshalAs(UnmanagedType.LPStr)] string aname);

        // [DllImport(DLLMONO)]
        //public extern static IntPtr mono_image_loaded([MarshalAs(UnmanagedType.LPStr)] string name);

        // [DllImport(DLLMONO)]
        //public extern static String Mono_assembly_get_name(IntPtr assembly);

        //[DllImport(DLLMONO)]
        //public extern static void mono_assembly_foreach();

        //[DllImport(DLLMONO)]
        //public extern static void mono_domain_free();

        //public static int* domain_assemblies;

        //static Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();


        public static void UnlinkAssemblies()
        {
            Process currentProcess = Process.GetCurrentProcess();

            //IntPtr _mono;

            //if (!ProcessUtils.GetMonoModule(currentProcess.Handle, out _mono))
            //  return;

            //Unlinker unlinker = new Unlinker(currentProcess.Handle, _mono);
            byte[] assembly = File.ReadAllBytes("C:\\service\\Schwerer_Gustav.dll");
            //unlinker.UnlinkAssembly(assembly);
        }

        public static unsafe void IntegCheck()
        {
            //Mono = GetModuleHandle(DLLMONO);
            // = GetProcAddress(Mono, "mono_image_loaded");
        }

        public static void Init()
        {
            UnlinkAssemblies();
        }

        /*
            IntPtr domain = mono_get_root_domain();
            IntPtr Ass = IntPtr.Zero;
            mono_assembly_foreach(AssemblyEnumerator, Ass);

            foreach(IntPtr intptr in asmList)
            {
                if (mono_assembly_get_name(intptr).Contains("Assembly"))
                {
                     asm = intptr;
                }
            }
            

            //asm = mono_image_loaded("Assembly-CSharp");

            asm = *(IntPtr*)((long)mono_image_loaded("Assembly-CSharp") + 1040);

            GSList* assembly = *(GSList**)(asm); //(domain + (int)asm)
            while (assembly != null)
            {
                GSList* nextAssembly = assembly->next;
                //assembly->prev = (GSList*)0x750;
                //assembly->next = (GSList*)0x750;
                assembly = nextAssembly;
            }
            *(IntPtr*)(asm) = (IntPtr)0x0;
            */
    }
}
