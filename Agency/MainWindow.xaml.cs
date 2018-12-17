using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Agency
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow window = new ClientWindow();
            window.Show();
        }

        private void Agents_Click(object sender, RoutedEventArgs e)
        {
            AgentWindow window = new AgentWindow();
            window.Show();
        }
    }
}
