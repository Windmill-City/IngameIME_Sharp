using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IngameIME_Sharp;

namespace ImeSharp.Demo
{
    public partial class Form1 : Form
    {
        private BaseIME_Sharp api;

        private String compStr = "";
        private String storedStr = "";

        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //api = new IMM();
            api = new TSF();
            initIMEControl();
        }

        private void initIMEControl()
        {
            api.Initialize(Handle);
            //Composition
            api.m_compositionHandler.eventComposition += M_compositionHandler_eventComposition; ;
            api.m_compositionHandler.eventGetTextExt += M_compositionHandler_eventGetTextExt; ;

            //CandidateList
            api.m_candidateListWrapper.eventCandidateList += M_candidateListWrapper_eventCandidateList;

            //AlphaMode
            api.eventAlphaMode += Api_eventAlphaMode;

            api.setState(true);
            IMEStateChange.Text = "IMEState:" + (api.State() ? "Enabled" : "Disabled");
            IMEAPIData.Text = api is TSF ? "TF API" : "IMM32 API";
            UILessData.Text = api.FullScreen() ? "TRUE" : "FALSE";
        }

        #region Handle CandidateList

        private void M_candidateListWrapper_eventCandidateList(refCandidateList list)
        {
            CandListData.Text = "";
            foreach (var cand in list.Candidates)
            {
                CandListData.Text += cand;
                CandListData.Text += "\r\n";
            }
        }

        #endregion Handle CandidateList

        #region Handle Composition

        private void M_compositionHandler_eventComposition(refCompositionEventArgs comp)
        {
            label_CompCaret.Text = string.Format("Comp CaretPos: {0} ", comp.m_lCaretPos);

            storedStr += comp.m_strCommit;
            compStr = comp.m_strComposition;

            label_DisplayStr.Text = storedStr + compStr;
        }

        #endregion Handle Composition

        #region Handle CompExt

        private void M_compositionHandler_eventGetTextExt(refRECT rect)
        {
            Font f = new Font("Microsoft YaHei", 20F, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);
            Size sif = TextRenderer.MeasureText(storedStr, f, new Size(0, 0), TextFormatFlags.NoPadding);
            Size sif2 = TextRenderer.MeasureText(compStr, f, new Size(0, 0), TextFormatFlags.NoPadding);
            //Map rect
            rect.left = label_DisplayStr.Location.X + sif.Width;
            rect.top = label_DisplayStr.Location.Y;
            //should use Font height, because some IME draw CompStr themselves, when CompStr is Empty
            //so the candidate window wont cover the text
            rect.bottom = rect.top + f.Height;
            rect.right = rect.left + sif2.Width;
        }

        #endregion Handle CompExt

        #region Handle AlphaMode

        private void Api_eventAlphaMode(bool alphaMode)
        {
            AlphaMode.Text = alphaMode ? "TRUE" : "FALSE";
        }

        #endregion

        #region Handle WinForm

        private void IMEStateChange_Click(object sender, EventArgs e)
        {
            api.setState(!api.State());
            IMEStateChange.Text = "IMEState:" + (api.State() ? "Enabled" : "Disabled");
        }

        private void UILessData_Click(object sender, EventArgs e)
        {
            api.setFullScreen(!api.FullScreen());
            UILessData.Text = api.FullScreen() ? "TRUE" : "FALSE";
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == 0x102)//WM_CHAR
            {
                storedStr += (char)msg.WParam;
                label_DisplayStr.Text = storedStr + compStr;
            }
            return base.PreProcessMessage(ref msg);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Back:
                    if (storedStr.Length > 0)
                        storedStr = storedStr.Remove(storedStr.Length - 1, 1);
                    this.label_DisplayStr.Text = storedStr + compStr;
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion Handle WinForm
    }
}