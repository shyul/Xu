/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Universal Event
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class EventDockPanel : DockForm
    {
        public EventDockPanel(string formName) : base(formName)
        {
            HasIcon = true;
            Btn_Pin.Enabled = true;
            Btn_Close.Enabled = true;

            BackColor = Color.FromArgb(255, 255, 253, 245);
            Icon = Properties.Resources.Blank_16;

            ResumeLayout(false);
            PerformLayout();
        }

        protected int m_TextHeight = 10;
        protected int m_MaxLineNum = 0;

        protected override void CoordinateLayout()
        {
            ResumeLayout(true);
            m_TextHeight = TextRenderer.MeasureText("Google", Main.Theme.ConsoleFont).Height + 3;
            m_MaxLineNum = (int)Math.Floor((double)Height / (m_TextHeight));
            PerformLayout();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g = pe.Graphics;
            //g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            int startIndex = ObsoletedEvent.OutputMessages.Count - m_MaxLineNum;
            if (startIndex < 0) startIndex = 0;

            int BaseY = Height;
            for (int i = ObsoletedEvent.OutputMessages.Count - 1; i >= 0; i--)
            {
                string[] lines = ObsoletedEvent.OutputMessages[i].Split('\n');

                for (int j = lines.Length - 1; j >= 0; j--)
                {
                    BaseY -= m_TextHeight;
                    g.DrawString(lines[j], Main.Theme.ConsoleFont, Brushes.DimGray, new Point(3, BaseY));
                }
                if (BaseY < 0) break;
            }
        }
    }
}
