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

    public delegate void CandidateListEventHandler(CandidateList list);

    public delegate void CompositionEventHandler(CompositionEventArgs comp);

    public delegate void GetCompExtEventHandler(ref RECT rect);

    public interface IIMEControl : IDisposable
    {
        event CandidateListEventHandler CandidateListEvent;

        event CompositionEventHandler CompositionEvent;

        event GetCompExtEventHandler GetCompExtEvent;

        void Initialize(IntPtr handle, bool isUIElementOnly = false);

        bool isIMEEnabled();

        void EnableIME();

        void DisableIME();
    }
}