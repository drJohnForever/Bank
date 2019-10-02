using System;
using System.Data;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для BuyBilet.xaml
    /// </summary>
    public partial class BuyBilet : Window
    {
        DataRow row;
        Window wm; 
        public BuyBilet(DataRow row, Window wm)
        {
            InitializeComponent();
            this.row = row;
            LBPrice.Content = string.Format("К оплате: {0} руб", row.ItemArray[3]);
            this.wm = wm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Mail mail = new Mail(this, row, (Table)wm);
            //this.Hide();
            //mail.Show();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wm.Show();
        }
    }
}
