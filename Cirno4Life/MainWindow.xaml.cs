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
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace Cirno4Life
{
    public partial class MainWindow : Window
    {
        public SlideshowViewer slideshow { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            var parent = new Window()
            {
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false,
                ShowActivated = false,
                Left = -213213,
                Top = -2312312,
            };
            parent.Show();
            Owner = parent;

            DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(Environment.CurrentDirectory, "cirno"));
            if (!di.Exists)
                di.Create();

            slideshow = new SlideshowViewer(di);
            slideshow.Start();
            Grid_Root.Children.Add(slideshow);

            InitNotify();
            
            SizeChanged += GifDisplayer_SizeChanged;
        }

        private void InitNotify()
        {
            NotifyIcon icon = new NotifyIcon();
            icon.Icon = System.Drawing.SystemIcons.Question;

            var menu = new System.Windows.Forms.ContextMenu();
            
            var settings = new System.Windows.Forms.MenuItem("Settings");
            settings.Click += delegate
            {
                var wnd = new SettingWindow();
                wnd.ShowDialog();
            };
            menu.MenuItems.Add(settings);

            var exit = new System.Windows.Forms.MenuItem("Exit");
            exit.Click += (o, s) => Environment.Exit(0);
            menu.MenuItems.Add(exit);

            icon.ContextMenu = menu;
            icon.Visible = true;
            icon.ShowBalloonTip(1, "ff", "ff", ToolTipIcon.None);
        }

        private void GifDisplayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var desktopWorkingArea = SystemParameters.WorkArea;
            Left = desktopWorkingArea.Right - e.NewSize.Width;
            Top = desktopWorkingArea.Bottom - e.NewSize.Height;
        }
    }
}
