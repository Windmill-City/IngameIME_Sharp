using ImeSharp.Native;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TextStorLib;

namespace ImeSharp
{
    internal struct ADVISE_SINK
    {
        public IUnknown punk;
        public ITextStoreACPSink pTextStoreACPSink;
        public uint dwMask;
    }

    public class TF_TextStore : ITextStoreACP2
    {
        #region Private

        private TF_IMEControl tF_IMEControl;

        private IntPtr m_hWnd;

        //TextStore
        private int m_acpStart;

        private int m_acpEnd;
        private uint m_cchOldLength;
        private bool m_fInterimChar;
        private TsActiveSelEnd m_ActiveSelEnd;
        private string m_StoredStr;

        //TextStoreSink
        private ADVISE_SINK m_AdviseSink;

        private msctfLib.ITextStoreACPServices m_pServices;
        private bool m_fNotify;

        //DocLock
        private bool m_fLocked;

        private uint m_dwLockType;
        private Queue<uint> m_queuedLockReq;

        //TextBox
        private TS_STATUS m_status;

        private bool m_fLayoutChanged;

        //Composition
        private string m_CompStr;

        #endregion Private

        #region Public

        public bool m_fhasEdited;
        public bool m_Commit;
        public bool m_Composing;
        public int m_CommitStart;
        public int m_CommitEnd;
        public int m_CompStart;
        public int m_CompEnd;

        #endregion Public

        public TF_TextStore(TF_IMEControl iMEControl)
        {
            tF_IMEControl = iMEControl;
        }

        public void AdviseSink(ref GUID riid, object punk, uint dwMask)
        {
            punk = punk as IUnknown;
            //see if this advise sink already exists
            if (punk == m_AdviseSink.punk)
            {
                //this is the same advise sink, so just update the advise mask
                m_AdviseSink.dwMask = dwMask;
            }
            else if (null != m_AdviseSink.punk)
            {
                //only one advise sink is allowed at a time
                throw new COMException("TextStore_CONNECT_E_CANNOTCONNECT");
            }
            else if (punk as ITextStoreACPSink != null)
            {
                //set the advise mask
                m_AdviseSink.dwMask = dwMask;

                //get the ITextStoreACPSink interface
                m_AdviseSink.pTextStoreACPSink = punk as ITextStoreACPSink;

                //get the ITextStoreACPServices interface
                m_pServices = punk as msctfLib.ITextStoreACPServices;
            }
        }

        public void UnadviseSink(object punk)
        {
            punk = punk as IUnknown;
            if (punk == m_AdviseSink.punk)
            {
                //remove the advise sink from the list
                _ClearAdviseSink(m_AdviseSink);

                m_pServices = null;
            }
            else
            {
                throw new COMException("TextStore_CONNECT_E_NOCONNECTION");
            }
        }

        public void RequestLock(uint dwLockFlags, ref int phrSession)
        {
            if (null == m_AdviseSink.pTextStoreACPSink)
            {
                throw new COMException("TextStore_NoSink", HResult.E_UNEXPECTED);
            }

            phrSession = HResult.E_FAIL;
            if (m_fLocked)
            {
                //the document is locked

                if ((dwLockFlags & TextStor.TS_LF_SYNC) > 0)
                {
                    /*
                    The caller wants an immediate lock, but this cannot be granted because
                    the document is already locked.
                    */
                    phrSession = TextStor.TS_E_SYNCHRONOUS;
                }
                else
                {
                    //the request is asynchronous

                    //Queue the lock request
                    m_queuedLockReq.Enqueue(dwLockFlags);
                    phrSession = TextStor.TS_S_ASYNC;
                }
            }

            //lock the document
            _LockDocument(dwLockFlags);

            //call OnLockGranted
            try
            {
                m_AdviseSink.pTextStoreACPSink.OnLockGranted(dwLockFlags);
            }
            catch (COMException e)
            {
                phrSession = e.HResult;
                _UnlockDocument();
                return;
            }

            phrSession = HResult.S_OK;

            //unlock the document
            _UnlockDocument();
        }

        public void GetStatus(ref TS_STATUS pdcs)
        {
            /*
	        Can be zero or:
	        TS_SD_READONLY  // if set, document is read only; writes will fail
	        TS_SD_LOADING   // if set, document is loading, expect additional inserts
	        */
            pdcs.dwDynamicFlags = m_status.dwDynamicFlags;

            /*
            Can be zero or:
            TS_SS_DISJOINTSEL   // if set, the document supports multiple selections
            TS_SS_REGIONS       // if clear, the document will never contain multiple regions
            TS_SS_TRANSITORY    // if set, the document is expected to have a short lifespan
            TS_SS_NOHIDDENTEXT  // if set, the document will never contain hidden text (for perf)
            */
            pdcs.dwStaticFlags = m_status.dwStaticFlags;
        }

