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
            this.IMEStateChange = new System.Windows.Forms.Label();
            this.label_DisplayStr = new System.Windows.Forms.Label();
            this.label_CompCaret = new System.Windows.Forms.Label();
            this.label_CandName = new System.Windows.Forms.Label();
            this.CandListData = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.IMEAPIName = new System.Windows.Forms.Label();
            this.IMEAPIData = new System.Windows.Forms.Label();
            this.UILessName = new System.Windows.Forms.Label();
            this.Click2Change2 = new System.Windows.Forms.Label();
            this.UILessData = new System.Windows.Forms.Label();
            this.WarningLabel = new System.Windows.Forms.Label();
            this.DrawData = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.AlphaMode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // IMEStateChange
            // 
            this.IMEStateChange.AutoSize = true;
            this.IMEStateChange.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IMEStateChange.Location = new System.Drawing.Point(12, 419);
            this.IMEStateChange.Name = "IMEStateChange";
            this.IMEStateChange.Size = new System.Drawing.Size(186, 22);
            this.IMEStateChange.TabIndex = 2;
            this.IMEStateChange.Text = "IMEState:Unknown";
            this.IMEStateChange.Click += new System.EventHandler(this.IMEStateChange_Click);
            // 
            // label_DisplayStr
            // 
            this.label_DisplayStr.AutoSize = true;
            this.label_DisplayStr.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_DisplayStr.Location = new System.Drawing.Point(12, 9);
            this.label_DisplayStr.Name = "label_DisplayStr";
            this.label_DisplayStr.Size = new System.Drawing.Size(119, 30);
            this.label_DisplayStr.TabIndex = 3;
            this.label_DisplayStr.Text = "Test Input";
            // 
            // label_CompCaret
            // 
            this.label_CompCaret.AutoSize = true;
            this.label_CompCaret.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CompCaret.Location = new System.Drawing.Point(567, 419);
            this.label_CompCaret.Name = "label_CompCaret";
            this.label_CompCaret.Size = new System.Drawing.Size(186, 22);
            this.label_CompCaret.TabIndex = 4;
            this.label_CompCaret.Text = "Comp CaretPos: 0";
            // 
            // label_CandName
            // 
            this.label_CandName.AutoSize = true;
            this.label_CandName.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_CandName.Location = new System.Drawing.Point(13, 109);
            this.label_CandName.Name = "label_CandName";
            this.label_CandName.Size = new System.Drawing.Size(164, 22);
            this.label_CandName.TabIndex = 5;
            this.label_CandName.Text = "CandidateList:";
            // 
            // CandListData
            // 
            this.CandListData.AutoSize = true;
            this.CandListData.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CandListData.Location = new System.Drawing.Point(15, 144);
            this.CandListData.Name = "CandListData";
            this.CandListData.Size = new System.Drawing.Size(88, 16);
            this.CandListData.TabIndex = 6;
            this.CandListData.Text = "Candidates";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 407);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "ClickToChange";
            // 
            // IMEAPIName
            // 
            this.IMEAPIName.AutoSize = true;
            this.IMEAPIName.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IMEAPIName.Location = new System.Drawing.Point(479, 109);
            this.IMEAPIName.Name = "IMEAPIName";
            this.IMEAPIName.Size = new System.Drawing.Size(98, 22);
            this.IMEAPIName.TabIndex = 8;
            this.IMEAPIName.Text = "IMEAPI：";
            // 
            // IMEAPIData
            // 
            this.IMEAPIData.AutoSize = true;
            this.IMEAPIData.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.IMEAPIData.Location = new System.Drawing.Point(583, 109);
            this.IMEAPIData.Name = "IMEAPIData";
            this.IMEAPIData.Size = new System.Drawing.Size(87, 22);
            this.IMEAPIData.TabIndex = 9;
            this.IMEAPIData.Text = "Unknown";
            // 
            // UILessName
            // 
            this.UILessName.AutoSize = true;
            this.UILessName.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UILessName.Location = new System.Drawing.Point(479, 163);
            this.UILessName.Name = "UILessName";
            this.UILessName.Size = new System.Drawing.Size(87, 22);
            this.UILessName.TabIndex = 8;
            this.UILessName.Text = "UILess:";
            // 
            // Click2Change2
            // 
            this.Click2Change2.AutoSize = true;
            this.Click2Change2.Location = new System.Drawing.Point(585, 148);
            this.Click2Change2.Name = "Click2Change2";
            this.Click2Change2.Size = new System.Drawing.Size(83, 12);
            this.Click2Change2.TabIndex = 7;
            this.Click2Change2.Text = "ClickToChange";
            // 
            // UILessData
            // 
            this.UILessData.AutoSize = true;
            this.UILessData.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UILessData.Location = new System.Drawing.Point(583, 163);
            this.UILessData.Name = "UILessData";
            this.UILessData.Size = new System.Drawing.Size(87, 22);
            this.UILessData.TabIndex = 10;
            this.UILessData.Text = "Unknown";
            this.UILessData.Click += new System.EventHandler(this.UILessData_Click);
            // 
            // WarningLabel
            // 
            this.WarningLabel.AutoSize = true;
            this.WarningLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.WarningLabel.Location = new System.Drawing.Point(15, 81);
            this.WarningLabel.Name = "WarningLabel";
            this.WarningLabel.Size = new System.Drawing.Size(368, 16);
            this.WarningLabel.TabIndex = 11;
            this.WarningLabel.Text = "To change IMEAPI, you need to change the code";
            // 
            // DrawData
            // 
            this.DrawData.AutoSize = true;
            this.DrawData.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DrawData.Location = new System.Drawing.Point(567, 386);
            this.DrawData.Name = "DrawData";
            this.DrawData.Size = new System.Drawing.Size(54, 22);
            this.DrawData.TabIndex = 12;
            this.DrawData.Text = "0000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(446, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 22);
            this.label1.TabIndex = 13;
            this.label1.Text = "AlphaMode:";
            // 
            // AlphaMode
            // 
            this.AlphaMode.AutoSize = true;
            this.AlphaMode.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.AlphaMode.Location = new System.Drawing.Point(583, 208);
            this.AlphaMode.Name = "AlphaMode";
            this.AlphaMode.Size = new System.Drawing.Size(87, 22);
            this.AlphaMode.TabIndex = 14;
            this.AlphaMode.Text = "Unknown";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.AlphaMode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DrawData);
            this.Controls.Add(this.WarningLabel);
            this.Controls.Add(this.UILessData);
            this.Controls.Add(this.IMEAPIData);
            this.Controls.Add(this.UILessName);
            this.Controls.Add(this.IMEAPIName);
            this.Controls.Add(this.Click2Change2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CandListData);
            this.Controls.Add(this.label_CandName);
            this.Controls.Add(this.label_CompCaret);
            this.Controls.Add(this.label_DisplayStr);
            this.Controls.Add(this.IMEStateChange);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label IMEStateChange;
        private System.Windows.Forms.Label label_DisplayStr;
        private System.Windows.Forms.Label label_CompCaret;
        private System.Windows.Forms.Label label_CandName;
        private System.Windows.Forms.Label CandListData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label IMEAPIName;
        private System.Windows.Forms.Label IMEAPIData;
        private System.Windows.Forms.Label UILessName;
        private System.Windows.Forms.Label Click2Change2;
        private System.Windows.Forms.Label UILessData;
        private System.Windows.Forms.Label WarningLabel;
        private System.Windows.Forms.Label DrawData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AlphaMode;
    }
}

