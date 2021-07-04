
namespace Xu.EE.FPGA
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.BtnImportXilinxPackageFile = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(33, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(188, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import Xilinx Pin Map";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(33, 164);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 38);
            this.button2.TabIndex = 1;
            this.button2.Text = "Export XDC Constraint File";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(33, 208);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 38);
            this.button3.TabIndex = 2;
            this.button3.Text = "Import Vivado Pin Report";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(33, 252);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(188, 38);
            this.button4.TabIndex = 3;
            this.button4.Text = "Export Combined Check List";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // BtnImportXilinxPackageFile
            // 
            this.BtnImportXilinxPackageFile.Location = new System.Drawing.Point(33, 76);
            this.BtnImportXilinxPackageFile.Name = "BtnImportXilinxPackageFile";
            this.BtnImportXilinxPackageFile.Size = new System.Drawing.Size(188, 38);
            this.BtnImportXilinxPackageFile.TabIndex = 4;
            this.BtnImportXilinxPackageFile.Text = "Import Xilinx Package File";
            this.BtnImportXilinxPackageFile.UseVisualStyleBackColor = true;
            this.BtnImportXilinxPackageFile.Click += new System.EventHandler(this.BtnImportXilinxComponent_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.FileName = "*.txt";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnImportXilinxPackageFile);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "MainForm";
            this.Text = "Main Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button BtnImportXilinxPackageFile;
        private System.Windows.Forms.OpenFileDialog OpenFile;
    }
}

