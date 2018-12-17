using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
    /// Interaction logic for DealWindow.xaml
    /// </summary>
    public partial class DealWindow : Window
    {
        public DealWindow()
        {
            InitializeComponent();
            Refresh();
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand sql = new SqlCommand("select id from supplies", connection);
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while(reader.Read())
                {
                    supply.Items.Add(reader[0].ToString());
                }
            }
            SqlCommand sql1 = new SqlCommand("select id from demands", connection);
            using (SqlDataReader reader = sql1.ExecuteReader())
            {
                while (reader.Read())
                {
                    demand.Items.Add(reader[0].ToString());
                }
            }
            connection.Close();
            supply.SelectedIndex = 0;
            demand.SelectedIndex = 0;
        }

        private void Refresh()
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from deals", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();

            SqlDataAdapter adapter1 = new SqlDataAdapter("select * from closed_deals", connection);
            DataTable table1 = new DataTable();
            adapter1.Fill(table1);
            dataGrid1.ItemsSource = table1.AsDataView();
            connection.Close();
        }

        

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.Items.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlCommand command = new SqlCommand($"delete from deals where id = {Convert.ToInt32(row[0])}", connection);
                SqlCommand command1 = new SqlCommand($"insert into closed_deals values ({Convert.ToInt32(row[1])}, {Convert.ToInt32(row[2])})", connection);
                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                connection.Close();
            }
            Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int uSupply = Convert.ToInt32(supply.Text);
            int uDemand = Convert.ToInt32(demand.Text);

            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"insert into deals values ({uSupply}, {uDemand})", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int uSupply = Convert.ToInt32(supply.Text);
            int uDemand = Convert.ToInt32(demand.Text);
            int uId = Convert.ToInt32(id.Text);

            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"update deals set supply = {uSupply}, demand = {uDemand} where id = {uId}", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }
    }
}
