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

    public delegate void CompositionHandler(CompositionEventArgs comp);

    public delegate void GetCompExtHandler(ref RECT rect);

    public interface IIMEControl : IDisposable
    {
        event CandidateListHandler CandidateListEvent;

        event CompositionHandler CompositionEvent;

        event GetCompExtHandler GetCompExtEvent;

        void Initialize(IntPtr handle, bool isUIElementOnly = false);

        bool isIMEEnabled();

        void EnableIME();

        void DisableIME();
    }
}