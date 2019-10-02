using System;
using System.Data;
using System.Windows;

namespace Bank
{
    public partial class Table : Window
    {
        DB db = new DB();
        MainWindow mn;
        
        public Table(MainWindow mn)
        {
            InitializeComponent();
            this.mn = mn;
        }
        

        private void Window_Closed(object sender, EventArgs e)
        {
            mn.Close();
            Close();
        }

        private void CBType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string[] str = new string[3];
            //DataRow row = db.getInfoBiletType(CBType.SelectedValue.ToString()).Rows[0];
            //LBInfo.Content = db.getFilledInfo(row);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClientOutCredit COC = new ClientOutCredit(this, db);
            this.Hide();
            COC.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Tables tables = new Tables(this);
            this.Hide();
            tables.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Client clientPage = new Client(this, db);
            this.Hide();
            clientPage.Show();
        }

        //public string BuyedTicket()
        //{
        //    return db.addTicket((int)db.getInfoBiletType(CBType.SelectedValue.ToString()).Rows[0].ItemArray[0]);
        //}
    }
}
