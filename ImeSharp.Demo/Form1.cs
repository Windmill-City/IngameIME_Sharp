#define UILess

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImeSharp.Demo
{
    public partial class Form1 : Form
    {
        private IIMEControl iMEControl;
        private bool uiless = true;

        private String compStr = "";
        private String storedStr = "";

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            iMEControl = ImeSharp.Get_IMM32Control();
            initIMEControl(uiless);
        }

        private void initIMEControl(bool UILess)
        {
            if (UILess)
                iMEControl.Initialize(Handle, true);
            else
                iMEControl.Initialize(Handle);

            //Composition
            iMEControl.CompositionEvent += IMEControl_CompositionEvent;
            iMEControl.GetCompExtEvent += IMEControl_GetCompExtEvent;

            //CandidateList
            if (UILess)
                iMEControl.CandidateListEvent += IMEControl_CandidateListEvent;

            iMEControl.EnableIME();
            IMEStateChange.Text = "IMEState:" + (iMEControl.isIMEEnabled() ? "Enabled" : "Disabled");
            IMEAPIData.Text = iMEControl is TF_IMEControl ? "TF API" : "IMM32 API";
            UILessData.Text = UILess ? "TRUE" : "FALSE";
        }

        #region Handle CandidateList

        private void IMEControl_CandidateListEvent(CandidateList list)
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

        private void IMEControl_CompositionEvent(CompositionEventArgs comp)
        {
            label_CompCaret.Text = string.Format("Comp CaretPos: {0} ", comp.caretPos);

            storedStr += comp.strCommit;
            compStr = comp.strComp;

            label_DisplayStr.Text = storedStr + compStr;
        }

        #endregion Handle Composition

        #region Handle CompExt

        private void IMEControl_GetCompExtEvent(ref global::ImeSharp.RECT rect)
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

        #region Handle WinForm

        private void IMEStateChange_Click(object sender, EventArgs e)
        {
            if (iMEControl.isIMEEnabled())
                iMEControl.DisableIME();
            else
                iMEControl.EnableIME();
            IMEStateChange.Text = "IMEState:" + (iMEControl.isIMEEnabled() ? "Enabled" : "Disabled");
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

        private void IMEAPIData_Click(object sender, EventArgs e)
        {
            if (iMEControl is TF_IMEControl)
            {
                iMEControl.Dispose();
                iMEControl = ImeSharp.Get_IMM32Control();
            }
            else
            {
                iMEControl.Dispose();
                iMEControl = ImeSharp.Get_TFControl();
            }
            initIMEControl(uiless);
        }

        private void UILessData_Click(object sender, EventArgs e)
        {
            uiless = !uiless;
            if (iMEControl is TF_IMEControl)
            {
                iMEControl.Dispose();
                iMEControl = ImeSharp.Get_TFControl();
            }
            else
            {
                iMEControl.Dispose();
                iMEControl = ImeSharp.Get_IMM32Control();
            }
            initIMEControl(uiless);
        }
    }
}