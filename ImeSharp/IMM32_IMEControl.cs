using System;

namespace ImeSharp
{
    internal class IMM32_IMEControl : IIMEControl
    {
        public event UpdateCompSelHandler CompSelEvent;

        public event UpdateCompStrHandler CompStrEvent;

        public event CommitHandler CommitEvent;

        public event GetCompExtHandler GetCompExtEvent;

        public void DisableIME()
        {
            throw new NotImplementedException();
        }

        public void EnableIME()
        {
            throw new NotImplementedException();
        }

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            throw new NotImplementedException();
        }
    }
}