using System;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace Bank
{
    /// <summary>
    /// Логика взаимодействия для Tables.xaml
    /// </summary>
    public partial class Tables : Window
    {
        Word.Application winword = new Word.Application(); //создаем COM-объект Word
        Word.Document document = null; // объявление и инициализация документа
        Window mw;
        DataGrid[] DGs;
        DB db = new DB();
        SqlCommand[] coms = new SqlCommand[9];

        public Tables(Window mw)
        {
            InitializeComponent();
            this.mw = mw;
            DGs = new DataGrid[] { DGSpec, DGVisit, DGClients, DGBankOperation, DGTimeWorl, DGRole, DGAccounts, DGSpecial, DGBankOut };
            for (byte i = 0; i < DGs.Length; i++)
                coms[i] = new SqlCommand(DB.commandsView[i], db.sql);
            ShowTables();

        }

        private void ShowTables()
        {
            for (byte i = 0; i < DGs.Length; i++)
            {
                DataTable tempdt = new DataTable();
                tempdt.Load((SqlDataReader)coms[i].ExecuteReader());
                DGs[i].ItemsSource = tempdt.DefaultView;
            }

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.Show();
            Close();
        }
        

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Excel_doc();
        }

        public void Excel_doc()
        {
            Excel.Application e = new Excel.Application();
            e.Visible = false;
            e.SheetsInNewWorkbook = 2;
            Excel.Workbook workBook = e.Workbooks.Add(Type.Missing);
            e.DisplayAlerts = false;

            Excel.Worksheet sheet = (Excel.Worksheet)e.Worksheets.get_Item(1);
            sheet.Name = "Таблица посещений";

            Excel.Range r1 = (Excel.Range)sheet.Range[sheet.Cells[1, 1], sheet.Cells[6, 2]];
            r1.EntireColumn.AutoFit();

            r1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
            r1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
            r1.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
            r1.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
            r1.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

            r1.Cells.Font.Size = 12;
            r1.Cells[1, 1] = "Дата";

            r1.Cells[2, 1] = "Время";

            r1.Cells[3, 1] = "Имя";

            r1.Cells[4, 1] = "Фамилия";

            r1.Cells[5, 1] = "Отчество";

            r1.Cells[6, 1] = "Специалист";


            r1.Cells[1, 2] = "Значение параметра";

            r1.Cells[2, 2] = DGVisit.Items[1];

            r1.Cells[3, 2] = DGVisit.Items[2];

            r1.Cells[4, 2] = DGVisit.Items[3];

            r1.Cells[5, 2] = DGVisit.Items[4];

            r1.Cells[6, 2] = DGVisit.Items[5];


            e.Visible = true;
        }


    }
}