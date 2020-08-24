using System;
using System.Runtime.InteropServices;

namespace ImeSharp
{
    public class TF_IMEControl : IIMEControl
    {
        #region Event

        public event GetCompExtEventHandler GetCompExtEvent;

        public event CandidateListEventHandler CandidateListEvent;

        public event CompositionEventHandler CompositionEvent;

        #endregion Event

        #region Private

        internal AppWrapper appWrapper;
        private CompositionHandler compHandler;
        private CandidateListWrapper candWrapper;

        #endregion Private

        #region IIMEControl

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            appWrapper = new AppWrapper();
            appWrapper.Initialize(handle, isUIElementOnly ? ActivateMode.UIELEMENTENABLEDONLY : ActivateMode.DEFAULT);
            //handle composition
            compHandler = appWrapper.GetCompHandler();
            compHandler.eventComposition += onCompostion;
            compHandler.eventGetCompExt += onGetCompExt;
            //handle candidatelist
            if (isUIElementOnly)
            {
                candWrapper = appWrapper.GetCandWapper();
                candWrapper.eventCandidateList += onCandidateList;
            }
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

        public void Dispose()
        {
            if (candWrapper != null)
                candWrapper.Dispose();
            appWrapper.Dispose();
        }

        #endregion IIMEControl

        #region EventRaiser

        public void onCandidateList(refCandidateList list)
        {
            CandidateList _list = new CandidateList();
            _list.CurSel = list.CurSel;
            _list.Count = list.Count;
            _list.CurPage = list.CurPage;
            _list.PageSize = list.PageSize;
            _list.Candidates = list.Candidates;
            CandidateListEvent?.Invoke(_list);
        }

        public void onCompostion(refCompositionEventArgs comp)
        {
            CompositionEventArgs _comp = new CompositionEventArgs();
            _comp.caretPos = comp.m_caretPos;
            _comp.state = (CompositionState)comp.m_state;
            _comp.strCommit = comp.m_strCommit;
            _comp.strComp = comp.m_strComp;
            CompositionEvent?.Invoke(_comp);
        }

        public void onGetCompExt(IntPtr rect)
        {
            RECT rect_ = (RECT)Marshal.PtrToStructure(rect, typeof(RECT));//Map from
            GetCompExtEvent?.Invoke(ref rect_);
            Marshal.StructureToPtr(rect_, rect, true);//Map to
        }

        #endregion EventRaiser
    }
}