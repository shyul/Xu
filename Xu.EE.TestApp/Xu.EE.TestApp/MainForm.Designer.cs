
namespace Xu.EE.TestApp
{
    partial class MainForm
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
            this.BtnInitialize = new System.Windows.Forms.Button();
            this.TextBoxResouceName = new System.Windows.Forms.TextBox();
            this.TextBoxFrequency = new System.Windows.Forms.TextBox();
            this.TextBoxAmplitude = new System.Windows.Forms.TextBox();
            this.TextBoxDcOffset = new System.Windows.Forms.TextBox();
            this.TextBoxDutyCycle = new System.Windows.Forms.TextBox();
            this.BtnSetFGEN = new System.Windows.Forms.Button();
            this.BtnFgenON = new System.Windows.Forms.Button();
            this.BtnFgenOFF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnInitialize
            // 
            this.BtnInitialize.Location = new System.Drawing.Point(246, 89);
            this.BtnInitialize.Name = "BtnInitialize";
            this.BtnInitialize.Size = new System.Drawing.Size(75, 23);
            this.BtnInitialize.TabIndex = 0;
            this.BtnInitialize.Text = "Connect";
            this.BtnInitialize.UseVisualStyleBackColor = true;
            this.BtnInitialize.Click += new System.EventHandler(this.BtnInitialize_Click);
            // 
            // TextBoxResouceName
            // 
            this.TextBoxResouceName.Location = new System.Drawing.Point(130, 91);
            this.TextBoxResouceName.Name = "TextBoxResouceName";
            this.TextBoxResouceName.Size = new System.Drawing.Size(110, 20);
            this.TextBoxResouceName.TabIndex = 1;
            this.TextBoxResouceName.Text = "VB8012-309528E";
            // 
            // TextBoxFrequency
            // 
            this.TextBoxFrequency.Location = new System.Drawing.Point(130, 171);
            this.TextBoxFrequency.Name = "TextBoxFrequency";
            this.TextBoxFrequency.Size = new System.Drawing.Size(110, 20);
            this.TextBoxFrequency.TabIndex = 2;
            this.TextBoxFrequency.Text = "5e6";
            // 
            // TextBoxAmplitude
            // 
            this.TextBoxAmplitude.Location = new System.Drawing.Point(130, 197);
            this.TextBoxAmplitude.Name = "TextBoxAmplitude";
            this.TextBoxAmplitude.Size = new System.Drawing.Size(110, 20);
            this.TextBoxAmplitude.TabIndex = 3;
            this.TextBoxAmplitude.Text = "1";
            // 
            // TextBoxDcOffset
            // 
            this.TextBoxDcOffset.Location = new System.Drawing.Point(130, 223);
            this.TextBoxDcOffset.Name = "TextBoxDcOffset";
            this.TextBoxDcOffset.Size = new System.Drawing.Size(110, 20);
            this.TextBoxDcOffset.TabIndex = 4;
            this.TextBoxDcOffset.Text = "0";
            // 
            // TextBoxDutyCycle
            // 
            this.TextBoxDutyCycle.Location = new System.Drawing.Point(130, 249);
            this.TextBoxDutyCycle.Name = "TextBoxDutyCycle";
            this.TextBoxDutyCycle.Size = new System.Drawing.Size(110, 20);
            this.TextBoxDutyCycle.TabIndex = 5;
            this.TextBoxDutyCycle.Text = "50";
            // 
            // BtnSetFGEN
            // 
            this.BtnSetFGEN.Location = new System.Drawing.Point(288, 194);
            this.BtnSetFGEN.Name = "BtnSetFGEN";
            this.BtnSetFGEN.Size = new System.Drawing.Size(114, 23);
            this.BtnSetFGEN.TabIndex = 6;
            this.BtnSetFGEN.Text = "Update FGEN";
            this.BtnSetFGEN.UseVisualStyleBackColor = true;
            this.BtnSetFGEN.Click += new System.EventHandler(this.BtnSetFGEN_Click);
            // 
            // BtnFgenON
            // 
            this.BtnFgenON.Location = new System.Drawing.Point(288, 223);
            this.BtnFgenON.Name = "BtnFgenON";
            this.BtnFgenON.Size = new System.Drawing.Size(114, 23);
            this.BtnFgenON.TabIndex = 7;
            this.BtnFgenON.Text = "FGEN ON";
            this.BtnFgenON.UseVisualStyleBackColor = true;
            this.BtnFgenON.Click += new System.EventHandler(this.BtnFgenON_Click);
            // 
            // BtnFgenOFF
            // 
            this.BtnFgenOFF.Location = new System.Drawing.Point(288, 247);
            this.BtnFgenOFF.Name = "BtnFgenOFF";
            this.BtnFgenOFF.Size = new System.Drawing.Size(114, 23);
            this.BtnFgenOFF.TabIndex = 8;
            this.BtnFgenOFF.Text = "FGEN OFF";
            this.BtnFgenOFF.UseVisualStyleBackColor = true;
            this.BtnFgenOFF.Click += new System.EventHandler(this.BtnFgenOFF_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnFgenOFF);
            this.Controls.Add(this.BtnFgenON);
            this.Controls.Add(this.BtnSetFGEN);
            this.Controls.Add(this.TextBoxDutyCycle);
            this.Controls.Add(this.TextBoxDcOffset);
            this.Controls.Add(this.TextBoxAmplitude);
            this.Controls.Add(this.TextBoxFrequency);
            this.Controls.Add(this.TextBoxResouceName);
            this.Controls.Add(this.BtnInitialize);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnInitialize;
        private System.Windows.Forms.TextBox TextBoxResouceName;
        private System.Windows.Forms.TextBox TextBoxFrequency;
        private System.Windows.Forms.TextBox TextBoxAmplitude;
        private System.Windows.Forms.TextBox TextBoxDcOffset;
        private System.Windows.Forms.TextBox TextBoxDutyCycle;
        private System.Windows.Forms.Button BtnSetFGEN;
        private System.Windows.Forms.Button BtnFgenON;
        private System.Windows.Forms.Button BtnFgenOFF;
    }
}

