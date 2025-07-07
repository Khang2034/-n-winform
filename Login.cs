using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

            // 1) Parent the sign-up detail panel to the overlay container
            guna2Panel_SignUp.Controls.Add(guna2Panel3);

            // 2) Position the detail panel in the center of the overlay
            guna2Panel3.Location = new Point(
                (guna2Panel_SignUp.Width - guna2Panel3.Width) / 2,
                (guna2Panel_SignUp.Height - guna2Panel3.Height) / 2
            );

            // 3) Initially hide the detail panel
            guna2Panel3.Visible = false;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            guna2ShadowForm1.SetShadowForm(this);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txbUsername.Text;
            var password = txbPassword.Text;

            if (AccountDAO.Instance.Login(username, password))
            {

                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(username);
                using (var f = new TableManager(loginAccount))
                {

                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                }
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn thoát chương trình?", "Thông báo",
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
            // Start the fade-in overlay
            guna2Panel_SignUp.FillColor = Color.FromArgb(0, 62, 93, 52);
            guna2Panel_SignUp.Visible = true;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Increase alpha until desired opacity, then show detail panel
            if (guna2Panel_SignUp.FillColor.A >= 200)
            {
                timer1.Stop();
                guna2Panel3.Visible = true;
                return;
            }

            var newA = Math.Min(200, guna2Panel_SignUp.FillColor.A + 5);
            guna2Panel_SignUp.FillColor = Color.FromArgb(newA, 62, 93, 52);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Hide everything again
            guna2Panel3.Visible = false;
            guna2Panel_SignUp.Visible = false;
        }

        private void guna2PictureBox8_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
