using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using EO.WebBrowser.Wpf;

namespace EOBrowserDispose
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            var control = new WebControl()
            {
                WebView = new WebView()
            };

            BrowserHost.Children.Add(control);
        }

        private void Unload_Click(object sender, RoutedEventArgs e)
        {
            if (BrowserHost.Children.Count == 0)
            {
                MessageBox.Show("Not loaded.");
                return;
            }

            var webControl = (WebControl)BrowserHost.Children[0];

            WeakReference webControlRef = new WeakReference(webControl);
            WeakReference webViewRef = new WeakReference(webControl.WebView);

            BrowserHost.Children.Remove(webControl);

            if (MessageBox.Show("Clean up web view?", "?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                webControl.WebView.Dispose();
                webControl.WebView = null;
            }

            webControl = null;

            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            if (webControlRef.IsAlive)
                MessageBox.Show("Failed to unload web control.");

            if (webViewRef.IsAlive)
                MessageBox.Show("Failed to unload web view.");
        }

        private void Dialog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dialog();

            dialog.ShowDialog();

            WeakReference webControlRef = new WeakReference(dialog.WebControl);
            WeakReference webViewRef = new WeakReference(dialog.WebControl.WebView);

            if (MessageBox.Show("Clean up web view?", "?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dialog.WebControl.WebView.Dispose();
                dialog.WebControl.WebView = null;
            }

            dialog = null;

            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();

            if (webControlRef.IsAlive)
                MessageBox.Show("Failed to unload web control.");

            if (webViewRef.IsAlive)
                MessageBox.Show("Failed to unload web view.");
        }
    }
}
