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
            iMEControl = ImeSharp.GetDefaultControl();
            iMEControl.Initialize(Handle);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                if (iMEControl.isIMEEnabled())
                    iMEControl.DisableIME();
                else
                    iMEControl.EnableIME();
        }
    }
}