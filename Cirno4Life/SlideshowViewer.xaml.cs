using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cirno4Life
{
    /// <summary>
    /// SlideshowViewer.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SlideshowViewer : UserControl
    {
        public static string[] ImageFormats = new string[] { "gif", "jpeg", "jpg", "png", "bmp" };

        public event EventHandler Nexted;

        public DirectoryInfo DirectoryInfo { get; set; }
        private TimeSpan _interval = TimeSpan.FromMilliseconds(3500);
        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                _interval = value;
                if(slideUpdater != null)
                    slideUpdater.Interval = value;
            }
        }
        public TimeSpan FadeOut { get; set; } = TimeSpan.FromMilliseconds(500);
        public TimeSpan FadeIn { get; set; } = TimeSpan.FromMilliseconds(500);

        private ImageRenderer image;
        private DispatcherTimer slideUpdater;
        private List<FileInfo> imageList = new List<FileInfo>();
        private int imgIndex;

        public SlideshowViewer(DirectoryInfo directory)
        {
            InitializeComponent();

            DirectoryInfo = directory;
        }

        private bool IsImage(string path)
        {
            foreach (var format in ImageFormats)
            {
                if (path.ToLower().EndsWith(format))
                    return true;
            }
            return false;
        }

        private void UpdateList()
        {
            if (DirectoryInfo.Exists)
            {
                var files = DirectoryInfo.GetFiles();
                foreach(var fi in files)
                {
                    if (IsImage(fi.FullName))
                    {
                        imageList.Add(fi);
                    }
                }
            }
        }

        public void Start()
        {
            if(slideUpdater == null)
            {
                slideUpdater = new DispatcherTimer();
                slideUpdater.Interval = Interval;
                slideUpdater.Tick += (o, s) => { Next(); };
            }

            Next();
            slideUpdater.Start();
        }

        public void Stop()
        {
            slideUpdater.Stop();
        }

        public void Next()
        {
            UpdateList();

            if (imageList.Count > 0)
            {
                if (image != null)
                {
                    var target = image;
                    var fadeOut = ConstructFadeOut(target, FadeOut);
                    fadeOut.Completed += delegate
                    {
                        Grid_Root.Children.Remove(target);
                    };
                    fadeOut.Begin();
                }

                imgIndex++;
                imgIndex %= imageList.Count;

                var file = imageList[imgIndex];
                image = new ImageRenderer(file.FullName)
                {
                    Opacity = 0,
                    HorizontalAlignment = Settings.Current.ImageHorizontalAlignment,
                    VerticalAlignment = Settings.Current.ImageVerticalAlignment
                };

                Storyboard fadeIn = ConstructFadeIn(image, FadeIn);
                fadeIn.Begin();

                Grid_Root.Children.Add(image);
            }

            Nexted?.Invoke(this, null);
        }

        private Storyboard ConstructFadeOut(DependencyObject target, TimeSpan duration)
        {
            return ConstructFade(target, OpacityProperty, 1, 0, new KeySpline(0, 0, 0, 1), duration);
        }

        private Storyboard ConstructFadeIn(DependencyObject target, TimeSpan duration)
        {
            return ConstructFade(target, OpacityProperty, 0, 1, new KeySpline(0, 0, 0, 1), duration);
        }

        private Storyboard ConstructFade(DependencyObject target, DependencyProperty targetProperty, double start, double to, KeySpline keySpline, TimeSpan duration)
        {
            Storyboard board = new Storyboard();
            DoubleAnimationUsingKeyFrames ani = new DoubleAnimationUsingKeyFrames();
            ani.KeyFrames.Add(new SplineDoubleKeyFrame(start, KeyTime.FromPercent(0), keySpline));
            ani.KeyFrames.Add(new SplineDoubleKeyFrame(to, KeyTime.FromPercent(1), keySpline));
            ani.Duration = new Duration(duration);
            board.Children.Add(ani);
            board.Duration = new Duration(duration);

            Storyboard.SetTarget(ani, target);
            Storyboard.SetTargetProperty(ani, new PropertyPath(targetProperty));

            return board;
        }
    }
}
