using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cirno4Life
{
    public class Settings
    {
        public static Settings Current { get; set; }
        
        static Settings()
        {
            // TODO: Load setting
            Current = new Settings();
        }

        public double Opacity { get; set; } = 1;
        public HorizontalAlignment ImageHorizontalAlignment { get; set; } = HorizontalAlignment.Right;
        public VerticalAlignment ImageVerticalAlignment { get; set; } = VerticalAlignment.Bottom;
        public TimeSpan SlideInterval { get; set; } = TimeSpan.FromMilliseconds(3500);
        public TimeSpan SlideFadeOut { get; set; } = TimeSpan.FromMilliseconds(750);
        public TimeSpan SlideFadeIn { get; set; } = TimeSpan.FromMilliseconds(500);
    }
}
