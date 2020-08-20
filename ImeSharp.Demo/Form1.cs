using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImeSharp.Demo
{
    public partial class Form1 : Form
    {
        private IIMEControl iMEControl;

        public Form1()
        {
            InitializeComponent();
            KeyDown += Form1_KeyDown;

            iMEControl = ImeSharp.GetDefaultControl();
            //iMEControl = ImeSharp.Get_IMM32Control();
            iMEControl.Initialize(Handle);
            iMEControl.EnableIME();
            iMEControl.CompStrEvent += IMEControl_CompStrEvent;
            iMEControl.CommitEvent += IMEControl_CommitEvent;
        }

        private void IMEControl_CommitEvent(string commit)
        {
            textBoxResult.Text += commit;
        }

        private void IMEControl_CompStrEvent(string comp)
        {
            labelComp.Text = comp;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && textBoxResult.Text.Length > 0)
                textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 1);
        }
        private void label1_Click(object sender, EventArgs e)
        {
            if (iMEControl.isIMEEnabled())
            {
                iMEControl.DisableIME();
                label1.Text = "IME Disabled";
            }
            else
            {
                iMEControl.EnableIME();
                label1.Text = "IME Enabled";
            }
        }
    }
}