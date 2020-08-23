using System;

namespace ImeSharp
{
    public delegate void UpdateCompStrHandler(string comp);

    public delegate void UpdateCompSelHandler(int acpStart, int acpEnd);

    public delegate void CommitHandler(string commit);

    public delegate void GetCompExtHandler(ref TextStorLib.tagRECT rECT);

    public interface IIMEControl
    {
        event UpdateCompSelHandler CompSelEvent;

        event UpdateCompStrHandler CompStrEvent;

        event CommitHandler CommitEvent;

        event GetCompExtHandler GetCompExtEvent;

        void Initialize(IntPtr handle, bool isUIElementOnly = false);

        void EnableIME();

        void DisableIME();
    }
}