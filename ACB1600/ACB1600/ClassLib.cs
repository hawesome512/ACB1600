using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACB1600
{
        enum Authority
        {
                R,
                W,
                RW
        }
        class Node
        {
                public int Address
                {
                        get;
                        set;
                }
                public string Name
                {
                        get;
                        set;
                }
                public string Alias
                {
                        get;
                        set;
                }
                public string Unit
                {
                        get;
                        set;
                }
                public int ShowType
                {
                        get;
                        set;
                }
                public Authority NodeAuthority
                {
                        get;
                        set;
                }
                public string Extra
                {
                        get;
                        set;
                }
                public string Value
                {
                        get;
                        set;
                }
        }
}
