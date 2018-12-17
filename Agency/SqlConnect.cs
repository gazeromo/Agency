using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency
{
    public static class SqlConnect
    {
        private static string instance = null;
        public static string Instance
        {
            get
            {
                if(instance == null)
                {
                    return instance = @"Data Source=(LocalDB)\MSSQLLOCALDB;Initial Catalog=agency;Integrated Security=True";
                } else
                {
                    return instance;
                }
            }
        }
        
    }
}
