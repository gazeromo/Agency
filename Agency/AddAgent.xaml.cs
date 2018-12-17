using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace Agency
{
    /// <summary>
    /// Interaction logic for AddAgent.xaml
    /// </summary>
    public partial class AddAgent : Window
    {
        public AddAgent()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string uname = name.Text;
            string ulastname = lastname.Text;
            string umiddlename = middlename.Text;
            object ushare = share.Text;
            
            if (Regex.IsMatch(uname, "^[а-яА-Я]+$") && Regex.IsMatch(ulastname, "^[а-яА-Я]+$") && Regex.IsMatch(umiddlename, "^[а-яА-Я]+$")) {
                if(string.IsNullOrWhiteSpace((string)ushare) || !Regex.IsMatch((string)ushare, "^[0-9]+$"))
                {
                    ushare = DBNull.Value;
                }
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    connection.Open();
                    SqlCommand sql = new SqlCommand("insert into agents values (@name, @lastname, @middlename, @share)", connection);
                    sql.Parameters.Add(new SqlParameter("name", uname));
                    sql.Parameters.Add(new SqlParameter("lastname", ulastname));
                    sql.Parameters.Add(new SqlParameter("middlename", umiddlename));
                    sql.Parameters.Add(new SqlParameter("share", ushare));
                    sql.ExecuteNonQuery();
                    MessageBox.Show("Запись добавлена!");
                }
            } else
            {
                MessageBox.Show("ФИО не введено или не корректно!");
            }
        }
    }
}
