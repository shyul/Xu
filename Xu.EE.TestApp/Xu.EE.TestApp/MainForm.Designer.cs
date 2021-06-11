
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
            this.BtnTestMSO = new System.Windows.Forms.Button();
            this.TextBoxP25Voltage = new System.Windows.Forms.TextBox();
            this.TextBoxN25Voltage = new System.Windows.Forms.TextBox();
            this.TextBoxP6Voltage = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnP25WriteSettings = new System.Windows.Forms.Button();
            this.TextBoxP25Current = new System.Windows.Forms.TextBox();
            this.TextBoxN25Current = new System.Windows.Forms.TextBox();
            this.TextBoxP6Current = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnN25WriteSettings = new System.Windows.Forms.Button();
            this.BtnP6WriteSettings = new System.Windows.Forms.Button();
            this.BtnPowerTurnON = new System.Windows.Forms.Button();
            this.BtnPowerTurnOFF = new System.Windows.Forms.Button();
            this.BtnTestDMMConfig = new System.Windows.Forms.Button();
            this.BtnTestDMMRead = new System.Windows.Forms.Button();
            this.TextBoxKeySight = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.BtnConnectMSO = new System.Windows.Forms.Button();
            this.BtnConnectFGEN = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.TextBoxKeySightFGEN = new System.Windows.Forms.TextBox();
            this.BtnTestImportAlteraPins = new System.Windows.Forms.Button();
            this.OpenFile = new System.Windows.Forms.OpenFileDialog();
            this.BtnImportCSVPins = new System.Windows.Forms.Button();
            this.BtnTestFTDI = new System.Windows.Forms.Button();
            this.BtnTestFTDISendData = new System.Windows.Forms.Button();
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
            // BtnTestMSO
            // 
            this.BtnTestMSO.Location = new System.Drawing.Point(288, 323);
            this.BtnTestMSO.Name = "BtnTestMSO";
            this.BtnTestMSO.Size = new System.Drawing.Size(114, 23);
            this.BtnTestMSO.TabIndex = 9;
            this.BtnTestMSO.Text = "Test MSO";
            this.BtnTestMSO.UseVisualStyleBackColor = true;
            this.BtnTestMSO.Click += new System.EventHandler(this.BtnTestMSO_Click);
            // 
            // TextBoxP25Voltage
            // 
            this.TextBoxP25Voltage.Location = new System.Drawing.Point(525, 42);
            this.TextBoxP25Voltage.Name = "TextBoxP25Voltage";
            this.TextBoxP25Voltage.Size = new System.Drawing.Size(100, 20);
            this.TextBoxP25Voltage.TabIndex = 10;
            this.TextBoxP25Voltage.Text = "5";
            // 
            // TextBoxN25Voltage
            // 
            this.TextBoxN25Voltage.Location = new System.Drawing.Point(525, 68);
            this.TextBoxN25Voltage.Name = "TextBoxN25Voltage";
            this.TextBoxN25Voltage.Size = new System.Drawing.Size(100, 20);
            this.TextBoxN25Voltage.TabIndex = 11;
            this.TextBoxN25Voltage.Text = "-5";
            // 
            // TextBoxP6Voltage
            // 
            this.TextBoxP6Voltage.Location = new System.Drawing.Point(525, 94);
            this.TextBoxP6Voltage.Name = "TextBoxP6Voltage";
            this.TextBoxP6Voltage.Size = new System.Drawing.Size(100, 20);
            this.TextBoxP6Voltage.TabIndex = 12;
            this.TextBoxP6Voltage.Text = "3.3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(487, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "+25V";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(490, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "-25V";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(493, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "+6V";
            // 
            // BtnP25WriteSettings
            // 
            this.BtnP25WriteSettings.Location = new System.Drawing.Point(737, 40);
            this.BtnP25WriteSettings.Name = "BtnP25WriteSettings";
            this.BtnP25WriteSettings.Size = new System.Drawing.Size(93, 23);
            this.BtnP25WriteSettings.TabIndex = 16;
            this.BtnP25WriteSettings.Text = "Write Setting";
            this.BtnP25WriteSettings.UseVisualStyleBackColor = true;
            this.BtnP25WriteSettings.Click += new System.EventHandler(this.BtnP25WriteSettings_Click);
            // 
            // TextBoxP25Current
            // 
            this.TextBoxP25Current.Location = new System.Drawing.Point(631, 42);
            this.TextBoxP25Current.Name = "TextBoxP25Current";
            this.TextBoxP25Current.Size = new System.Drawing.Size(100, 20);
            this.TextBoxP25Current.TabIndex = 19;
            this.TextBoxP25Current.Text = "0.5";
            // 
            // TextBoxN25Current
            // 
            this.TextBoxN25Current.Location = new System.Drawing.Point(631, 68);
            this.TextBoxN25Current.Name = "TextBoxN25Current";
            this.TextBoxN25Current.Size = new System.Drawing.Size(100, 20);
            this.TextBoxN25Current.TabIndex = 20;
            this.TextBoxN25Current.Text = "0.5";
            // 
            // TextBoxP6Current
            // 
            this.TextBoxP6Current.Location = new System.Drawing.Point(631, 94);
            this.TextBoxP6Current.Name = "TextBoxP6Current";
            this.TextBoxP6Current.Size = new System.Drawing.Size(100, 20);
            this.TextBoxP6Current.TabIndex = 21;
            this.TextBoxP6Current.Text = "1";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(524, 144);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(110, 20);
            this.textBox7.TabIndex = 22;
            this.textBox7.Text = "192.168.44.12";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(640, 142);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 23;
            this.button4.Text = "Connect";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(553, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Voltage";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(661, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Current";
            // 
            // BtnN25WriteSettings
            // 
            this.BtnN25WriteSettings.Location = new System.Drawing.Point(737, 65);
            this.BtnN25WriteSettings.Name = "BtnN25WriteSettings";
            this.BtnN25WriteSettings.Size = new System.Drawing.Size(93, 23);
            this.BtnN25WriteSettings.TabIndex = 26;
            this.BtnN25WriteSettings.Text = "Write Setting";
            this.BtnN25WriteSettings.UseVisualStyleBackColor = true;
            this.BtnN25WriteSettings.Click += new System.EventHandler(this.BtnN25WriteSettings_Click);
            // 
            // BtnP6WriteSettings
            // 
            this.BtnP6WriteSettings.Location = new System.Drawing.Point(737, 91);
            this.BtnP6WriteSettings.Name = "BtnP6WriteSettings";
            this.BtnP6WriteSettings.Size = new System.Drawing.Size(93, 23);
            this.BtnP6WriteSettings.TabIndex = 27;
            this.BtnP6WriteSettings.Text = "Write Setting";
            this.BtnP6WriteSettings.UseVisualStyleBackColor = true;
            this.BtnP6WriteSettings.Click += new System.EventHandler(this.BtnP6WriteSettings_Click);
            // 
            // BtnPowerTurnON
            // 
            this.BtnPowerTurnON.Location = new System.Drawing.Point(836, 40);
            this.BtnPowerTurnON.Name = "BtnPowerTurnON";
            this.BtnPowerTurnON.Size = new System.Drawing.Size(34, 23);
            this.BtnPowerTurnON.TabIndex = 28;
            this.BtnPowerTurnON.Text = "ON";
            this.BtnPowerTurnON.UseVisualStyleBackColor = true;
            this.BtnPowerTurnON.Click += new System.EventHandler(this.BtnPowerTurnON_Click);
            // 
            // BtnPowerTurnOFF
            // 
            this.BtnPowerTurnOFF.Location = new System.Drawing.Point(876, 40);
            this.BtnPowerTurnOFF.Name = "BtnPowerTurnOFF";
            this.BtnPowerTurnOFF.Size = new System.Drawing.Size(39, 23);
            this.BtnPowerTurnOFF.TabIndex = 31;
            this.BtnPowerTurnOFF.Text = "OFF";
            this.BtnPowerTurnOFF.UseVisualStyleBackColor = true;
            this.BtnPowerTurnOFF.Click += new System.EventHandler(this.BtnPowerTurnOFF_Click);
            // 
            // BtnTestDMMConfig
            // 
            this.BtnTestDMMConfig.Location = new System.Drawing.Point(443, 245);
            this.BtnTestDMMConfig.Name = "BtnTestDMMConfig";
            this.BtnTestDMMConfig.Size = new System.Drawing.Size(115, 23);
            this.BtnTestDMMConfig.TabIndex = 32;
            this.BtnTestDMMConfig.Text = "Test DMM Config";
            this.BtnTestDMMConfig.UseVisualStyleBackColor = true;
            this.BtnTestDMMConfig.Click += new System.EventHandler(this.BtnTestDMMConfig_Click);
            // 
            // BtnTestDMMRead
            // 
            this.BtnTestDMMRead.Location = new System.Drawing.Point(443, 274);
            this.BtnTestDMMRead.Name = "BtnTestDMMRead";
            this.BtnTestDMMRead.Size = new System.Drawing.Size(115, 23);
            this.BtnTestDMMRead.TabIndex = 33;
            this.BtnTestDMMRead.Text = "Test DMM Read";
            this.BtnTestDMMRead.UseVisualStyleBackColor = true;
            this.BtnTestDMMRead.Click += new System.EventHandler(this.BtnTestDMMRead_Click);
            // 
            // TextBoxKeySight
            // 
            this.TextBoxKeySight.Location = new System.Drawing.Point(130, 64);
            this.TextBoxKeySight.Name = "TextBoxKeySight";
            this.TextBoxKeySight.Size = new System.Drawing.Size(236, 20);
            this.TextBoxKeySight.TabIndex = 34;
            this.TextBoxKeySight.Text = "USB0::0x0957::0x1799::MY54490786::INSTR";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(108, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "VirtualBench Address";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 36;
            this.label7.Text = "KeySight MSO Address";
            // 
            // BtnConnectMSO
            // 
            this.BtnConnectMSO.Location = new System.Drawing.Point(372, 62);
            this.BtnConnectMSO.Name = "BtnConnectMSO";
            this.BtnConnectMSO.Size = new System.Drawing.Size(75, 23);
            this.BtnConnectMSO.TabIndex = 37;
            this.BtnConnectMSO.Text = "Connect";
            this.BtnConnectMSO.UseVisualStyleBackColor = true;
            this.BtnConnectMSO.Click += new System.EventHandler(this.BtnConnectMSO_Click);
            // 
            // BtnConnectFGEN
            // 
            this.BtnConnectFGEN.Location = new System.Drawing.Point(372, 33);
            this.BtnConnectFGEN.Name = "BtnConnectFGEN";
            this.BtnConnectFGEN.Size = new System.Drawing.Size(75, 23);
            this.BtnConnectFGEN.TabIndex = 40;
            this.BtnConnectFGEN.Text = "Connect";
            this.BtnConnectFGEN.UseVisualStyleBackColor = true;
            this.BtnConnectFGEN.Click += new System.EventHandler(this.BtnConnectFGEN_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 13);
            this.label8.TabIndex = 39;
            this.label8.Text = "KeySight FGEN Address";
            // 
            // TextBoxKeySightFGEN
            // 
            this.TextBoxKeySightFGEN.Location = new System.Drawing.Point(130, 35);
            this.TextBoxKeySightFGEN.Name = "TextBoxKeySightFGEN";
            this.TextBoxKeySightFGEN.Size = new System.Drawing.Size(236, 20);
            this.TextBoxKeySightFGEN.TabIndex = 38;
            this.TextBoxKeySightFGEN.Text = "USB0::0x0957::0x2C07::MY59001326::INSTR";
            // 
            // BtnTestImportAlteraPins
            // 
            this.BtnTestImportAlteraPins.Location = new System.Drawing.Point(595, 339);
            this.BtnTestImportAlteraPins.Name = "BtnTestImportAlteraPins";
            this.BtnTestImportAlteraPins.Size = new System.Drawing.Size(135, 23);
            this.BtnTestImportAlteraPins.TabIndex = 41;
            this.BtnTestImportAlteraPins.Text = "Import Altera Pins";
            this.BtnTestImportAlteraPins.UseVisualStyleBackColor = true;
            this.BtnTestImportAlteraPins.Click += new System.EventHandler(this.BtnTestImportAlteraPins_Click);
            // 
            // OpenFile
            // 
            this.OpenFile.FileName = "pin.csv";
            // 
            // BtnImportCSVPins
            // 
            this.BtnImportCSVPins.Location = new System.Drawing.Point(596, 368);
            this.BtnImportCSVPins.Name = "BtnImportCSVPins";
            this.BtnImportCSVPins.Size = new System.Drawing.Size(135, 23);
            this.BtnImportCSVPins.TabIndex = 42;
            this.BtnImportCSVPins.Text = "Import Csv Pins";
            this.BtnImportCSVPins.UseVisualStyleBackColor = true;
            this.BtnImportCSVPins.Click += new System.EventHandler(this.BtnImportCSVPins_Click);
            // 
            // BtnTestFTDI
            // 
            this.BtnTestFTDI.Location = new System.Drawing.Point(68, 401);
            this.BtnTestFTDI.Name = "BtnTestFTDI";
            this.BtnTestFTDI.Size = new System.Drawing.Size(114, 23);
            this.BtnTestFTDI.TabIndex = 43;
            this.BtnTestFTDI.Text = "Test FTDI";
            this.BtnTestFTDI.UseVisualStyleBackColor = true;
            this.BtnTestFTDI.Click += new System.EventHandler(this.BtnTestFTDI_Click);
            // 
            // BtnTestFTDISendData
            // 
            this.BtnTestFTDISendData.Location = new System.Drawing.Point(188, 401);
            this.BtnTestFTDISendData.Name = "BtnTestFTDISendData";
            this.BtnTestFTDISendData.Size = new System.Drawing.Size(145, 23);
            this.BtnTestFTDISendData.TabIndex = 44;
            this.BtnTestFTDISendData.Text = "Test FTDI Send Data";
            this.BtnTestFTDISendData.UseVisualStyleBackColor = true;
            this.BtnTestFTDISendData.Click += new System.EventHandler(this.BtnTestFTDISendData_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 450);
            this.Controls.Add(this.BtnTestFTDISendData);
            this.Controls.Add(this.BtnTestFTDI);
            this.Controls.Add(this.BtnImportCSVPins);
            this.Controls.Add(this.BtnTestImportAlteraPins);
            this.Controls.Add(this.BtnConnectFGEN);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.TextBoxKeySightFGEN);
            this.Controls.Add(this.BtnConnectMSO);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TextBoxKeySight);
            this.Controls.Add(this.BtnTestDMMRead);
            this.Controls.Add(this.BtnTestDMMConfig);
            this.Controls.Add(this.BtnPowerTurnOFF);
            this.Controls.Add(this.BtnPowerTurnON);
            this.Controls.Add(this.BtnP6WriteSettings);
            this.Controls.Add(this.BtnN25WriteSettings);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.TextBoxP6Current);
            this.Controls.Add(this.TextBoxN25Current);
            this.Controls.Add(this.TextBoxP25Current);
            this.Controls.Add(this.BtnP25WriteSettings);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxP6Voltage);
            this.Controls.Add(this.TextBoxN25Voltage);
            this.Controls.Add(this.TextBoxP25Voltage);
            this.Controls.Add(this.BtnTestMSO);
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
        private System.Windows.Forms.Button BtnTestMSO;
        private System.Windows.Forms.TextBox TextBoxP25Voltage;
        private System.Windows.Forms.TextBox TextBoxN25Voltage;
        private System.Windows.Forms.TextBox TextBoxP6Voltage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnP25WriteSettings;
        private System.Windows.Forms.TextBox TextBoxP25Current;
        private System.Windows.Forms.TextBox TextBoxN25Current;
        private System.Windows.Forms.TextBox TextBoxP6Current;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnN25WriteSettings;
        private System.Windows.Forms.Button BtnP6WriteSettings;
        private System.Windows.Forms.Button BtnPowerTurnON;
        private System.Windows.Forms.Button BtnPowerTurnOFF;
        private System.Windows.Forms.Button BtnTestDMMConfig;
        private System.Windows.Forms.Button BtnTestDMMRead;
        private System.Windows.Forms.TextBox TextBoxKeySight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button BtnConnectMSO;
        private System.Windows.Forms.Button BtnConnectFGEN;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TextBoxKeySightFGEN;
        private System.Windows.Forms.Button BtnTestImportAlteraPins;
        private System.Windows.Forms.OpenFileDialog OpenFile;
        private System.Windows.Forms.Button BtnImportCSVPins;
        private System.Windows.Forms.Button BtnTestFTDI;
        private System.Windows.Forms.Button BtnTestFTDISendData;
    }
}

