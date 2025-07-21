using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;
using static WindowsFormsApp1.AccountProfile;

namespace WindowsFormsApp1
{
    public partial class TableManager : Form
    {
        private Account _loginAccount;

        public Account LoginAccount
        {
            get { return _loginAccount; }
            set { _loginAccount = value; ChangeAccount(_loginAccount.Type); }
        }

        public TableManager(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc; // gọi setter → tự đổi _loginAccount và chạy ChangeAccount


            LoadTable();
            LoadCategory();
            LoadComboBoxTable(cbSwitchTable);
        }

        #region Method

        private Image GetFoodImageFromResource(string resourceName)
        {
            if (string.IsNullOrWhiteSpace(resourceName))
                return null;

            var prop = typeof(Properties.Resources).GetProperty(resourceName, BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (prop == null)
            {
                MessageBox.Show($"Không tìm thấy resource: {resourceName}");
                return null;
            }

            var value = prop.GetValue(null);

            if (value is byte[] imageBytes)
            {
                using (var ms = new MemoryStream(imageBytes))
                {
                    return Image.FromStream(ms);
                }
            }

            if (value is Image img)
            {
                return img;
            }

            MessageBox.Show($"Resource '{resourceName}' không phải là Image hoặc byte[]");
            return null;
        }




        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1; // Chỉ hiển thị Admin nếu là tài khoản Admin
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetListFoodByCategory(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();
            foreach (Table item in tableList)
            {
                Button btn = new Button()
                {
                    Width = TableDAO.TableWidth,
                    Height = TableDAO.TableHeight,
                    Text = item.Name + Environment.NewLine + item.Status,
                    Tag = item,
                    TextAlign = ContentAlignment.BottomCenter,
                    TextImageRelation = TextImageRelation.TextAboveImage,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.Black
                };
                btn.Click += btn_Click;
                if (item.Status == "Trống")
                {
                    btn.BackColor = Color.AliceBlue;
                    btn.BackgroundImage = Properties.Resources.emptyTable;
                }
                else
                {
                    btn.BackColor = Color.MistyRose;
                    btn.BackgroundImage = Properties.Resources.fillTable;
                }
                btn.BackgroundImageLayout = ImageLayout.Zoom;
                flpTable.Controls.Add(btn);
            }
        }

        void showBill(int id)
        {
            lsvBill.Items.Clear();
            List<WindowsFormsApp1.DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (WindowsFormsApp1.DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());

                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;
                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN");// chọn culture tương ứng của bản thân

            //Thread.CurrentThread.CurrentCulture = culture; // đặt culture cho thread hiện tại

            txbTotalPrice.Text = totalPrice.ToString("c", culture);
        }
        void LoadComboBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        #endregion
        #region Events

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this,new EventArgs());
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as Table).ID;

            int id2 = (cbSwitchTable.SelectedItem as Table).ID;
            if (MessageBox.Show(string.Format("Bạn có chắc muốn chuyển {0} sang {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name), "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTableStatus(id1, id2);
            }
            LoadTable();
        }

        private void cbFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFood.SelectedItem is DTO.Food selectedFood)
            {
                lblDescription.Text = selectedFood.Description ?? "Không có mô tả";
                lblPrice.Text = selectedFood.Price.ToString("c0", new CultureInfo("vi-VN"));

                pbxFood.Image = GetFoodImageFromResource(selectedFood.ResourceName);
            }
        }

        private void btnRemoveSelectedFood_Click(object sender, EventArgs e)
        {
            if (lsvBill.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn món trong hóa đơn để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Table table = lsvBill.Tag as Table;
            if (table == null) return;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            if (idBill == -1) return;

            ListViewItem selectedItem = lsvBill.SelectedItems[0];
            string foodName = selectedItem.Text;

            Category selectedCategory = cbCategory.SelectedItem as Category;
            if (selectedCategory == null) return;

            DTO.Food foodToRemove = FoodDAO.Instance
                .GetListFoodByCategory(selectedCategory.Id)
                .FirstOrDefault(f => f.Name == foodName);

            if (foodToRemove == null) return;

            int foodID = foodToRemove.Id;
            int countToRemove = int.Parse(selectedItem.SubItems[1].Text);

            // Xác nhận trước khi xóa
            if (MessageBox.Show($"Bạn có chắc muốn xóa {foodName} khỏi hóa đơn không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            BillInfoDao.Instance.InsertBillInfo(idBill, foodID, -countToRemove);

            showBill(table.ID);
            LoadTable();
        }

        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvBill.Tag = (sender as Button).Tag; // Lưu thông tin bàn vào lsvBill.Tag
            showBill(tableID);
        }
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile(LoginAccount);
            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }
        void f_UpdateAccount(object sender, AccountEvent e)
        {
            LoginAccount = e.Acc; // cập nhật lại thông tin đăng nhập (bao gồm DisplayName mới)
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }


        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();

            f.loginAccount = LoginAccount; // truyền thông tin tài khoản đăng nhập vào form Admin
            f.InsertFood += f_InsertFood;
            f.DeleteFood += f_DeleteFood;
            f.UpdateFood += f_UpdateFood;

            f.ShowDialog();
        }

        void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag == null)
                showBill((lsvBill.Tag as Table).ID);
        }

        void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag == null)
                showBill((lsvBill.Tag as Table).ID);
            LoadTable();
        }

        void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag == null)
                showBill((lsvBill.Tag as Table).ID);
        }


        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.Id;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbFood.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn món ăn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.ID);
                int newIdBill = BillDAO.Instance.GetMaxIDBill();
                BillInfoDao.Instance.InsertBillInfo(newIdBill, foodID, count);
            }
            else
            {
                BillInfoDao.Instance.InsertBillInfo(idBill, foodID, count);
            }

            showBill(table.ID);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int discount = (int)nmDisCount.Value;

            double totalPrice = double.Parse(txbTotalPrice.Text, NumberStyles.Currency, new CultureInfo("vi-VN"));
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc muốn thanh toán hóa đơn cho {0}\nTổng tiền - (Tổng tiền / 100 ) x Giảm giá\n=> {1} - ({1} / 100) x {2} = {3}", table.Name, totalPrice, discount, finalTotalPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);
                    showBill(table.ID);

                    LoadTable();
                }
            }

        }

        #endregion

    }
}
