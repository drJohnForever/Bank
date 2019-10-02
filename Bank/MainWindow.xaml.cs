using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DB db = new DB();
        SplashScreen sp;
        DispatcherTimer timer = new DispatcherTimer();
        byte i = 0;

        public MainWindow()
        {
            InitializeComponent();

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            i++;
            if (i == 10)
            {
                sp.Close();
                timer.Stop();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int role = db.Authorization(TBLogin.Text, PasswordBox.Password);
            if (role == 0)
            {
                MessageBox.Show("Неверная комбинация логина и пароля!");
                return;
            }
            else showTable(role);
        }

        private void showTable(int role)
        {
            Table table = new Table(this);
            Hide();
            table.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Reg reg = new Reg(this);
            this.Hide();
            reg.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sp = new SplashScreen(this);
            this.Hide();
            sp.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpConection con = new OpConection(this);
            this.Hide();
            con.Show();
        }
    }
}
