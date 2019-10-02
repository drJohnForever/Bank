using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        Window wm;
        DB db;
        private readonly Regex regex = new Regex("[^0-9]+");
        private string TemplateText = "Оформление кредита на сумму: {0} руб.";

        public Client(Window wm, DB db)
        {
            InitializeComponent();
            this.wm = wm;
            this.db = db;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClientInfoLabel.Content = db.getClientInfo(1);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wm.Show();
            Close();
        }

        private void CreditTB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = db.getClientData(1);
            list.Add(string.Format(TemplateText, CreditTB.Text));
            Mail mail = new Mail(this, list.ToArray<string>());
            this.Hide();
            mail.Show();
        }
    }
}
