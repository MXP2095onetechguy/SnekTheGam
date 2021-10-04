using System;
using System.Runtime.InteropServices;


namespace AsmCs{
    class AssemblyCs{
        public bool IsUnicodeSupported(){
            return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Console.OutputEncoding.CodePage is 1200 or 65001;
        }
    }
}