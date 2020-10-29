/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Xu
{
    [DesignerCategory("Code")]
    public class OrbMenuHost : ContextHost
    {
        public OrbMenuHost(OrbMenu om) : base(om)
        {
            OrbMenu = om;
        }

        protected OrbMenu OrbMenu { get; set; }


        public override void Close()
        {

            OrbMenu.MoForm.Invalidate(true);
            /*
            if (RibbonTab != null)
            {
                RibbonTab.HostContainer.DeActivate();
                RibbonTab.Ribbon.Invalidate(true);
            }*/
        }
    }


    [DesignerCategory("Code")]
    public class OrbMenu : UserControl
    {
        #region Ctor
        public OrbMenu(MosaicForm fm) // : base()
        {
            MoForm = fm;
        }
        #endregion
        #region Components
        public MosaicForm MoForm { get; protected set; }

        #endregion
        #region Control

        #endregion
        #region Coordinate
        public Rectangle OrbButtonRect;
        #endregion
        #region Paint


        public void PaintOrbControl(Graphics g, Font font, SolidBrush br, Color color)
        {
            if (OrbButtonRect != Rectangle.Empty)
            {
                switch (ButtonMouseState)
                {
                    case (MouseState.Out): g.FillRectangleStyle2010(OrbButtonRect, 0, color, ControlPaint.Light(color, 0.4f)); break;
                    case (MouseState.Hover): g.FillRectangleStyle2010(OrbButtonRect, 0, ControlPaint.Light(color, 0.1f), ControlPaint.Light(color, 0.5f)); break;
                    default: g.FillRectangleStyle2010(OrbButtonRect, 0, ControlPaint.Light(color, 0.3f), ControlPaint.Light(color, 0.1f)); break;
                }
                //g.DrawString(Description, Theme.FontBold, Theme.LightTextBrush, OrbButtonRect, Theme.TextAlignCenter);
            }
        }

        #endregion
        #region Mouse
        public MouseState ButtonMouseState = MouseState.Out;
        public bool ButtonIsHot
        {
            get { return (Enabled && ButtonMouseState == MouseState.Down); }
        }
        public void ButtonReset() { ButtonMouseState = MouseState.Out; }
        public bool ButtonMouseMove(Point pt)
        {
            bool GotPt = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (GotPt) ? MouseState.Hover : MouseState.Out;
            return GotPt;
        }
        public bool ButtonMouseDown(Point pt)
        {
            bool GotPt = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (GotPt) ? MouseState.Down : MouseState.Out;
            return GotPt;
        }
        public bool ButtonMouseUp(Point pt)
        {
            bool IsExec = OrbButtonRect.Contains(pt) & Enabled;
            ButtonMouseState = (IsExec) ? MouseState.Hover : MouseState.Out;
            if (IsExec)
            {



            }
            return IsExec;
        }
        #endregion
    }
}
