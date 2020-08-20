using System;
using System.Runtime.InteropServices;

namespace ImeSharp
{
    public class IMM32_IMEControl : IIMEControl
    {
        #region Events

        public event UpdateCompSelHandler CompSelEvent;

        public event UpdateCompStrHandler CompStrEvent;

        public event CommitHandler CommitEvent;

        public event GetCompExtHandler GetCompExtEvent;

        public event CandidateListHandler CandidateListEvent;

        #endregion Events

        #region WM Msg

        public const int WM_IME_SETCONTEXT = 0x0281;
        public const int WM_IME_NOTIFY = 0x0282;
        public const int WM_IME_CONTROL = 0x0283;
        public const int WM_IME_STARTCOMPOSITION = 0x010D;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_INPUTLANGCHANGE = 0x0051;

        #endregion WM Msg

        #region IMM32

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CANDIDATELIST
        {
            public uint dwSize;
            public uint dwStyle;
            public uint dwCount;
            public uint dwSelection;
            public uint dwPageStart;
            public uint dwPageSize;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.U4)]
            public uint[] dwOffset;
        }

        // CANDIDATEFORM structures
        [StructLayout(LayoutKind.Sequential)]
        public struct CANDIDATEFORM
        {
            public int dwIndex;
            public int dwStyle;
            public POINT ptCurrentPos;
            public RECT rcArea;
        }

        // COMPOSITIONFORM structures
        [StructLayout(LayoutKind.Sequential)]
        public struct COMPOSITIONFORM
        {
            public int dwStyle;
            public POINT ptCurrentPos;
            public RECT rcArea;
        }

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern IntPtr ImmCreateContext();

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern bool ImmDestroyContext(IntPtr hIMC);

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern IntPtr ImmReleaseContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("imm32.dll", CharSet = CharSet.Unicode)]
        public static extern uint ImmGetCandidateList(IntPtr hIMC, uint deIndex, IntPtr candidateList, uint dwBufLen);

        [DllImport("imm32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int ImmGetCompositionString(IntPtr hIMC, int CompositionStringFlag, char[] buf, int bufferLength);

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll", SetLastError = true)]
        public static extern bool ImmGetOpenStatus(IntPtr hIMC);

        [DllImport("Imm32.dll", SetLastError = true)]
        public static extern bool ImmSetOpenStatus(IntPtr hIMC, bool open);

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern bool ImmSetCandidateWindow(IntPtr hIMC, ref CANDIDATEFORM candidateForm);

        [DllImport("imm32.dll", SetLastError = true)]
        public static extern int ImmSetCompositionWindow(IntPtr hIMC, ref COMPOSITIONFORM compForm);

        #endregion IMM32

        #region User32

        public const int GWL_WNDPROC = -4;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        #endregion User32

        #region Private

        private IntPtr _immContext;
        private IntPtr _hWnd;
        private IntPtr prevWndProc;

        private bool _isUIElementOnly;
        private bool _isIMEEnabled;

        #endregion Private

        #region IIMEControl

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            _hWnd = handle;
            _isUIElementOnly = isUIElementOnly;
            _immContext = ImmGetContext(handle);

            prevWndProc = SetWindowLong(handle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(new WndProc(Handle_IMM32_MSG)));
        }

        public bool isIMEEnabled()
        {
            return _isIMEEnabled;
        }

        public void DisableIME()
        {
            _isIMEEnabled = false;
            ImmSetOpenStatus(_immContext, false);
        }

        public void EnableIME()
        {
            _isIMEEnabled = true;
            ImmSetOpenStatus(_immContext, true);
        }

        #endregion IIMEControl

        #region Process WM Msg

        private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private IntPtr Handle_IMM32_MSG(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                //When init window, we will recive this Msg
                //If we want IME input, we need to AccrociateContext
                case WM_IME_INPUTLANGCHANGE:
                    if (_isIMEEnabled)
                        ImmAssociateContext(hWnd, _immContext);
                    goto Handled;

                //SETCONTEXT should be handled by IME, we can control IMEs UI(Candidate Window, etc) by changing the lParam
                //See:https://docs.microsoft.com/en-us/windows/win32/intl/wm-ime-setcontext
                //we need to associatecontext, for activate IME
                case WM_IME_SETCONTEXT:
                    if (_isIMEEnabled && (int)wParam == 1)//wParam == 1 means window active otherwise not
                    {
                        ImmAssociateContext(hWnd, _immContext);
                        ImmSetOpenStatus(_immContext, true);
                    }
                    break;

                case WM_IME_COMPOSITION:
                    Handle_COMPOSITION_Msg((int)wParam, (int)lParam);
                    goto Handled;

                case WM_IME_STARTCOMPOSITION:
                case WM_IME_ENDCOMPOSITION:
                    //we handle and draw the comp text
                    //so we dont pass this msg to the next

                    //Clear CompStr
                    onCompStr("");
                    goto Handled;

                default:
                    break;
            }
            return CallWindowProc(prevWndProc, hWnd, msg, wParam, lParam);
        Handled:
            return IntPtr.Zero;
        }

        #region Handle WM_IME_COMPOSITION

        #region Change Flag

        // parameter of ImmGetCompositionString

        public const int GCS_COMPREADSTR = 0x0001;//0
        public const int GCS_COMPREADATTR = 0x0002;
        public const int GCS_COMPREADCLAUSE = 0x0004;//2

        public const int GCS_COMPSTR = 0x0008;//3//Comp Text/Selection change
        public const int GCS_COMPATTR = 0x0010;
        public const int GCS_COMPCLAUSE = 0x0020;//5

        public const int GCS_CURSORPOS = 0x0080;//6

        public const int GCS_DELTASTART = 0x0100;//7

        public const int GCS_RESULTREADSTR = 0x0200;//8
        public const int GCS_RESULTREADCLAUSE = 0x0400;
        public const int GCS_RESULTSTR = 0x0800;//10//Commit Result
        public const int GCS_RESULTCLAUSE = 0x1000;

        // style bit flags for WM_IME_COMPOSITION

        public const int CS_INSERTCHAR = 0x2000;//12
        public const int CS_NOMOVECARET = 0x4000;

        #endregion Change Flag

        /// <summary>
        /// Handle the changement
        /// </summary>
        /// <param name="compFlag">Modify Flag for Composition Text</param>
        /// <param name="genrealFlag">Modify Flag for Composition Text or Result Text</param>
        private void Handle_COMPOSITION_Msg(int compFlag, int genrealFlag)
        {
            if ((GCS_COMPSTR & compFlag) != 0)//Comp String/Sel Changed
            {
                int size = ImmGetCompositionString(_immContext, GCS_COMPSTR, null, 0);
                char[] buf = new char[size / Marshal.SizeOf(typeof(short))];
                ImmGetCompositionString(_immContext, GCS_COMPSTR, buf, size);
                onCompStr(new string(buf));
            }
            if ((GCS_RESULTSTR & genrealFlag) != 0)//Has commited
            {
                int size = ImmGetCompositionString(_immContext, GCS_RESULTSTR, null, 0);
                char[] buf = new char[size / Marshal.SizeOf(typeof(short))];
                ImmGetCompositionString(_immContext, GCS_RESULTSTR, buf, size);
                onCommit(new string(buf));
            }
        }

        #endregion Handle WM_IME_COMPOSITION

        #endregion Process WM Msg

        #region EventRaiser

        public void onCandidateList(CandidateList list)
        {
            CandidateListEvent.Invoke(list);
        }

        public void onCompSel(int acpStart, int acpEnd)
        {
            CompSelEvent(acpStart, acpEnd);
        }

        public void onCompStr(string comp)
        {
            CompStrEvent(comp);
        }

        public void onCommit(string commit)
        {
            CommitEvent(commit);
        }

        public void onGetCompExt(ref refRECT rECT)
        {
            GetCompExtEvent(ref rECT);
        }

        #endregion EventRaiser
    }
}