using System;

namespace ImeSharp
{
    public static class ImeSharp
    {
        public static IIMEControl GetDefaultControl()
        {
            return IsWindows7OrBelow() ? (IIMEControl)new IMM32_IMEControl() : new TF_IMEControl();
        }

        public static IIMEControl Get_IMM32Control()
        {
            return new IMM32_IMEControl();
        }

        public static IIMEControl Get_TFControl()
        {
            return new TF_IMEControl();
        }

        /// <summary>
        /// return true if current OS version is Windows 7 or below.
        /// </summary>
        public static bool IsWindows7OrBelow()
        {
            if (Environment.OSVersion.Version.Major <= 5)
                return true;

            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1)
                return true;

            return false;
        }
    }
}