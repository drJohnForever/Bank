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
using System.Windows.Shapes;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для OpConection.xaml
    /// </summary>
    public partial class OpConection : Window
    {
        MainWindow mw;

        public OpConection(MainWindow mw)
        {
            InitializeComponent();
            this.mw = mw;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Fun);
            thread.Start();
            ((Button)sender).IsEnabled = false;
        }

        private void Fun()
        {
            try
            {
                mw.db = new DB(new string[] { TBLogin.Text, TBPassword.Text, TBServer.Text, "Binbank" });
            }
            catch { MessageBox.Show("Ошибка подключения!"); }
            finally
            {
                mw.Show();
                this.Close();
            }
        }
    }
}
