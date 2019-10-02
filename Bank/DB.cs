using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Bank
{
    public class DB
    {
        const string AuthName = "[Authorization]";
        /// <summary>
        /// Экземляр класса SQLConnnection, позволяющая обращаться к базе данных. 
        /// </summary>

        public SqlConnection sql = new SqlConnection(string.Format("Data Source = {0}; Initial Catalog = {1}; Persist Security Info = {2}; integrated security = {3}", "Home", "Binbank", "True", "True"));
        /// <summary>
        /// Коллекция всех таблиц, удобна в использовании.
        /// </summary>

        public enum RoleID
        {
            Admin = 1, Specialist, Client
        }

        public DB(string[] str = null)
        {
            try
            {
                if (str != null || str[0] != String.Empty)
                    sql = new SqlConnection("Data Source = " + str[2] + "; Initial Catalog =" + str[3] + ";" +
    " Persist Security Info = true; User ID = " + str[0] + "; Password = \"" + str[1] + "\"");
            }
            catch { }
            sql.Open();
        }

        public enum Views
        {
            Specialist,
            Services,
            Visit,
            Client,
            BankOperation,
            TimeWork,
            Role,
            Auth,
            Special,
            BankOut
        }

        static string TemplateClientData = "<html><head><meta charset=\"utf-8\"><title>Банковская выписка</title></head><body><table border=\"1\" width=\"100%\" cellpadding=\"5\"><center><caption>Банковская выписка</caption></center><tr><th>Имя</th><th>Фамилия</th><th>Отчество</th><th>День рождения</th><th>Серия паспорта</th><th>Номер паспорта</th><th>Банковская операция</th></tr><tr><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td></tr></table></body></html>";
        static string QueryClientInfo = "select IdClient as 'Номер', FirstName as 'Имя', Surname as 'Фамилия', Otch as 'Отчество', Birthday as 'День рождения', Number_passport as 'Номер паспорта', Serial_passport as 'Серия паспорта' from Client where IdClient = {0}";
        static string TemplateClientInfo = "Имя: {1}\nФамилия: {2}\nОтчество: {3}\nДень рождения: {4}\nСерия и номер паспорта: {5} {6}";
        public string GetHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input + "Roma"));
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Коллекция, в которой хранятся все команды (запросы).
        /// </summary>
        static public List<string> commandsView = new List<string>()
    {"select IdSpecialist as 'Номер', FirstName as 'Имя', Surname as 'Фамилия', Otch as 'Отчество', Birthday as 'День рождения', Number_passport as 'Номер паспорта', Serial_passport as 'Серия паспорта', Special as 'Специальность' from Specialist join Special on Special.IdSpecial = Specialist.IdSpecial ",
        //"select IdServices as 'Номер', Name_Services as 'Наименование' from dbo.Services ",
        "select Visit.IdVisit as 'Номер', Visit.Date_Visit as 'Дата', Visit.Time_Visit as 'Время', Client.FirstName as 'Имя', Client.Surname as 'Фамилия', Client.Otch as 'Отчество', SpecialsView.Специальность as 'Специалист' from Visit join Client on Visit.IdClient = Client.IdClient join Auth on Auth.IdClient = Client.IdClient join SpecialsView on SpecialsView.Номер = Auth.IdSpecialist",
        "select IdClient as 'Номер', FirstName as 'Имя', Surname as 'Фамилия', Otch as 'Отчество', Birthday as 'День рождения', Number_passport as 'Номер паспорта', Serial_passport as 'Серия паспорта' from Client",
        "select IdBankOperation as 'Номер', Number_Bank_Operation as 'Номер банковской операции', Data_Bank_Operation as 'Дата' from BankOperation ",
        "select IdTimeWork as 'Номер', Start_work as 'Начало работы', End_work as 'Фамилия', IdSpecialist as 'Номер специалиста' from TimeWork ",
        "select IdRole as 'Номер', [Role] as 'Роль' from Role_List ",
        "select IdAuth as 'Номер', [Login] as 'Логин', [Password] as 'Пароль', IdRole as 'Номер роли', IdSpecialist as 'Номер специалиста', IdClient as 'Номер клиента' from Auth",
        "select IdSpecial as 'Номер', Special as 'Специальность' from dbo.[Special]",
        "select IdBankOut as 'Номер', Number_Bank_Out as 'Номер банковской выписки', IdVisit as 'Номер посещения', IdTimeWork as 'Номер времени работы', IdBankOperation as 'Номер банковской операции' from BankOut"
    };
       

        public DataTable GetFilledTable(byte index)
        {
            DataTable tempDT = new DataTable();
            tempDT.Load((SqlDataReader)new SqlCommand(commandsView[index], sql).ExecuteReader());
            return tempDT;
        }

        //public void InsertToTable(byte index, string[] inputs)
        //{
        //    sql.Open();
        //    TablesLoad(index);
        //    tables[index].Load((SqlDataReader)new SqlCommand(string.Format(commandsInsert[index], inputs), sql).ExecuteReader());
        //    sql.Close();
        //}

        //public void DeleteToTable(byte index, uint rowIndex)
        //{
        //    sql.Open();
        //    TablesLoad(index);
        //    tables[index].Load((SqlDataReader)new SqlCommand(string.Format(commandsDelete[index], rowIndex), sql).ExecuteReader());
        //    sql.Close();
        //}

        /// <summary>
        /// Фильтрация таблицы
        /// </summary>
        /// <param name="index">Индекс таблицы</param>
        /// <param name="likeString">Текст поиска</param>
        /// <param name="inputs">Столбцы таблицы</param>
        /// <returns>Отфильтрованная таблица</returns>
        public DataTable FiltherTable(byte index, string likeString, string[] inputs)
        {
            DataTable tempDT = new DataTable();
            sql.Open();
            string com = commandsView[index] + " where ";
            for (byte i = 0; i < inputs.Length; i++) com += string.Format(" {0} like '%{1}%'", '{' + i.ToString() + '}', likeString) + ((i < inputs.Length - 1) ? " or" : string.Empty);
            tempDT.Load((SqlDataReader)new SqlCommand(string.Format(com, inputs), sql).ExecuteReader());
            sql.Close();
            return tempDT;
        }
        /// <summary>
        /// Сортировка таблицы.
        /// </summary>
        /// <param name="index">Индекс таблицы.</param>
        /// <param name="header">Заголовок столбца.</param>
        /// <param name="e">Событие</param>
        /// <returns></returns>
        //static public DataTable TableSorting(byte index, string header, GridViewSortEventArgs e)
        //{
        //    sql.Open();
        //    DataTable tempDT = new DataTable();
        //    e.SortDirection = (e.SortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
        //    tempDT.Load((SqlDataReader)new SqlCommand(string.Format("{0} order by {1} {2}", commands[index], header, e.SortDirection == SortDirection.Ascending ? "Asc" : "Desc"), sql).ExecuteReader());
        //    sql.Close();
        //    return tempDT;
        //}
        /// <summary>
        /// Авторизация в системе.
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        /// <returns>Тип аккаунта</returns>
        public int Authorization(string Login, string Password)
        {
            try
            {
                SqlCommand command = new SqlCommand("select [dbo].[Authorization](@Login,@Password)", sql);
                command.Parameters.Add(new SqlParameter() { ParameterName = "@Login", Value = Login });
                command.Parameters.Add(new SqlParameter() { ParameterName = "@Password", Value = GetHash(Password) });
                return int.Parse(command.ExecuteScalar().ToString());
            }
            catch { return 0; }
        }
        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        /// <param name="Role_Id">Роль в системе</param>
        /// <param name="System_Access">Доступ к системе (0 - нет, 1 - есть) </param>
        public byte Registration(string Login, string Password, byte Role_Id = (byte)RoleID.Client, string idClient = "null", string idSpesialist = "null")
        {
            SqlCommand command = new SqlCommand(
                string.Format("exec [dbo].[Registration] '{0}', '{1}', {2}, {3}, {4}", Login, GetHash(Password), Role_Id,
                idClient.Equals(String.Empty) ? "null" : idClient, idSpesialist.Equals(string.Empty) ? "null" : idSpesialist), sql);
            return byte.Parse(command.ExecuteScalar().ToString());
        }

        public string getClientInfo(byte index)
        {
            DataTable tempDT = new DataTable();
            tempDT.Load((SqlDataReader)new SqlCommand(string.Format(QueryClientInfo, 1), sql).ExecuteReader());
            return string.Format(TemplateClientInfo, tempDT.Rows[0].ItemArray);
        }

        public List<string> getClientData(byte index)
        {
            List<string> list = new List<string>();
            DataTable tempDT = new DataTable();
            tempDT.Load((SqlDataReader)new SqlCommand(string.Format(QueryClientInfo, 1), sql).ExecuteReader());
            foreach (var temp in tempDT.Rows[0].ItemArray)
                list.Add(temp.ToString());
            return list;
        }

        static public string getFormatedTextMail(string[] text)
        {
            return string.Format(TemplateClientData, text);
        }

    }

}

