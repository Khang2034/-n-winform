using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.DTO;
using System.Globalization; // Thêm thư viện lịch

namespace WindowsFormsApp1
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();

            Load();
        }

        void Load()
        {
            // Thiết lập culture tiếng Việt
            CultureInfo vietnameseCulture = new CultureInfo("vi-VN");
            System.Threading.Thread.CurrentThread.CurrentCulture = vietnameseCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = vietnameseCulture;
            LoadDataTimePickerBill();
            // Thiết lập định dạng ngày tháng cho DateTimePicker
            dtpkFromDate.Format = DateTimePickerFormat.Custom;
            dtpkFromDate.CustomFormat = "dd/MM/yyyy";
            dtpkToDate.Format = DateTimePickerFormat.Custom;
            dtpkToDate.CustomFormat = "dd/MM/yyyy";

            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            LoadListFood();
        }

        #region Methods
        void LoadDataTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadListFood()
        {
            dtgvFood.DataSource = FoodDAO.Instance.GetListFood();
        }
        #endregion

        #region Events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }
        #endregion

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
    }
}