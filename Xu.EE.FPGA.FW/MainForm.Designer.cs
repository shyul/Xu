
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
            this.BtnImportAlteraPackageFile = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnExportNets = new System.Windows.Forms.Button();
            this.BtnImportQuartusPinFile = new System.Windows.Forms.Button();
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
            this.BtnExportCSVFile.Location = new System.Drawing.Point(12, 438);
            this.BtnExportCSVFile.Name = "BtnExportCSVFile";
            this.BtnExportCSVFile.Size = new System.Drawing.Size(239, 52);
            this.BtnExportCSVFile.TabIndex = 1;
            this.BtnExportCSVFile.Text = "Export CSV File";
            this.BtnExportCSVFile.UseVisualStyleBackColor = true;
            this.BtnExportCSVFile.Click += new System.EventHandler(this.BtnExportCSVFile_Click);
            // 
            // BtnImportAltiumPinMapReport
            // 
            this.BtnImportAltiumPinMapReport.Location = new System.Drawing.Point(147, 98);
            this.BtnImportAltiumPinMapReport.Name = "BtnImportAltiumPinMapReport";
            this.BtnImportAltiumPinMapReport.Size = new System.Drawing.Size(239, 52);
            this.BtnImportAltiumPinMapReport.TabIndex = 2;
            this.BtnImportAltiumPinMapReport.Text = "Import Altium Pin Map Report";
            this.BtnImportAltiumPinMapReport.UseVisualStyleBackColor = true;
            this.BtnImportAltiumPinMapReport.Click += new System.EventHandler(this.BtnImportAltiumPinMapReport_Click);
            // 
            // BtnExportXilinxXDCFile
            // 
            this.BtnExportXilinxXDCFile.Location = new System.Drawing.Point(12, 192);
            this.BtnExportXilinxXDCFile.Name = "BtnExportXilinxXDCFile";
            this.BtnExportXilinxXDCFile.Size = new System.Drawing.Size(239, 52);
            this.BtnExportXilinxXDCFile.TabIndex = 3;
            this.BtnExportXilinxXDCFile.Text = "Export Vivado XDC File";
            this.BtnExportXilinxXDCFile.UseVisualStyleBackColor = true;
            this.BtnExportXilinxXDCFile.Click += new System.EventHandler(this.BtnExportXilinxXDCFile_Click);
            // 
            // BtnImportAlteraPackageFile
            // 
            this.BtnImportAlteraPackageFile.Location = new System.Drawing.Point(307, 12);
            this.BtnImportAlteraPackageFile.Name = "BtnImportAlteraPackageFile";
            this.BtnImportAlteraPackageFile.Size = new System.Drawing.Size(239, 52);
            this.BtnImportAlteraPackageFile.TabIndex = 4;
            this.BtnImportAlteraPackageFile.Text = "Import Altera Package File";
            this.BtnImportAlteraPackageFile.UseVisualStyleBackColor = true;
            this.BtnImportAlteraPackageFile.Click += new System.EventHandler(this.BtnImportAlteraPackageFile_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(307, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(239, 52);
            this.button1.TabIndex = 5;
            this.button1.Text = "Export Quartus QSF File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnExportNets
            // 
            this.BtnExportNets.Location = new System.Drawing.Point(307, 429);
            this.BtnExportNets.Name = "BtnExportNets";
            this.BtnExportNets.Size = new System.Drawing.Size(239, 52);
            this.BtnExportNets.TabIndex = 6;
            this.BtnExportNets.Text = "Export Nets";
            this.BtnExportNets.UseVisualStyleBackColor = true;
            this.BtnExportNets.Click += new System.EventHandler(this.BtnExportNets_Click);
            // 
            // BtnImportQuartusPinFile
            // 
            this.BtnImportQuartusPinFile.Location = new System.Drawing.Point(307, 250);
            this.BtnImportQuartusPinFile.Name = "BtnImportQuartusPinFile";
            this.BtnImportQuartusPinFile.Size = new System.Drawing.Size(239, 52);
            this.BtnImportQuartusPinFile.TabIndex = 7;
            this.BtnImportQuartusPinFile.Text = "Import Quartus Pin File";
            this.BtnImportQuartusPinFile.UseVisualStyleBackColor = true;
            this.BtnImportQuartusPinFile.Click += new System.EventHandler(this.BtnImportQuartusPinFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.BtnImportQuartusPinFile);
            this.Controls.Add(this.BtnExportNets);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnImportAlteraPackageFile);
            this.Controls.Add(this.BtnExportXilinxXDCFile);
            this.Controls.Add(this.BtnImportAltiumPinMapReport);
            this.Controls.Add(this.BtnExportCSVFile);
            this.Controls.Add(this.BtnImportXilinxPackageFile);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnImportXilinxPackageFile;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.SaveFileDialog SaveFile;
        private System.Windows.Forms.Button BtnExportCSVFile;
        private System.Windows.Forms.Button BtnImportAltiumPinMapReport;
        private System.Windows.Forms.Button BtnExportXilinxXDCFile;
        private System.Windows.Forms.Button BtnImportAlteraPackageFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnExportNets;
        private System.Windows.Forms.Button BtnImportQuartusPinFile;
    }
}

