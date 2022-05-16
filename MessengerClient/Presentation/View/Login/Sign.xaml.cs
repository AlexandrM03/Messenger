using MessengerClient.Logic.ViewModel.LoginVM;
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
using System.Windows.Shapes;

namespace MessengerClient.Presentation.View.Login
{
    /// <summary>
    /// Interaction logic for Sign.xaml
    /// </summary>
    public partial class Sign : Window
    {
        public Sign()
        {
            InitializeComponent();
            DataContext = new SignVM(this);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
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
