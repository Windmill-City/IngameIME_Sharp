using System;

namespace ImeSharp
{
    internal class IMM32_IMEControl : IIMEControl
    {
        public event UpdateCompSelHandler CompSelEvent;

        public event UpdateCompStrHandler CompStrEvent;

        public event CommitHandler CommitEvent;

        public event GetCompExtHandler GetCompExtEvent;

        public event CandidateListHandler CandidateListEvent;

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
        }

        public bool isIMEEnabled()
        {
            return false;
        }

        public void DisableIME()
        {
        }

        public void EnableIME()
        {
        }

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

        public void onGetCompExt(ref TextStorLib.tagRECT rECT)
        {
            GetCompExtEvent(ref rECT);
        }

        #endregion EventRaiser
    }
}