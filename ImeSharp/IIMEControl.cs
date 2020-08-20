using System;
using System.Runtime.InteropServices;

namespace ImeSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public delegate void CandidateListHandler(CandidateList list);

    public delegate void UpdateCompStrHandler(string comp);

    public delegate void UpdateCompSelHandler(int acpStart, int acpEnd);

    public delegate void CommitHandler(string commit);

    public delegate void GetCompExtHandler(RECT rect);

    public interface IIMEControl
    {
        event CandidateListHandler CandidateListEvent;

        event UpdateCompSelHandler CompSelEvent;

        event UpdateCompStrHandler CompStrEvent;

        event CommitHandler CommitEvent;

        event GetCompExtHandler GetCompExtEvent;

        void Initialize(IntPtr handle, bool isUIElementOnly = false);

        bool isIMEEnabled();

        void EnableIME();

        void DisableIME();
    }
}