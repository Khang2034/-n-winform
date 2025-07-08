using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1.DTO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string username, string password)
        {
            string query = "USP_Login @userName , @password";


            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { username, password });


            return result.Rows.Count > 0;
        }

        public bool UpdateAccount (string username, string displayName, string pass, string newPass)
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @username, @displayname, @password, @newpassword", new object[] { username, displayName, pass, newPass });
            return result > 0;
        }
        public Account GetAccountByUserName(string username)
        {
            string query = "SELECT * FROM Account WHERE username = @username";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { username });

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }
    }
}