        public void QueryInsert(int acpTestStart, int acpTestEnd, uint cch, ref int pacpResultStart, ref int pacpResultEnd)
        {
            if (acpTestStart > m_StoredStr.Length || acpTestEnd > m_StoredStr.Length)
                throw new COMException("QueryInsert request a invaild pos", HResult.E_INVALIDARG);
            pacpResultStart = acpTestStart;
            pacpResultEnd = acpTestEnd;
        }

        public void GetSelection(uint ulIndex, uint ulCount, ref TS_SELECTION_ACP[] pSelection, ref uint pcFetched)
        {
            pcFetched = 0;

            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READ))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            //check the requested index
            if (TextStor.TS_DEFAULT_SELECTION == ulIndex)
            {
                ulIndex = 0;
            }
            else if (ulIndex > 1)
            {
                /*
                The index is too high. This app only supports one selection.
                */
                throw new COMException("Invaild Selection Count", HResult.E_INVALIDARG);
            }

            pSelection[0].acpStart = m_acpStart;
            pSelection[0].acpEnd = m_acpEnd;
            pSelection[0].style.fInterimChar = m_fInterimChar;
            if (m_fInterimChar)
            {
                /*
                fInterimChar will be set when an intermediate character has been
                set. One example of when this will happen is when an IME is being
                used to enter characters and a character has been set, but the IME
                is still active.
                */
                pSelection[0].style.ase = TsActiveSelEnd.TS_AE_NONE;
            }
            else
            {
                pSelection[0].style.ase = m_ActiveSelEnd;
            }

