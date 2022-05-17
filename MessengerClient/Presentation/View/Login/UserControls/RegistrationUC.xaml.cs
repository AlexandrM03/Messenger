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

namespace MessengerClient.Presentation.View.Login.UserControls
{
    /// <summary>
    /// Interaction logic for RegistrationUC.xaml
    /// </summary>
    public partial class RegistrationUC : UserControl
    {
        public RegistrationUC()
        {
            InitializeComponent();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).RegistrationModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void repeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).RegistrationModel.RepeatPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
