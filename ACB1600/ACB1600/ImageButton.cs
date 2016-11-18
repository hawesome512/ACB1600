using System;
using System.Windows;
using System.Windows.Controls;

namespace ACB1600
{
        public class ImageButton : Button
        {
                private string m_imagepath;

                public string ImgPath
                {
                        get
                        {
                                return m_imagepath;
                        }
                        set
                        {
                                m_imagepath = value;
                        }
                }
        }
}