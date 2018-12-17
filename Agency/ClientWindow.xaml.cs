using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from clients", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();
            connection.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddClient window = new AddClient();
            window.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems[0] != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from clients where id={row["id"]}", connection);
                    command.ExecuteNonQuery();
                }
            }
            Refresh();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.Items.Count > 0)
            {
                try
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    xname.Text = row[1] as string;
                    xlastname.Text = row[2] as string;
                    xmiddlename.Text = row[3] as string;
                    xphone.Text = row[4] as string;
                    xemail.Text = row[5] as string;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            object uname = xname.Text;
            if (string.IsNullOrWhiteSpace((string)uname))
            {
                uname = DBNull.Value;
            }
            object ulastname = xlastname.Text;
            if (string.IsNullOrWhiteSpace((string)ulastname))
            {
                ulastname = DBNull.Value;
            }
            object umiddlename = xmiddlename.Text;
            if (string.IsNullOrWhiteSpace((string)umiddlename))
            {
                umiddlename = DBNull.Value;
            }
            object uemail = xemail.Text;
            object uphone = xphone.Text;

            if ((!string.IsNullOrWhiteSpace((string)uemail) && string.IsNullOrWhiteSpace((string)uphone)) || (string.IsNullOrWhiteSpace((string)uemail) && !string.IsNullOrWhiteSpace((string)uphone))
                || (!string.IsNullOrWhiteSpace((string)uemail) && !string.IsNullOrWhiteSpace((string)uphone)))
            {
                if (string.IsNullOrWhiteSpace((string)uemail))
                {
                    uemail = DBNull.Value;
                }
                if (string.IsNullOrWhiteSpace((string)uphone))
                {
                    uphone = DBNull.Value;
                }
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    int id = Convert.ToInt32(row[0]);
                    connection.Open();
                    SqlCommand sql = new SqlCommand($"update clients set name=@name, lastname=@lastname, middlename=@middlename, phone=@phone, email=@email where id={id}", connection);
                    sql.Parameters.Add(new SqlParameter("name", uname));
                    sql.Parameters.Add(new SqlParameter("lastname", ulastname));
                    sql.Parameters.Add(new SqlParameter("middlename", umiddlename));
                    sql.Parameters.Add(new SqlParameter("phone", uphone));
                    sql.Parameters.Add(new SqlParameter("email", uemail));
                    sql.ExecuteNonQuery();
                    MessageBox.Show("Запись изменена!");
                }
            }
            else
            {
                MessageBox.Show("Номер телефона или Email должны быть заполнены!");
            }
            Refresh();
        }
    }
}
