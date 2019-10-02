using System;
using System.Windows;
using System.Net;
using System.Net.Mail;
using System.Data;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для Mail.xaml
    /// </summary>
    public partial class Mail : Window
    {
        string[] text;
        Window wm;

        public Mail(Window wm, string[] text)
        {
            InitializeComponent();
            this.text = text;
            this.wm = wm;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("zoopark-2019@mail.ru", "Bank!");
                // кому отправляем
                MailAddress to = new MailAddress(TBMail.Text);
                // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Банк!";
                // текст письма
                m.Body = DB.getFormatedTextMail(text);
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("zoopark-2019@mail.ru", "Zoo228");
                smtp.EnableSsl = true;
                smtp.Send(m);
                MessageBox.Show("Банковская выписки оправлена на почту!");
                Close();
            }
            catch { MessageBox.Show("Ошибка!"); }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            wm.Close();
        }
    }
}
