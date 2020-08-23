using System;
using System.Runtime.InteropServices;

namespace ImeSharp
{
    public class TF_IMEControl : IIMEControl
    {
        #region Event

        public event GetCompExtHandler GetCompExtEvent;

        public event CandidateListHandler CandidateListEvent;

        public event CompositionHandler CompositionEvent;

        #endregion Event

        #region Private

        private AppWrapper appWrapper;

        #endregion Private

        #region IIMEControl

        public void Initialize(IntPtr handle, bool isUIElementOnly = false)
        {
            appWrapper = new AppWrapper();
            appWrapper.Initialize(handle, isUIElementOnly ? ActivateMode.UIELEMENTENABLEDONLY : ActivateMode.DEFAULT);

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

        public void Dispose()
        {
            appWrapper.Dispose();
        }

        #endregion IIMEControl

        #region EventRaiser

        public void onCandidateList(CandidateList list)
        {
            CandidateListEvent?.Invoke(list);
        }

        public void onCompostion(CompositionEventArgs comp)
        {
            CompositionEvent?.Invoke(comp);
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