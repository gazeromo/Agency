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
    /// Interaction logic for BuildingWindow.xaml
    /// </summary>
    public partial class BuildingWindow : Window
    {
        public BuildingWindow()
        {
            InitializeComponent();

            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();

            SqlCommand sql = new SqlCommand("select distinct type from buildings", connection);
            using (SqlDataReader reader = sql.ExecuteReader())
            {
                while (reader.Read())
                {
                    typeSelector.Items.Add(reader[0] as string);
                }
            }
            SqlCommand sql1 = new SqlCommand("select distinct city from buildings", connection);
            using (SqlDataReader reader = sql1.ExecuteReader())
            {
                while (reader.Read())
                {
                    citySelector.Items.Add(reader[0] as string);
                }
            }
            SqlCommand sql2 = new SqlCommand("select distinct street from buildings", connection);
            using (SqlDataReader reader = sql2.ExecuteReader())
            {
                while (reader.Read())
                {
                    streetSelector.Items.Add(reader[0] as string);
                }
            }
            connection.Close();
            typeSelector.Items.Add("*");
            citySelector.Items.Add("*");
            streetSelector.Items.Add("*");
            typeSelector.SelectedIndex = 0;
            citySelector.SelectedIndex = 0;
            streetSelector.SelectedIndex = 0;

            updTypeSelector.Items.Add("Дом");
            updTypeSelector.Items.Add("Квартира");
            updTypeSelector.Items.Add("Земля");
            updTypeSelector.SelectedItem = 0;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string type = (string)typeSelector.SelectedItem;
            if(type == "*")
            {
                type = "";
            }
            string city = (string)citySelector.SelectedItem;
            if (city == "*")
            {
                city = "";
            }
            string street = (string)streetSelector.SelectedItem;
            if (street == "*")
            {
                street = "";
            }
            if (type == "Квартира")
            {
                
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings join apartments on buildings.id_apart = apartments.id where " +
                    "type like '%'+@type+'%' and city like '%'+@city+'%' and street like '%'+@street+'%'", connection);
                adapter.SelectCommand.Parameters.AddWithValue("type", type);
                adapter.SelectCommand.Parameters.AddWithValue("city", city);
                adapter.SelectCommand.Parameters.AddWithValue("street", street);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.AsDataView();
            } else if (type == "Дом")
            {
                
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings join houses on buildings.id_house = houses.id where " +
                    "type like '%'+@type+'%' and city like '%'+@city+'%' and street like '%'+@street+'%'", connection);
                adapter.SelectCommand.Parameters.AddWithValue("type", type);
                adapter.SelectCommand.Parameters.AddWithValue("city", city);
                adapter.SelectCommand.Parameters.AddWithValue("street", street);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.AsDataView();
            } else if (type == "Земля")
            {
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings join sites on sites.id = buildings.id_site where " +
                    "type like '%'+@type+'%' and city like '%'+@city+'%' and street like '%'+@street+'%'", connection);
                adapter.SelectCommand.Parameters.AddWithValue("type", type);
                adapter.SelectCommand.Parameters.AddWithValue("city", city);
                adapter.SelectCommand.Parameters.AddWithValue("street", street);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.AsDataView();
            } else
            {
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings where " +
                    "type like '%'+@type+'%' and city like '%'+@city+'%' and street like '%'+@street+'%'", connection);
                adapter.SelectCommand.Parameters.AddWithValue("type", type);
                adapter.SelectCommand.Parameters.AddWithValue("city", city);
                adapter.SelectCommand.Parameters.AddWithValue("street", street);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGrid.ItemsSource = table.AsDataView();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from buildings", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();
            connection.Close();
        }

        private void Delte_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                if((string)row["type"] == "Квартира")
                {
                    SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from apartments where id={row["id_apart"]}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();
                } else if ((string)row["type"] == "Дом")
                {
                    
                    SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from houses where id={row["id_house"]}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                }
                else if ((string)row["type"] == "Земля")
                {

                    SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                    connection.Open();
                    SqlCommand command = new SqlCommand($"delete from sites where id={row["id_site"]}", connection);
                    command.ExecuteNonQuery();
                    connection.Close();

                } 
            }
        }

        private void UpdTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((string)updTypeSelector.SelectedItem == "Квартира")
            {
                apartmentsGrid.Visibility = Visibility.Visible;
                housesGrid.Visibility = Visibility.Collapsed;
                sitesGrid.Visibility = Visibility.Collapsed;
            }
            else if ((string)updTypeSelector.SelectedItem == "Дом")
            {
                apartmentsGrid.Visibility = Visibility.Collapsed;
                housesGrid.Visibility = Visibility.Visible;
                sitesGrid.Visibility = Visibility.Collapsed;
            }
            else if ((string)updTypeSelector.SelectedItem == "Земля")
            {
                apartmentsGrid.Visibility = Visibility.Collapsed;
                housesGrid.Visibility = Visibility.Collapsed;
                sitesGrid.Visibility = Visibility.Visible;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int? value = null;    
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            if ((string)updTypeSelector.SelectedItem == "Квартира") {
                SqlCommand command = new SqlCommand($"insert into apartments values ({Convert.ToInt32(aHouseNumber.Text)}, {Convert.ToInt32(aApartNumber.Text)}, {Convert.ToInt32(aRooms.Text)}); select cast(scope_identity() as int)", connection);
                value = (int?)command.ExecuteScalar();
                SqlCommand sql = new SqlCommand($"insert into buildings values (N'{city.Text}', N'{street.Text}', {Convert.ToInt32(area.Text)}, N'{updTypeSelector.SelectedItem}', {value}, NULL, NULL)", connection);
                sql.ExecuteNonQuery();
            }
            else if ((string)updTypeSelector.SelectedItem == "Дом")
            {
                SqlCommand command = new SqlCommand($"insert into houses values ({Convert.ToInt32(hHouseNumber.Text)}, {Convert.ToInt32(hRooms.Text)}); select cast(scope_identity() as int)", connection);
                value = (int?)command.ExecuteScalar();
                SqlCommand sql = new SqlCommand($"insert into buildings values (N'{city.Text}', N'{street.Text}', {Convert.ToInt32(area.Text)}, N'{updTypeSelector.SelectedItem}', NULL, {value}, NULL)", connection);
                sql.ExecuteNonQuery();
            }
            else if ((string)updTypeSelector.SelectedItem == "Земля")
            {
                SqlCommand command = new SqlCommand($"insert into sites values ({Convert.ToDouble(sLat.Text)}, {Convert.ToDouble(sLon.Text)}); select cast(scope_identity() as int)", connection);
                value = (int?)command.ExecuteScalar();
                SqlCommand sql = new SqlCommand($"insert into buildings values (N'{city.Text}', N'{street.Text}', {Convert.ToInt32(area.Text)}, N'{updTypeSelector.SelectedItem}', NULL, NULL, {value})", connection);
                sql.ExecuteNonQuery();
            }

        }
    }
}
