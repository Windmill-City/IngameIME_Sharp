﻿using System;
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

        #region IMM32

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct CANDIDATELIST
        {
            public uint dwSize;
            public uint dwStyle;
            public uint dwCount;
            public uint dwSelection;
            public uint dwPageStart;
            public uint dwPageSize;

            public fixed uint dwOffset[1];
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
        public static extern uint ImmGetCandidateList(IntPtr hIMC, uint deIndex, byte[] buf, uint dwBufLen);

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
        private WndProc WndProc_;

        private bool _isUIElementOnly;
        private bool _isIMEEnabled;

        private CandidateList candidate = new CandidateList();

        #endregion Private

        #region IIMEControl

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            _hWnd = handle;
            _isUIElementOnly = isUIElementOnly;
            _immContext = ImmGetContext(handle);

            WndProc_ = new WndProc(Handle_IMM32_MSG);//in case gc
            prevWndProc = SetWindowLong(handle, GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(WndProc_));
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

        public void Dispose()
        {
            ImmAssociateContext(_hWnd, IntPtr.Zero);
            ImmReleaseContext(_hWnd, _immContext);

            SetWindowLong(_hWnd, GWL_WNDPROC, prevWndProc);
        }

        #endregion IIMEControl

        #region Process WM Msg

        private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        #region WM Msg

        public const int WM_IME_SETCONTEXT = 0x0281;
        public const int WM_IME_NOTIFY = 0x0282;
        public const int WM_IME_CONTROL = 0x0283;
        public const int WM_IME_STARTCOMPOSITION = 0x010D;
        public const int WM_IME_ENDCOMPOSITION = 0x010E;
        public const int WM_IME_COMPOSITION = 0x010F;
        public const int WM_IME_INPUTLANGCHANGE = 0x0051;

        public const int WM_GETDLGCODE = 0x0087;

        public const int DLGC_WANTALLKEYS = 0x0004;

        #endregion WM Msg

        #region SetContext Flags

        // lParam for WM_IME_SETCONTEXT

        public const uint ISC_SHOWUICANDIDATEWINDOW = 0x00000001;
        public const uint ISC_SHOWUICOMPOSITIONWINDOW = 0x80000000;
        public const uint ISC_SHOWUIGUIDELINE = 0x40000000;
        public const uint ISC_SHOWUIALLCANDIDATEWINDOW = 0x0000000F;
        public const uint ISC_SHOWUIALL = 0xC000000F;

        #endregion SetContext Flags

        private IntPtr Handle_IMM32_MSG(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WM_GETDLGCODE:
                    //Return this to make IME get the keys
                    return (IntPtr)DLGC_WANTALLKEYS;
                //When init window, we will recive this Msg
                //If we want IME input, we need to AccrociateContext
                case WM_IME_INPUTLANGCHANGE:
                    ImmAssociateContext(hWnd, _immContext);
                    goto Handled;

                //SETCONTEXT should be handled by IME, we can control IMEs UI(Candidate Window, etc) by changing the lParam
                //See:https://docs.microsoft.com/en-us/windows/win32/intl/wm-ime-setcontext
                //we need to associatecontext, for activate IME
                case WM_IME_SETCONTEXT:
                    if ((long)wParam == 1)//wParam == 1 means window active otherwise not
                        ImmAssociateContext(hWnd, _immContext);
                    if (_isIMEEnabled)
                        ImmSetOpenStatus(_immContext, true);
                    if (_isUIElementOnly)
                        lParam = (IntPtr)(((long)lParam) & ~ISC_SHOWUICANDIDATEWINDOW);
                    lParam = (IntPtr)(((long)lParam) & ~ISC_SHOWUICOMPOSITIONWINDOW);
                    break;

                case WM_IME_COMPOSITION:
                    Handle_COMPOSITION_Msg((long)wParam, (long)lParam);
                    goto Handled;

                case WM_IME_STARTCOMPOSITION:
                case WM_IME_ENDCOMPOSITION:
                    //we handle and draw the comp text
                    //so we dont pass this msg to the next

                    //Clear CompStr
                    onCompStr("");
                    //Clear candidates
                    candidate.Reset();
                    onCandidateList(candidate);
                    goto Handled;

                case WM_IME_NOTIFY:
                    Handle_NOTIFY_Msg((long)wParam, (long)lParam);
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
        private void Handle_COMPOSITION_Msg(long compFlag, long genrealFlag)
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

        #region Handle WM_IME_NOTIFY

        #region NOTIFY Flag

        // wParam of report message WM_IME_NOTIFY

        public const int IMN_CLOSESTATUSWINDOW = 0x0001;
        public const int IMN_OPENSTATUSWINDOW = 0x0002;
        public const int IMN_CHANGECANDIDATE = 0x0003;
        public const int IMN_CLOSECANDIDATE = 0x0004;
        public const int IMN_OPENCANDIDATE = 0x0005;
        public const int IMN_SETCONVERSIONMODE = 0x0006;
        public const int IMN_SETSENTENCEMODE = 0x0007;
        public const int IMN_SETOPENSTATUS = 0x0008;
        public const int IMN_SETCANDIDATEPOS = 0x0009;
        public const int IMN_SETCOMPOSITIONFONT = 0x000A;
        public const int IMN_SETCOMPOSITIONWINDOW = 0x000B;
        public const int IMN_SETSTATUSWINDOWPOS = 0x000C;
        public const int IMN_GUIDELINE = 0x000D;
        public const int IMN_PRIVATE = 0x000E;

        #endregion NOTIFY Flag

        private void Handle_NOTIFY_Msg(long cmd, long data)
        {
            switch (cmd)
            {
                case IMN_CHANGECANDIDATE:
                    uint size = ImmGetCandidateList(_immContext, 0, null, 0);

                    var buf = new byte[size];
                    size = ImmGetCandidateList(_immContext, 0, buf, size);
                    if (size > 0)//Fetch failed
                        candidate.Apply(buf);
                    else
                        candidate.Reset();
                    onCandidateList(candidate);
                    break;

                case IMN_OPENCANDIDATE:
                case IMN_CLOSECANDIDATE:
                    candidate.Reset();
                    onCandidateList(candidate);
                    break;

                default:
                    break;
            }
        }

        #endregion Handle WM_IME_NOTIFY

        #endregion Process WM Msg

        #region EventRaiser

        public void onCandidateList(CandidateList list)
        {
            CandidateListEvent?.Invoke(list);
        }

        public void onCompSel(int acpStart, int acpEnd)
        {
            CompSelEvent?.Invoke(acpStart, acpEnd);
        }

        public void onCompStr(string comp)
        {
            CompStrEvent?.Invoke(comp);
        }

        public void onCommit(string commit)
        {
            CommitEvent?.Invoke(commit);
        }

        public void onGetCompExt(ref RECT rect)
        {
            GetCompExtEvent?.Invoke(ref rect);
        }

        #endregion EventRaiser
    }
}