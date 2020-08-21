namespace ImeSharp.Demo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(574, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "IME Mode:";
            // 
            // imeModeState
            // 
            this.imeModeState.AutoSize = true;
            this.imeModeState.Location = new System.Drawing.Point(639, 149);
            this.imeModeState.Name = "imeModeState";
            this.imeModeState.Size = new System.Drawing.Size(47, 12);
            this.imeModeState.TabIndex = 9;
            this.imeModeState.Text = "Unknown";
            this.imeModeState.Click += new System.EventHandler(this.imeModeState_Click);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.imeModeState);
            this.Controls.Add(this.label3);
            this.Text = "Form1";
        }

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label imeModeState;
        #endregion Windows Form Designer generated code
    }
}