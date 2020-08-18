using System;

namespace ImeSharp
{
    public delegate void CandidateListHandler(CandidateList list);

    public delegate void UpdateCompStrHandler(string comp);

    public delegate void UpdateCompSelHandler(int acpStart, int acpEnd);

    public delegate void CommitHandler(string commit);

    public delegate void GetCompExtHandler(ref TextStorLib.tagRECT rECT);

    public interface IIMEControl
    {
        event CandidateListHandler CandidateListEvent;

        event UpdateCompSelHandler CompSelEvent;

        event UpdateCompStrHandler CompStrEvent;

        event CommitHandler CommitEvent;

        event GetCompExtHandler GetCompExtEvent;

        #region Event Raiser

        void onCandidateList(CandidateList list);

        void onCompSel(int acpStart, int acpEnd);

        void onCompStr(string comp);

        void onCommit(string commit);

        void onGetCompExt(ref TextStorLib.tagRECT rECT);

        #endregion Event Raiser

        void Initialize(IntPtr handle, bool isUIElementOnly = false);

        bool isIMEEnabled();

        void EnableIME();

        void DisableIME();
    }
}