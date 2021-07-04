
namespace Xu.EE.FPGA.FW
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
            this.BtnImportXilinxPackageFile = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveFile = new System.Windows.Forms.SaveFileDialog();
            this.BtnExportCSVFile = new System.Windows.Forms.Button();
            this.BtnImportAltiumPinMapReport = new System.Windows.Forms.Button();
            this.BtnExportXilinxXDCFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnImportXilinxPackageFile
            // 
            this.BtnImportXilinxPackageFile.Location = new System.Drawing.Point(12, 12);
            this.BtnImportXilinxPackageFile.Name = "BtnImportXilinxPackageFile";
            this.BtnImportXilinxPackageFile.Size = new System.Drawing.Size(239, 52);
            this.BtnImportXilinxPackageFile.TabIndex = 0;
            this.BtnImportXilinxPackageFile.Text = "Import Xilinx Package File";
            this.BtnImportXilinxPackageFile.UseVisualStyleBackColor = true;
            this.BtnImportXilinxPackageFile.Click += new System.EventHandler(this.BtnImportXilinxPackageFile_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.FileName = "openFileDialog1";
            // 
            // BtnExportCSVFile
            // 
            this.BtnExportCSVFile.Location = new System.Drawing.Point(12, 230);
            this.BtnExportCSVFile.Name = "BtnExportCSVFile";
            this.BtnExportCSVFile.Size = new System.Drawing.Size(239, 52);
            this.BtnExportCSVFile.TabIndex = 1;
            this.BtnExportCSVFile.Text = "Export CSV File";
            this.BtnExportCSVFile.UseVisualStyleBackColor = true;
            this.BtnExportCSVFile.Click += new System.EventHandler(this.BtnExportCSVFile_Click);
            // 
            // BtnImportAltiumPinMapReport
            // 
            this.BtnImportAltiumPinMapReport.Location = new System.Drawing.Point(12, 70);
            this.BtnImportAltiumPinMapReport.Name = "BtnImportAltiumPinMapReport";
            this.BtnImportAltiumPinMapReport.Size = new System.Drawing.Size(239, 52);
            this.BtnImportAltiumPinMapReport.TabIndex = 2;
            this.BtnImportAltiumPinMapReport.Text = "Import Altium Pin Map Report";
            this.BtnImportAltiumPinMapReport.UseVisualStyleBackColor = true;
            this.BtnImportAltiumPinMapReport.Click += new System.EventHandler(this.BtnImportAltiumPinMapReport_Click);
            // 
            // BtnExportXilinxXDCFile
            // 
            this.BtnExportXilinxXDCFile.Location = new System.Drawing.Point(12, 128);
            this.BtnExportXilinxXDCFile.Name = "BtnExportXilinxXDCFile";
            this.BtnExportXilinxXDCFile.Size = new System.Drawing.Size(239, 52);
            this.BtnExportXilinxXDCFile.TabIndex = 3;
            this.BtnExportXilinxXDCFile.Text = "Export Xilinx XDC File";
            this.BtnExportXilinxXDCFile.UseVisualStyleBackColor = true;
            this.BtnExportXilinxXDCFile.Click += new System.EventHandler(this.BtnExportXilinxXDCFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.BtnExportXilinxXDCFile);
            this.Controls.Add(this.BtnImportAltiumPinMapReport);
            this.Controls.Add(this.BtnExportCSVFile);
            this.Controls.Add(this.BtnImportXilinxPackageFile);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnImportXilinxPackageFile;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.SaveFileDialog SaveFile;
        private System.Windows.Forms.Button BtnExportCSVFile;
        private System.Windows.Forms.Button BtnImportAltiumPinMapReport;
        private System.Windows.Forms.Button BtnExportXilinxXDCFile;
    }
}

