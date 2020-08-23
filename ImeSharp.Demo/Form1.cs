﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
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
            iMEControl.GetCompExtEvent += this.IMEControl_GetCompExtEvent;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        private void IMEControl_GetCompExtEvent(IntPtr rRect)
        {
            RECT rect = (RECT)Marshal.PtrToStructure(rRect, typeof(RECT));//Map from

            Font f = new Font("Microsoft YaHei", 20F, FontStyle.Regular, GraphicsUnit.Pixel);
            Size sif2 = TextRenderer.MeasureText(labelComp.Text, f, new Size(0, 0), TextFormatFlags.NoPadding);
            //Map rect
            rect.left = labelComp.Location.X;
            rect.top = labelComp.Location.Y;
            //should use Font height, because some IME draw CompStr themselves, when CompStr is Empty
            //so the candidate window wont cover the text
            rect.bottom = rect.top + f.Height;
            rect.right = rect.left + sif2.Width;

            Marshal.StructureToPtr(rect, rRect, true);//Map to
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