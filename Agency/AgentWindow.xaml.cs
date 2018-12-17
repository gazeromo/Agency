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
    /// Interaction logic for AgentWindow.xaml
    /// </summary>
    public partial class AgentWindow : Window
    {
        public AgentWindow()
        {
            InitializeComponent();
            Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddAgent window = new AddAgent();
            window.Show();
        }

        private void Refresh()
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from agents", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();
            connection.Close();
        }

        

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems[0] != null)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from agents where id={row["id"]}", connection);
                    command.ExecuteNonQuery();
                }
            }
            Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string uname = xname.Text;
            string ulastname = xlastname.Text;
            string umiddlename = xmiddlename.Text;
            object ushare = xshare.Text;

            if (Regex.IsMatch(uname, "^[а-яА-Я]+$") && Regex.IsMatch(ulastname, "^[а-яА-Я]+$") && Regex.IsMatch(umiddlename, "^[а-яА-Я]+$"))
            {
                if (string.IsNullOrWhiteSpace((string)ushare) || !Regex.IsMatch((string)ushare, "^[0-9]+$"))
                {
                    ushare = DBNull.Value;
                }
                using (SqlConnection connection = new SqlConnection(SqlConnect.Instance))
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    int id = Convert.ToInt32(row[0]);
                    connection.Open();
                    SqlCommand sql = new SqlCommand($"update agents set name=@name, lastname=@lastname, middlename=@middlename, share=@share where id={id}", connection);
                    sql.Parameters.Add(new SqlParameter("name", uname));
                    sql.Parameters.Add(new SqlParameter("lastname", ulastname));
                    sql.Parameters.Add(new SqlParameter("middlename", umiddlename));
                    sql.Parameters.Add(new SqlParameter("share", ushare));
                    sql.ExecuteNonQuery();
                    MessageBox.Show("Запись изменена!");
                }
            }
            else
            {
                MessageBox.Show("ФИО не введено или не корректно!");
            }
            Refresh();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.Items.Count > 0 )
            {
                try
                {
                    DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                    xname.Text = row[1] as string;
                    xlastname.Text = row[2] as string;
                    xmiddlename.Text = row[3] as string;
                    xshare.Text = Convert.ToInt32(row[4]).ToString();
                } catch (Exception err)
                {
                    Console.WriteLine(err.ToString());
                }
            }
            

        }
    }
}
