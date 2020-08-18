using ImeSharp.Native;
using msctfLib;
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

        private ITfThreadMgr _tfThreadMgr;
        private ITfDocumentMgr _tfDocumentMgr;
        private ITfContext _tfContext;
        private uint _tfClientId;
        private TF_TextStore _tfTextStore;
        private TF_TextEditSink _tfTextEditSink;
        private bool _isIMEEnabled;

        #endregion Private

        public uint _ecReadOnly = msctf.TF_INVALID_COOKIE;

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            msctf.TF_CreateThreadMgr(out _tfThreadMgr);
            (_tfThreadMgr as ITfThreadMgrEx).ActivateEx(out _tfClientId, isUIElementOnly ?
                msctf.TF_TMF_UIELEMENTENABLEDONLY : 0u);

            //Document
            _tfThreadMgr.CreateDocumentMgr(out _tfDocumentMgr);
            _tfThreadMgr.AssociateFocus(ref handle, _tfDocumentMgr, out _);

            //Context
            _tfTextStore = new TF_TextStore(this);
            _tfDocumentMgr.CreateContext(_tfClientId, 0, (IntPtr)0, out _tfContext, out _ecReadOnly);

            _tfTextEditSink = new TF_TextEditSink(_tfContext, _tfTextStore);
        }

        public bool isIMEEnabled()
        {
            return _isIMEEnabled;
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