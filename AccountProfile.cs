using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1
{
    public partial class AccountProfile : Form
    {
        private Account _loginAccount;

        public Account LoginAccount
        {
            get { return _loginAccount; }
            set { _loginAccount = value; ChangeAccount(_loginAccount); }
        }

        void ChangeAccount(Account acc)
        {
            txbUsername.Text = acc.UserName;
            txbDisplayName.Text = acc.DisplayName;
        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string newpass = txbNewPass.Text;
            string reenterPass = txbReenterPass.Text;
            string username = txbUsername.Text;

            if (!newpass.Equals(reenterPass))
            {
                MessageBox.Show("Mật khẩu mới không khớp, vui lòng nhập lại", "Thông báo");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(username, displayName, password, newpass))
                {
                    MessageBox.Show("Cập nhật tài khoản thành công", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if(_updateAccount != null)
                    {
                        _updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(username)));
                    }
                }
                else
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private event EventHandler<AccountEvent> _updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { _updateAccount += value; }
            remove { _updateAccount -= value; }
        }

        public AccountProfile(Account acc)
        {
            InitializeComponent();
            LoginAccount = acc;

        }

        private void AccountProfile_Load(object sender, EventArgs e)
        {

        }

        private void txbPassoword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        public class AccountEvent : EventArgs
        {
            private Account acc;
            public Account Acc
            {
                get { return acc; }
                set { acc = value; }
            }

            public AccountEvent(Account acc)
            {
                this.Acc = acc;
            }
        }
    }
}
