using System;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        Window wm;

        public SplashScreen(Window wm)
        {
            InitializeComponent();
            this.wm = wm;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wm.Show();
            this.Close();
        }
    }
}
