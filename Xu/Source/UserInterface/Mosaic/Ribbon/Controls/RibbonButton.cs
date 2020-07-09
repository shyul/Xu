/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class RibbonButton : ButtonWidget, IStackable
    {
        public RibbonButton(Command cmd, int order = 0, Importance importance = Importance.Minor)
            : base(cmd, false, false, true, 1, order, importance)
        {
            Command = cmd;

            HoverFillBrush = Main.Theme.Hover.FillBrush;
            ClickFillBrush = Main.Theme.Click.FillBrush;
            HoverEdgePen = Main.Theme.Hover.EdgePen;
            ClickEdgePen = Main.Theme.Click.EdgePen;

            Coordinate();
            ResumeLayout(false);
            PerformLayout();
        }

        public override string Label => Command.Label;

        public override string Description => Command.Description;

        public override Color BackColor => Main.Theme.Panel.FillColor;

        #region Coordinate

        public bool IsSectionEnd { get { return (m_IsSectionEnd || Importance > Importance.Minor || (IsLineEnd && StackedY >= 2)); } set { m_IsSectionEnd = value; } }
        protected bool m_IsSectionEnd = false;

        public bool IsLineEnd { get { return (m_IsLineEnd || Importance > Importance.Minor); } set { m_IsLineEnd = value; } }
        protected bool m_IsLineEnd = false;

        public int StackedY { get; set; } = 0;

        public int SectionIndex { get; set; } = 0;

        protected Rectangle m_IconRect = Rectangle.Empty;

        protected Rectangle m_LabelRects1 = Rectangle.Empty;

        protected Rectangle m_LabelRects2 = Rectangle.Empty;

        protected string[] m_LabelLines;

        protected int m_LineWidth = 0;

        public void BreakTextLine(int maxWidth, int maxLineCnt)
        {
            m_LabelLines = Label.Wordwarp(Main.Theme.Font, maxLineCnt, maxWidth, out m_LineWidth).ToArray();
        }

        public override void Coordinate()
        {
            SuspendLayout();

            if (Importance > Importance.Minor)
            {
                BreakTextLine(80, 2);
                int actualWidth = 36;
                if (m_LineWidth > actualWidth) actualWidth = m_LineWidth;
                if (actualWidth < 40) actualWidth = 40;
                //actualWidth += 6;
                Size = new Size(actualWidth, 66);
                //m_IconRect = new Rectangle(0, 0, actualWidth, 40);
                m_IconRect = new Rectangle((actualWidth - 32) / 2, 3, 32, 32);
                if (m_LabelLines.Length == 1)
                {
                    m_LabelRects1 = new Rectangle(0, m_IconRect.Bottom, actualWidth, Height - m_IconRect.Height);
                    m_LabelRects2 = Rectangle.Empty;
                }
                else
                {
                    m_LabelRects1 = new Rectangle(0, m_IconRect.Bottom + 1, actualWidth, (Height - m_IconRect.Height) / 2 - 1);
                    m_LabelRects2 = new Rectangle(0, m_LabelRects1.Bottom - 2, actualWidth, (Height - m_IconRect.Height) / 2 - 1);
                }
            }
            else if (Importance == Importance.Minor)
            {
                BreakTextLine(200, 1);
                m_IconRect = new Rectangle(3, 3, 16, 16);
                m_LabelRects1 = new Rectangle(m_IconRect.Right + 2, 0, m_LineWidth, 22);
                m_LabelRects2 = Rectangle.Empty;
                Size = new Size(m_LabelRects1.Right, 22);
            }
            else
            {
                Size = new Size(22, 22);
                m_IconRect = new Rectangle(3, 3, 16, 16);
                m_LabelRects1 = Rectangle.Empty;
                m_LabelRects2 = Rectangle.Empty;
            }

            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        #region Paint

        protected virtual Brush TextBrush => (Command.Enabled && Enabled) ? Main.Theme.Panel.ForeBrush : Main.Theme.GrayTextBrush;

        public override void PaintControl(Graphics g)
        {
            if (Command.Enabled && Enabled)
                if (MouseState == MouseState.Hover)
                {
                    g.FillRectangle(HoverFillBrush, ControlRect);
                    g.DrawRectangle(HoverEdgePen, ControlRect);
                }
                else if (MouseState == MouseState.Down)
                {
                    g.FillRectangle(ClickFillBrush, ControlRect);
                    g.DrawRectangle(ClickEdgePen, ControlRect);
                }

            if (m_IconRect != Rectangle.Empty)
            {
                Command.DrawIcon(g, m_IconRect, MouseState, false, Enabled);
            }

            if (Importance > Importance.Minor)
            {
                if (m_LabelLines.Length == 1)
                {
                    g.DrawString(m_LabelLines[0], Main.Theme.Font, TextBrush, m_LabelRects1, AppTheme.TextAlignCenter);
                }
                else if (m_LabelLines.Length > 1)
                {
                    if (m_LabelRects1 != Rectangle.Empty)
                        g.DrawString(m_LabelLines[0], Main.Theme.Font, TextBrush, m_LabelRects1, AppTheme.TextAlignCenter);
                    if (m_LabelRects2 != Rectangle.Empty)
                        g.DrawString(m_LabelLines[1], Main.Theme.Font, TextBrush, m_LabelRects2, AppTheme.TextAlignCenter);
                }
            }
            else if (Importance == Importance.Minor)
            {
                if (m_LabelRects1 != Rectangle.Empty)
                    g.DrawString(m_LabelLines[0], Main.Theme.Font, TextBrush, m_LabelRects1, AppTheme.TextAlignLeft);
            }
        }
        #endregion
    }
}