            pcFetched = 1;
        }

        public void SetSelection(uint ulCount, ref TS_SELECTION_ACP[] pSelection)
        {
            if (ulCount > 1)
            {
                //this implementaiton only supports a single selection
                throw new COMException("Invaild Selection Count", HResult.E_INVALIDARG);
            }

            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READWRITE))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            m_acpStart = pSelection[0].acpStart;
            m_acpEnd = pSelection[0].acpEnd;
            m_fInterimChar = pSelection[0].style.fInterimChar;
            if (m_fInterimChar)
            {
                /*
                fInterimChar will be set when an intermediate character has been
                set. One example of when this will happen is when an IME is being
                used to enter characters and a character has been set, but the IME
                is still active.
                */
                m_ActiveSelEnd = TsActiveSelEnd.TS_AE_NONE;
            }
            else
            {
                m_ActiveSelEnd = pSelection[0].style.ase;
            }
        }

        public void GetText(int acpStart, int acpEnd, ref char[] pchPlain, uint cchPlainReq, ref uint pcchPlainRet, ref TS_RUNINFO[] prgRunInfo, uint cRunInfoReq, ref uint pcRunInfoRet, ref int pacpNext)
        {
            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READ))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            bool fDoText = cchPlainReq > 0;
            bool fDoRunInfo = cRunInfoReq > 0;
            int cchTotal;

            pcchPlainRet = 0;
            pacpNext = acpStart;

            //get all of the text
            cchTotal = m_StoredStr.Length;

            //validate the start pos
            if ((acpStart < 0) || (acpStart > cchTotal))
            {
                throw new COMException("Invaild Pos", TextStor.TS_E_INVALIDPOS);
            }
            else
            {
                //are we at the end of the document
                if (acpStart == cchTotal)
                {
                    return;
                }
                else
                {
                    uint cchReq;

                    /*
                    acpEnd will be -1 if all of the text up to the end is being requested.
                    */

                    if (acpEnd >= acpStart)
                    {
                        cchReq = (uint)(acpEnd - acpStart);
                    }
                    else
                    {
                        cchReq = (uint)(cchTotal - acpStart);
                    }

                    if (fDoText)
                    {
                        if (cchReq > cchPlainReq)
                        {
                            cchReq = cchPlainReq;
                        }

                        if (cchPlainReq > 0)
                        {
                            //the text output is not NULL terminated
                            pchPlain = m_StoredStr.Substring(acpStart, (int)cchReq).ToCharArray();
                        }
                    }

                    //it is possible that only the length of the text is being requested
                    pcchPlainRet = cchReq;

                    if (fDoRunInfo)
                    {
                        /*
                        Runs are used to separate text characters from formatting characters.

                        In this example, sequences inside and including the <> are treated as
                        control sequences and are not displayed.

                        Plain text = "Text formatting."
                        Actual text = "Text <B><I>formatting</I></B>."

                        If all of this text were requested, the run sequence would look like this:

                        prgRunInfo[0].type = TS_RT_PLAIN;   //"Text "
                        prgRunInfo[0].uCount = 5;

                        prgRunInfo[1].type = TS_RT_HIDDEN;  //<B><I>
                        prgRunInfo[1].uCount = 6;

                        prgRunInfo[2].type = TS_RT_PLAIN;   //"formatting"
                        prgRunInfo[2].uCount = 10;

                        prgRunInfo[3].type = TS_RT_HIDDEN;  //</B></I>
                        prgRunInfo[3].uCount = 8;

                        prgRunInfo[4].type = TS_RT_PLAIN;   //"."
                        prgRunInfo[4].uCount = 1;

                        TS_RT_OPAQUE is used to indicate characters or character sequences
                        that are in the document, but are used privately by the application
                        and do not map to text.  Runs of text tagged with TS_RT_OPAQUE should
                        NOT be included in the pchPlain or cchPlainOut [ref] parameters.
                        */

                        /*
                        This implementation is plain text, so the text only consists of one run.
                        If there were multiple runs, it would be an error to have consecuative runs
                        of the same type.
                        */
                        prgRunInfo[0].type = TsRunType.TS_RT_PLAIN;
                        prgRunInfo[0].uCount = cchReq;
                    }

                    pacpNext = (int)(acpStart + cchReq);
                }
            }
        }

        public void SetText(uint dwFlags, int acpStart, int acpEnd, ref char[] pchText, uint cch, ref TS_TEXTCHANGE pChange)
        {
            /*
	        dwFlags can be:
	        TS_ST_CORRECTION
	        */

            //set the selection to the specified range
            TS_SELECTION_ACP[] tsa = new TS_SELECTION_ACP[1];
            tsa[0].acpStart = acpStart;
            tsa[0].acpEnd = acpEnd;
            tsa[0].style.ase = TsActiveSelEnd.TS_AE_START;
            tsa[0].style.fInterimChar = false;

            SetSelection(1, ref tsa);

            //call InsertTextAtSelection
            InsertTextAtSelection(TextStor.TS_IAS_NOQUERY, ref pchText, cch, out _, out _, ref pChange);
        }

        public void GetFormattedText(int acpStart, int acpEnd, out IDataObject ppDataObject)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void GetEmbedded(int acpPos, ref GUID rguidService, ref GUID riid, out object ppunk)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void QueryInsertEmbedded(ref GUID pguidService, ref tagFORMATETC pformatetc, out bool pfInsertable)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void InsertEmbedded(uint dwFlags, int acpStart, int acpEnd, IDataObject pDataObject, out TS_TEXTCHANGE pChange)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void InsertTextAtSelection(uint dwFlags, ref char[] pchText, uint cch, out int pacpStart, out int pacpEnd, ref TS_TEXTCHANGE pChange)
        {
            int lTemp = 0;

            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READWRITE))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            pacpStart = lTemp;
            pacpEnd = lTemp;

            int acpStart;
            int acpOldEnd;
            int acpNewEnd;

            acpOldEnd = m_acpEnd;

            //set the start point after the insertion
            acpStart = m_acpStart;

            //set the end point after the insertion
            acpNewEnd = (int)(m_acpStart + cch);

            if ((dwFlags & TextStor.TS_IAS_QUERYONLY) == 1)
            {
                pacpStart = acpStart;
                pacpEnd = acpOldEnd;
                return;
            }

            //insert the text
            m_StoredStr.Remove(acpStart, acpOldEnd - acpStart);
            m_StoredStr.Insert(acpStart, new string(pchText).Substring(0, acpNewEnd - acpStart));

            //set the selection
            m_acpStart = acpStart;
            m_acpEnd = acpNewEnd;

            if ((dwFlags & TextStor.TS_IAS_NOQUERY) == 0)
            {
                pacpStart = acpStart;
                pacpEnd = acpNewEnd;
            }

            //set the TS_TEXTCHANGE members
            pChange.acpStart = acpStart;
            pChange.acpOldEnd = acpOldEnd;
            pChange.acpNewEnd = acpNewEnd;

            //defer the layout change notification until the document is unlocked
            m_fLayoutChanged = true;
        }

        public void InsertEmbeddedAtSelection(uint dwFlags, IDataObject pDataObject, out int pacpStart, out int pacpEnd, out TS_TEXTCHANGE pChange)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void RequestSupportedAttrs(uint dwFlags, uint cFilterAttrs, ref TS_ATTRID paFilterAttrs)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void RequestAttrsAtPosition(int acpPos, uint cFilterAttrs, ref TS_ATTRID paFilterAttrs, uint dwFlags)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void RequestAttrsTransitioningAtPosition(int acpPos, uint cFilterAttrs, ref TS_ATTRID paFilterAttrs, uint dwFlags)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void FindNextAttrTransition(int acpStart, int acpHalt, uint cFilterAttrs, ref TS_ATTRID paFilterAttrs, uint dwFlags, out int pacpNext, out bool pfFound, out int plFoundOffset)
        {
            ///Note  If an application does not implement ITextStoreACP::FindNextAttrTransition, ITfReadOnlyProperty::EnumRanges fails with E_FAIL.
            // We don't support any attributes.
            // So we always return "not found".
            pacpNext = 0;
            pfFound = false;
            plFoundOffset = 0;
        }

        public void RetrieveRequestedAttrs(uint ulCount, out TS_ATTRVAL paAttrVals, out uint pcFetched)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void GetEndACP(out int pacp)
        {
            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READ))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            pacp = m_StoredStr.Length;
        }

        public void GetActiveView(out uint pvcView)
        {
            pvcView = 0;//only one view
        }

        public void GetACPFromPoint(uint vcView, ref tagPOINT ptScreen, uint dwFlags, out int pacp)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        public void GetTextExt(uint vcView, int acpStart, int acpEnd, ref tagRECT prc, ref bool pfClipped)
        {
            pfClipped = false;

            //does the caller have a lock
            if (!_IsLocked(TextStor.TS_LF_READ))
            {
                //the caller doesn't have a lock
                throw new COMException("NO LOCK", TextStor.TS_E_NOLOCK);
            }

            //According to Microsoft's doc, an ime should not make empty request,
            //but some ime draw comp text themseleves, when empty req will be make
            //Check empty request
            //if (acpStart == acpEnd) {
            //	return E_INVALIDARG;
            //}

            //Post Event
            tF_IMEControl.onGetCompExt(ref prc);
            Win32.MapWindowPoints(m_hWnd, (IntPtr)0, ref prc, 2);
        }

        public void GetScreenExt(uint vcView, ref tagRECT prc)
        {
            throw new COMException("NOT IMPL", HResult.E_NOTIMPL);
        }

        private void _ClearAdviseSink(ADVISE_SINK adviseSink)
        {
            adviseSink.punk = null;
            adviseSink.pTextStoreACPSink = null;

            adviseSink.dwMask = 0;
        }

        private bool _LockDocument(uint dwLockFlags)
        {
            if (m_fLocked)
            {
                return false;
            }

            m_fLocked = true;
            m_dwLockType = dwLockFlags;

            return true;
        }

        private void _UnlockDocument()
        {
            m_fLocked = false;
            m_dwLockType = 0;
            if (m_fhasEdited)
            {
                m_fhasEdited = false;
                if (m_Commit)
                {
                    m_Commit = false;
                    int commitLen = m_CommitEnd - m_CommitStart;
                    //Post event
                    tF_IMEControl.onCommit(m_StoredStr.Substring(m_CommitStart, commitLen));

                    m_StoredStr.Remove(m_CommitStart, commitLen);
                    TS_TEXTCHANGE textChange;
                    textChange.acpStart = m_CommitStart;
                    textChange.acpOldEnd = m_CommitEnd;
                    textChange.acpNewEnd = m_CommitStart;
                    m_AdviseSink.pTextStoreACPSink.OnTextChange(0, textChange);
                    m_acpStart = m_acpEnd = m_StoredStr.Length;
                    m_AdviseSink.pTextStoreACPSink.OnSelectionChange();
                    m_CommitStart = m_CommitEnd = 0;
                }

                if (m_Composing)
                {
                    tF_IMEControl.onCompStr(m_StoredStr.Substring(m_CompStart, m_CompEnd - m_CompStart));
                    tF_IMEControl.onCompSel(m_acpStart, m_acpEnd);
                }
                else
                {
                    tF_IMEControl.onCompStr("");
                    tF_IMEControl.onCompSel(0, 0);
                }
            }

            //if there is a queued lock, grant it
            if (m_queuedLockReq.Count > 0)
            {
                int phrSession = 0;
                RequestLock(m_queuedLockReq.Dequeue(), ref phrSession);
            }

            //if any layout changes occurred during the lock, notify the manager
            if (m_fLayoutChanged)
            {
                m_fLayoutChanged = false;
                m_AdviseSink.pTextStoreACPSink.OnLayoutChange(TsLayoutCode.TS_LC_CHANGE, 0);
            }
        }

        private bool _IsLocked(uint dwLockType)
        {
            return m_fLocked && (m_dwLockType & dwLockType) > 0;
        }
    }
}