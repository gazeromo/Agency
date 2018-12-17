using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agency
{
    class Agent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string middlename { get; set; }
        public int share { get; set; }

        public Agent(int id, string name, string lastname, string middlename, int share)
        {
            this.id = id;
            this.name = name;
            this.lastname = lastname;
            this.middlename = middlename;
            this.share = share;
        }
    }
}
