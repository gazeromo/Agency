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
    /// Interaction logic for AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public AddClient()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            object uname = name.Text;
            if(string.IsNullOrWhiteSpace((string)uname))
            {
                uname = DBNull.Value;
            }
            object ulastname = lastname.Text;
            if (string.IsNullOrWhiteSpace((string)ulastname))
            {
                ulastname = DBNull.Value;
            }
            object umiddlename = middlename.Text;
            if (string.IsNullOrWhiteSpace((string)umiddlename))
            {
                umiddlename = DBNull.Value;
            }
            object uemail = email.Text;
            object uphone = phone.Text;

            if ((!string.IsNullOrWhiteSpace((string)uemail) && string.IsNullOrWhiteSpace((string)uphone)) || (string.IsNullOrWhiteSpace((string)uemail) && !string.IsNullOrWhiteSpace((string)uphone)) 
                || (!string.IsNullOrWhiteSpace((string)uemail) && !string.IsNullOrWhiteSpace((string)uphone)))
            {
                if(string.IsNullOrWhiteSpace((string)uemail))
                {
                    uemail = DBNull.Value;
                }
                if (string.IsNullOrWhiteSpace((string)uphone))
                {
                    uphone = DBNull.Value;
                }
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    connection.Open();
                    SqlCommand sql = new SqlCommand("insert into clients values (@name, @lastname, @middlename, @phone, @email)", connection);
                    sql.Parameters.Add(new SqlParameter("name", uname));
                    sql.Parameters.Add(new SqlParameter("lastname", ulastname));
                    sql.Parameters.Add(new SqlParameter("middlename", umiddlename));
                    sql.Parameters.Add(new SqlParameter("phone", uphone));
                    sql.Parameters.Add(new SqlParameter("email", uemail));
                    sql.ExecuteNonQuery();
                    MessageBox.Show("Запись добавлена!");
                }
            }
            else
            {
                MessageBox.Show("Номер телефона или Email должны быть заполнены!");
            }
            
        }
    }
}
