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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Cirno4Life
{
    public partial class MainWindow : Window
    {
        public SlideshowViewer SlideShow { get; set; }

        private SettingWindow settingWindow;

        public MainWindow()
        {
            InitializeComponent();

            var parent = new Window()
            {
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false,
                ShowActivated = false,
                Left = -100000,
                Top = -100000,
            };
            parent.Show();
            Owner = parent;

            InitSlideShow();
            InitNotify();
            InitSettings();

            SizeChanged += OnSizeChanged;
            SourceInitialized += MainWindow_SourceInitialized;
            Closed += MainWindow_Closed;

            SlideShow.Start();
        }

        #region Initializers

        private void InitSettings()
        {
            Settings.Current.PropertyChanged += Current_PropertyChanged;

            Opacity = Settings.Current.Opacity;
            SlideShow.FadeIn = Settings.Current.SlideFadeIn;
            SlideShow.FadeOut = Settings.Current.SlideFadeOut;
            SlideShow.Interval = Settings.Current.SlideInterval;
            SlideShow.MaxSize = Settings.Current.ImageMaxSize;
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Settings.Opacity):
                    Opacity = Settings.Current.Opacity;
                    break;
                case nameof(Settings.ImageHorizontalAlignment):
                    OnSizeChanged(null, null);
                    break;
                case nameof(Settings.ImageVerticalAlignment):
                    OnSizeChanged(null, null);
                    break;
                case nameof(Settings.SlideFadeIn):
                    SlideShow.FadeIn = Settings.Current.SlideFadeIn;
                    break;
                case nameof(Settings.SlideFadeOut):
                    SlideShow.FadeOut = Settings.Current.SlideFadeOut;
                    break;
                case nameof(Settings.SlideInterval):
                    SlideShow.Interval = Settings.Current.SlideInterval;
                    break;
                case nameof(Settings.ImageMaxSize):
                    SlideShow.MaxSize = Settings.Current.ImageMaxSize;
                    break;
            }
        }

        private void InitSlideShow()
        {
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "cirno"));
            if (!di.Exists)
                di.Create();

            SlideShow = new SlideshowViewer(di);
            Grid_Root.Children.Add(SlideShow);
        }

        private void InitNotify()
        {
            NotifyIcon icon = new NotifyIcon();

            StreamResourceInfo sri = System.Windows.Application.GetResourceStream(new Uri("/Cirno4Life;component/cirno.ico", UriKind.Relative));
            if (sri != null)
                using (Stream s = sri.Stream)
                    icon.Icon = new System.Drawing.Icon(s);
            else
                icon.Icon = System.Drawing.SystemIcons.Error;

            var menu = new System.Windows.Forms.ContextMenu();

            var next = new System.Windows.Forms.MenuItem("Next");
            next.Click += (o, s) => SlideShow.Next();
            menu.MenuItems.Add(next);
            
            var settings = new System.Windows.Forms.MenuItem("Settings");
            settings.Click += delegate
            {
                if (settingWindow == null)
                {
                    settingWindow = new SettingWindow();
                    settingWindow.Closed += delegate { settingWindow = null; };
                    settingWindow.Show();
                    settingWindow.Activate();
                }
                else
                {
                    settingWindow.Activate();
                }
            };
            menu.MenuItems.Add(settings);

            var exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += (o, s) => Close();
            menu.MenuItems.Add(exit);

            icon.Click += (o, s) => SlideShow.Next();
            icon.Text = "Cirno4Life : Animated Gif Widget";
            icon.ContextMenu = menu;
            icon.Visible = true;
            icon.ShowBalloonTip(1, "test", "test", ToolTipIcon.None);
        }

        #endregion Initializers

        #region Events

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Settings.Save();
            Environment.Exit(0);
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            WinApi.SetTransClick(this);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var workArea = SystemParameters.WorkArea;

            switch (Settings.Current.ImageHorizontalAlignment)
            {
                case System.Windows.HorizontalAlignment.Left:
                    Left = workArea.Left;
                    break;
                case System.Windows.HorizontalAlignment.Center:
                    Left = (workArea.Left + workArea.Right) * 0.5 - ActualWidth * 0.5;
                    break;
                default:
                case System.Windows.HorizontalAlignment.Right:
                    Left = workArea.Right - ActualWidth;
                    break;
            }

            switch (Settings.Current.ImageVerticalAlignment)
            {
                case VerticalAlignment.Top:
                    Top = workArea.Top;
                    break;
                case VerticalAlignment.Center:
                    Top = (workArea.Top + workArea.Bottom) * 0.5 - ActualHeight * 0.5;
                    break;
                default:
                case VerticalAlignment.Bottom:
                    Top = workArea.Bottom - ActualHeight;
                    break;
            }
        }

        #endregion Events
    }
}
