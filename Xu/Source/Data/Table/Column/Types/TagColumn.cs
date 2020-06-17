/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

namespace Xu
{
    public class TagColumn : Column
    {
        public TagColumn(string name) => Name = name;

        public TagColumn(string name, string label)
        {
            Name = name;
            Label = label;
        }
    }
}
 