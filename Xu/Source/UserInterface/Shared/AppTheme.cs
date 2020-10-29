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
    public class AppTheme
    {
        public static readonly StringFormat TextAlignDefault = new StringFormat() { LineAlignment = StringAlignment.Near, Alignment = StringAlignment.Near };
        public static readonly StringFormat TextAlignLeft = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near };
        public static readonly StringFormat TextAlignRight = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Far };
        public static readonly StringFormat TextAlignCenter = new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };

        #region Fonts
        [Browsable(true), ReadOnly(false)]
        [Description("Font")]
        public Font Font { get; set; } = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font FontBold { get; set; } = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font TitleFont { get; set; } = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font SmallFont { get; set; } = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font TinyFont { get; set; } = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font ConsoleFont { get; set; } = new Font("Courier New", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        [Browsable(true), ReadOnly(false)]
        [Description("")]
        public Font SymbolFont { get; set; } = new Font("Segoe MDL2 Assets", 8.0F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        #endregion

        #region Colors

        public static SolidBrush TransparentBrush { get; set; } = new SolidBrush(Color.Transparent);
        public static SolidBrush WhiteBrush { get; set; } = new SolidBrush(Color.White);

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush PanelBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 245, 246, 247));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush CanvasBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 255, 253, 245));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush LightTextBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 250, 250, 250));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush GrayTextBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 120, 120, 120));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush DimTextBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 80, 80, 80));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public SolidBrush DarkTextBrush { get; set; } = new SolidBrush(Color.FromArgb(255, 30, 30, 30));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme Panel { get; set; } = new ColorTheme(
            Color.FromArgb(255, 30, 30, 30),
            Color.FromArgb(255, 245, 246, 247),
            Color.FromArgb(255, 218, 219, 220));


        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme Hover { get; set; } = new ColorTheme(
            Color.FromArgb(150, 32, 70, 80),
            Color.FromArgb(150, 200, 237, 235),
            Color.FromArgb(150, 32, 70, 80));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme Click { get; set; } = new ColorTheme(
            Color.FromArgb(80, 32, 70, 80),
            Color.FromArgb(80, 200, 237, 235),
            Color.FromArgb(80, 32, 70, 80));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme ActiveCursor { get; set; } = new ColorTheme(
            Color.FromArgb(255, 32, 70, 80), //Color.White,
            Color.FromArgb(255, 200, 237, 235),
            Color.FromArgb(255, 32, 70, 80));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme InactiveCursor { get; set; } = new ColorTheme(
            Color.Gray,
            Color.FromArgb(255, 208, 209, 205),
            Color.FromArgb(255, 133, 132, 130));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme Highlight { get; set; } = new ColorTheme(
            Color.White,
            Color.Orange,
            Color.DarkOrange);

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme Critical { get; set; } = new ColorTheme(
            Color.White,
            Color.Red,
            Color.Maroon);

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme SizeGrip { get; set; } = new ColorTheme(
            Color.Red,
            ControlPaint.Light(Color.FromArgb(255, 32, 70, 80), 1f),
            ControlPaint.Light(Color.FromArgb(255, 32, 70, 80), 1f));

        [Browsable(false), ReadOnly(false)]
        [Description("")]
        public ColorTheme HelperMask { get; set; } = new ColorTheme(
            Color.Red,
            ControlPaint.Light(Color.FromArgb(255, 32, 70, 80), 1f),
            Color.Red);

        #endregion

        public static bool DesignMode { get; set; } = false;
    }
}
