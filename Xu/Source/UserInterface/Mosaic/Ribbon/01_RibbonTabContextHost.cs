/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System.ComponentModel;

namespace Xu
{
    [DesignerCategory("Code")]
    public class RibbonTabContextHost : ContextHost
    {
        public RibbonTabContextHost(RibbonTabItem rt) : base(rt.Panel)
        {
            RibbonTab = rt;
        }

        protected RibbonTabItem RibbonTab;
        public bool IsShrink => RibbonTab.IsShrink;

        public override void Close()
        {
            if (RibbonTab != null)
            {
                RibbonTab.HostContainer.DeActivate();
                RibbonTab.Ribbon.Invalidate(true);
            }
        }
    }
}
