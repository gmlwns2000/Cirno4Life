using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Cirno4Life
{
    public class Settings : INotifyPropertyChanged
    {
        public static Settings Current { get; set; }

        public static void Load()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "settings.xml");
            if (File.Exists(path))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                using (StreamReader rdr = new StreamReader(path))
                {
                    var decoded = (Settings)xmlSerializer.Deserialize(rdr);
                    Current = decoded;
                }
            }
            else
            {
                Current = new Settings();
            }
        }

        public static void Save()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
            using (StreamWriter wr = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "settings.xml")))
            {
                xmlSerializer.Serialize(wr, Current);
            }
        }

        private double _opacity = 1;
        public double Opacity
        {
            get => _opacity;
            set { _opacity = value; OnPropertyChanged(); }
        }

        private HorizontalAlignment _imageHorizontalAlignment = HorizontalAlignment.Right;
        public HorizontalAlignment ImageHorizontalAlignment
        {
            get => _imageHorizontalAlignment;
            set { _imageHorizontalAlignment = value; OnPropertyChanged(); }
        }

        private VerticalAlignment _imageVerticalAlignment = VerticalAlignment.Bottom;
        public VerticalAlignment ImageVerticalAlignment
        {
            get => _imageVerticalAlignment;
            set { _imageVerticalAlignment = value; OnPropertyChanged(); }
        }

        private double _imageMaxSize = 600;
        public double ImageMaxSize
        {
            get => _imageMaxSize;
            set { _imageMaxSize = value; OnPropertyChanged(); }
        }

        private TimeSpan _slideInterval = TimeSpan.FromMilliseconds(6500);
        [XmlIgnore]
        public TimeSpan SlideInterval
        {
            get => _slideInterval;
            set { _slideInterval = value; OnPropertyChanged(); }
        }
        [Browsable(false)]
        [XmlElement("SlideInterval")]
        public double SlideIntervalMs
        {
            get => SlideInterval.TotalMilliseconds;
            set => SlideInterval = TimeSpan.FromMilliseconds(value);
        }

        private TimeSpan _slideFadeOut = TimeSpan.FromMilliseconds(750);
        [XmlIgnore]
        public TimeSpan SlideFadeOut
        {
            get => _slideFadeOut;
            set { _slideFadeOut = value; OnPropertyChanged(); }
        }
        [Browsable(false)]
        [XmlElement("SlideFadeOut")]
        public double SlideFadeoutMs
        {
            get => SlideFadeOut.TotalMilliseconds;
            set => SlideFadeOut = TimeSpan.FromMilliseconds(value);
        }

        private TimeSpan _slideFadeIn = TimeSpan.FromMilliseconds(500);
        [XmlIgnore]
        public TimeSpan SlideFadeIn
        {
            get => _slideFadeIn;
            set { _slideFadeIn = value; OnPropertyChanged(); }
        }
        [Browsable(false)]
        [XmlElement("SlideFadeIn")]
        public double SlideFadeinMs
        {
            get => SlideFadeIn.TotalMilliseconds;
            set => SlideFadeIn = TimeSpan.FromMilliseconds(value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
