using System;
using System.Runtime.InteropServices;

namespace ImeSharp.Native
{
    public class Ole
    {
        [DllImport("oleaut32.dll", PreserveSig = false)]
        public static extern void VariantClear(out object pObject);

        [DllImport("oleaut32.dll")]
        public static extern void VariantInit(out object pObject);
    }
}