using System;
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

namespace MessengerClient
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


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CurrentClient.Client.Disconnect(CurrentUser.User.Id);

            Close();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (!imageTheme.Source.ToString().Contains("dark"))
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("Themes/Light.xaml"), UriKind.Relative);
                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.First(n =>
                    n.Source.OriginalString.StartsWith("Themes/"));
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dictionary);

                imageTheme.Source = new BitmapImage(new Uri(@"D:\4 семестр\КП ООП\Messenger\Resources\dark_icon.png"));
            }
            else
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("Themes/Dark.xaml"), UriKind.Relative);
                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.First(n =>
                    n.Source.OriginalString.StartsWith("Themes/"));
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dictionary);

                imageTheme.Source = new BitmapImage(new Uri(@"D:\4 семестр\КП ООП\Messenger\Resources\light_icon.png"));

            }
        }

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (!imageLanguage.Source.ToString().Contains("en_icon"))
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("Languages/Ru.xaml"), UriKind.Relative);
                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.First(n =>
                    n.Source.OriginalString.StartsWith("Languages/"));
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dictionary);

                imageLanguage.Source = new BitmapImage(new Uri(@"D:\4 семестр\КП ООП\Messenger\Resources\en_icon.png"));
            }
            else
            {
                ResourceDictionary dictionary = new ResourceDictionary();
                dictionary.Source = new Uri(String.Format("Languages/En.xaml"), UriKind.Relative);
                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.First(n =>
                    n.Source.OriginalString.StartsWith("Languages/"));
                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                Application.Current.Resources.MergedDictionaries.Insert(ind, dictionary);

                imageLanguage.Source = new BitmapImage(new Uri(@"D:\4 семестр\КП ООП\Messenger\Resources\ru_icon.png"));
            }
        }
    }
}
