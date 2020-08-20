using System;

namespace ImeSharp
{
    public class TF_IMEControl : IIMEControl
    {
        #region Event

        public event UpdateCompSelHandler CompSelEvent;

        public event UpdateCompStrHandler CompStrEvent;

        public event CommitHandler CommitEvent;

        public event GetCompExtHandler GetCompExtEvent;

        public event CandidateListHandler CandidateListEvent;

        #endregion Event

        #region Private

        private AppWrapper appWrapper;

        #endregion Private

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            appWrapper = new AppWrapper();
            appWrapper.Initialize(handle, isUIElementOnly ? ActivateMode.UIELEMENTENABLEDONLY : ActivateMode.DEFAULT);

            appWrapper.eventCommit += onCommit;
            appWrapper.eventCompSel += onCompSel;
            appWrapper.eventCompStr += onCompStr;
            appWrapper.eventGetCompExt += onGetCompExt;
        }

        public bool isIMEEnabled()
        {
            return appWrapper.m_Initilized ? appWrapper.m_IsIMEEnabled : false;
        }

        public void DisableIME()
        {
            if (appWrapper.m_Initilized)
                appWrapper.DisableIME();
        }

        public void EnableIME()
        {
            if (appWrapper.m_Initilized)
                appWrapper.EnableIME();
        }

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

        public void onGetCompExt(IntPtr rect)
        {
            GetCompExtEvent?.Invoke(rect);
        }

        #endregion EventRaiser
    }
}