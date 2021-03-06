﻿using System;
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
    /// Interaction logic for SupplyWindow.xaml
    /// </summary>
    public partial class SupplyWindow : Window
    {
        public SupplyWindow()
        {
            InitializeComponent();
            Refresh();
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand("select * from agents", connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read()) {
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
            SqlCommand command2 = new SqlCommand("select * from buildings", connection);
            using (SqlDataReader reader = command2.ExecuteReader())
            {
                while (reader.Read())
                {
                    buildingSelector.Items.Add(reader[0].ToString() + " " + reader[1].ToString() + " " + reader[2].ToString());
                }
            }

            connection.Close();
            agentSelector.SelectedIndex = 0;
            clientSelector.SelectedIndex = 0;
            buildingSelector.SelectedIndex = 0;
        }

        private void Refresh()
        {
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from supplies", connection);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGrid.ItemsSource = table.AsDataView();
            connection.Close();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            int agentId = Convert.ToInt32(agentSelector.SelectedValue.ToString().Substring(0, agentSelector.SelectedValue.ToString().IndexOf(" ")));
            int clientId = Convert.ToInt32(clientSelector.SelectedValue.ToString().Substring(0, clientSelector.SelectedValue.ToString().IndexOf(" ")));
            int buildingId = Convert.ToInt32(buildingSelector.SelectedValue.ToString().Substring(0, buildingSelector.SelectedValue.ToString().IndexOf(" ")));
            int pricing = Convert.ToInt32(price.Text);
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"insert into supplies values ({agentId}, {clientId}, {buildingId}, {pricing})", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int updId = Convert.ToInt32(id.Text);
            int agentId = Convert.ToInt32(agentSelector.SelectedValue.ToString().Substring(0, agentSelector.SelectedValue.ToString().IndexOf(" ")));
            int clientId = Convert.ToInt32(clientSelector.SelectedValue.ToString().Substring(0, clientSelector.SelectedValue.ToString().IndexOf(" ")));
            int buildingId = Convert.ToInt32(buildingSelector.SelectedValue.ToString().Substring(0, buildingSelector.SelectedValue.ToString().IndexOf(" ")));
            int pricing = Convert.ToInt32(price.Text);
            SqlConnection connection = new SqlConnection(SqlConnect.Instance);
            connection.Open();
            SqlCommand command = new SqlCommand($"update supplies set agent = {agentId}, client = {clientId}, building = {buildingId}, price = {pricing} where id = {updId}", connection);
            command.ExecuteNonQuery();
            connection.Close();
            Refresh();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid.Items.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid.SelectedItems[0];
                SqlConnection connection = new SqlConnection(SqlConnect.Instance);
                connection.Open();
                SqlCommand command = new SqlCommand($"delete from supplies where id = {Convert.ToInt32(row[0])}", connection);
                command.ExecuteNonQuery();
                connection.Close();
                
            }
            Refresh();
        }
    }
}
