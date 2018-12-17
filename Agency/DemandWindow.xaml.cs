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
    /// Interaction logic for DemandWindow.xaml
    /// </summary>
    public partial class DemandWindow : Window
    {
        public DemandWindow()
        {
            InitializeComponent();
            Refresh();
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand("select * from agents", connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    agentSelector.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                }
            }
            SqlCommand command1 = new SqlCommand("select * from clients", connection);
            using (SqlDataReader reader = command1.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientSelector.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                }
            }

            connection.Close();
            agentSelector.SelectedIndex = 0;
            clientSelector.SelectedIndex = 0;
        }

        private void Refresh()
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from demands", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();
            connection.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlCommand command = new SqlCommand($"delete from demands where id = {Convert.ToInt32(row[0])}", connection);
                command.ExecuteNonQuery();
                connection.Close();

            }
            Refresh();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int agentId = Convert.ToInt32(agentSelector.SelectedValue.ToString().Substring(0, agentSelector.SelectedValue.ToString().IndexOf(" ")));
            int clientId = Convert.ToInt32(clientSelector.SelectedValue.ToString().Substring(0, clientSelector.SelectedValue.ToString().IndexOf(" ")));
            int uMinPrice = Convert.ToInt32(minPrice.Text);
            int uMaxPrice = Convert.ToInt32(maxPrice.Text);
            int uMinArea = Convert.ToInt32(minArea.Text);
            int uMaxArea = Convert.ToInt32(maxArea.Text);
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"insert into demands values ({agentId}, {clientId}, {uMinPrice}, {uMaxPrice}, {uMinArea}, {uMaxArea})", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int agentId = Convert.ToInt32(agentSelector.SelectedValue.ToString().Substring(0, agentSelector.SelectedValue.ToString().IndexOf(" ")));
            int clientId = Convert.ToInt32(clientSelector.SelectedValue.ToString().Substring(0, clientSelector.SelectedValue.ToString().IndexOf(" ")));
            int uMinPrice = Convert.ToInt32(minPrice.Text);
            int uMaxPrice = Convert.ToInt32(maxPrice.Text);
            int uMinArea = Convert.ToInt32(minArea.Text);
            int uMaxArea = Convert.ToInt32(maxArea.Text);
            int uId = Convert.ToInt32(id.Text);
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"update demands set agent = {agentId}, client = {clientId}, min_price = {uMinPrice}, max_price = {uMaxPrice}, min_area = {uMinArea}, max_area = {uMaxArea} where id = {uId}", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }

        private void QuickDeal_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.Items.Count > 0)
            {
                List<int> tIds = new List<int>();
                List<int> buildingIds = new List<int>();
                List<int> prices = new List<int>();
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                int id = Convert.ToInt32(row[0]);
                int minPrice = Convert.ToInt32(row[3]);
                int maxPrice = Convert.ToInt32(row[4]);
                int minArea = Convert.ToInt32(row[5]);
                int maxArea = Convert.ToInt32(row[6]);
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlCommand sql = new SqlCommand("select * from supplies", connection);
                using (SqlDataReader reader = sql.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        int tId = Convert.ToInt32(reader[0]);
                        int buildingId = Convert.ToInt32(reader[3]);
                        int price = Convert.ToInt32(reader[4]);
                        tIds.Add(tId);
                        buildingIds.Add(buildingId);
                        prices.Add(price);
                        
                    }
                }
                
                if (tIds.Count > 0)
                {
                    int tId = tIds[0];
                    int buildingId = buildingIds[0];
                    int price = prices[0];
                    int area = 0;
                    SqlCommand sql1 = new SqlCommand($"select area from buildings where id = {buildingId}", connection);
                    bool flag = false;
                    using (SqlDataReader reader1 = sql1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            area = Convert.ToInt32(reader1[0]);
                            
                                flag = true;
                           
                        }
                    }
                    if (!flag)
                    {
                        MessageBox.Show("Не удалось найти подходящюю сделку(");
                    } else
                    {
                    if (price >= minPrice && price <= maxPrice && area >= minArea && area <= maxArea)
                    {
                        SqlCommand insertCommand = new SqlCommand($"insert into deals values ({tId}, {id})", connection);
                        insertCommand.ExecuteNonQuery();
                        }
                        MessageBox.Show("Мы подобрали предложение!");
                    }


                } 
                connection.Close();
            }
            
        }
    }
}
