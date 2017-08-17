﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cirno4Life
{
    /// <summary>
    /// ImageRenderer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageRenderer : UserControl
    {
        private string path;
        public string FilePath
        {
            get => path;
            set
            {
                if (path != value)
                {
                    path = value;

                    var img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(path);
                    img.EndInit();

                    Width = img.Width;
                    Height = img.Height;

                    WpfAnimatedGif.ImageBehavior.SetAnimatedSource(Image, img);
                }
            }
        }

        public ImageRenderer(string filepath)
        {
            InitializeComponent();

            FilePath = filepath;
        }
    }
}
