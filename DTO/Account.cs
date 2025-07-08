using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DTO
{
    public class Account
    {
        public Account(string username, string displayname, int type, string password = null) 
        {
            this.username = username;
            this.displayname = displayname;
            this.type = type;
            this.password = password;
        }

        public Account(DataRow row) 
        {
            this.username = row["username"].ToString();
            this.displayname = row["displayname"].ToString();
            this.type = (int)row["type"];
            this.password = row["password"].ToString();
        }

        private int type;

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string displayname;

        public string DisplayName
        {
            get { return displayname; }
            set { displayname = value; }
        }

        private string username; //có gì thì sửa userName

        public string UserName
        {
            get { return username; }
            set { username = value; }
        }
    }
}
