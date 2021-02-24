/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Xu
{
    public class Command : IOrdered, IDisposable
    {
        #region Dispose
        ~Command() => Dispose();

        public virtual void Dispose()
        {
            Stop();
        }
        #endregion

        public Action<TaskControl<float>> Action { get; set; } = (TaskControl<float> control) => { };

        public TaskControl<float> TaskControl { get; } = new TaskControl<float>();

        // Methods
        public virtual void Start()
        {
            if (Enabled)
            {
                if (!(TaskControl.Cts is null)) TaskControl.Cts = new CancellationTokenSource();
                Action.Invoke(TaskControl);
            }
        }

        public virtual void Stop() { if (TaskControl.Cts is not null) TaskControl.Cts.Cancel(); }

        public virtual bool Enabled { get; set; } = true;

        public int Order { get; set; } = 0;

        public string Label { get; set; } = "DCMD";

        public string Name { get; set; } = "Default Command";

        public string Description { get; set; } = "This is a default command description, please modify.";

        public Importance Importance { get; set; } = Importance.Minor;

        public ShortcutKey ShortcutKey { get; set; }

        public ColorTheme Theme { get; set; } = new ColorTheme();

        public Dictionary<IconType, Dictionary<Size, Bitmap>> IconList { get; set; } = new Dictionary<IconType, Dictionary<Size, Bitmap>>()
        {
            {
                IconType.Normal, new Dictionary<Size, Bitmap>() {
                { new Size(16, 16), Properties.Resources.Blank_16 },
                { new Size(24, 24), Properties.Resources.Blank_24 },
                { new Size(32, 32), Properties.Resources.Blank_32 }, }
            },
        };

        public static Bitmap GetIcon(Dictionary<Size, Bitmap> icon, Size sz)
        {
            if (icon.ContainsKey(sz))
                return icon[sz];
            else
            {
                int pixel = sz.Width * sz.Height;
                if (icon.Count > 0)
                {
                    Bitmap bm = icon.ElementAt(0).Value;
                    for (int i = 0; i < icon.Count; i++)
                    {
                        Size s = icon.ElementAt(i).Key;
                        if (pixel < s.Width * s.Height)
                        {
                            bm = icon.ElementAt(i).Value;
                        }
                    }
                    return new Bitmap(bm, sz);
                }
                else
                    return new Bitmap(Xu.Properties.Resources.Blank_32, sz);
            }
        }

        public virtual Bitmap GetIcon(Size sz, MouseState state, bool check)
        {
            switch (state)
            {
                case (MouseState.Hover):
                    if (check)
                    {
                        if (IconList.ContainsKey(IconType.CheckedHover))
                            return GetIcon(IconList[IconType.CheckedHover], sz);
                    }
                    if (IconList.ContainsKey(IconType.Hover))
                        return GetIcon(IconList[IconType.Hover], sz);
                    else
                        return GetIcon(IconList[IconType.Normal], sz);

                case (MouseState.Down):
                    if (check)
                    {
                        if (IconList.ContainsKey(IconType.CheckedDown))
                            return GetIcon(IconList[IconType.CheckedDown], sz);
                    }
                    if (IconList.ContainsKey(IconType.Down))
                        return GetIcon(IconList[IconType.Down], sz);
                    else
                        return GetIcon(IconList[IconType.Normal], sz);

                default:
                    if (check)
                    {
                        if (IconList.ContainsKey(IconType.Checked))
                            return GetIcon(IconList[IconType.Checked], sz);
                    }
                    return GetIcon(IconList[IconType.Normal], sz);
            }
        }

        public virtual void DrawIcon(Graphics g, Rectangle bound, MouseState state, bool check, bool enabled)
        {
            if (Enabled & enabled)
                g.DrawImageUnscaled(GetIcon(bound.Size, state, check), bound.Location);
            else
                ControlPaint.DrawImageDisabled(g, GetIcon(bound.Size, state, check), bound.Location.X, bound.Location.Y, Color.Transparent);
        }

        public virtual void DrawIcon(Graphics g, Rectangle bound, Color newc, MouseState state, bool check, bool enabled)
        {
            if (Enabled & enabled)
                g.DrawIcon(GetIcon(bound.Size, state, check), bound, newc);
            else
                ControlPaint.DrawImageDisabled(g, GetIcon(bound.Size, state, check), bound.Location.X, bound.Location.Y, Color.Transparent);
        }

        public virtual void DrawIconCenter(Graphics g, Size sz, Rectangle area, MouseState state, bool check, bool enabled)
            => DrawIcon(g, new Rectangle(area.X + (area.Width - sz.Width) / 2, area.Y + (area.Height - sz.Height) / 2, sz.Width, sz.Height), state, check, enabled);

        public virtual void DrawIconCenter(Graphics g, Size sz, Rectangle area, Color newc, MouseState state, bool check, bool enabled)
            => DrawIcon(g, new Rectangle(area.X + (area.Width - sz.Width) / 2, area.Y + (area.Height - sz.Height) / 2, sz.Width, sz.Height), newc, state, check, enabled);

    }
}
