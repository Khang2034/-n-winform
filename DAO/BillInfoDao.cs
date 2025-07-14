using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    class BillInfoDao
    {
        private static BillInfoDao instance;
        public static BillInfoDao Instance
        {
            get { if (instance == null) instance = new BillInfoDao(); return BillInfoDao.instance; }
            private set { BillInfoDao.instance = value; }
        }
        private BillInfoDao() { }
        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill = " + id);
            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }
            return listBillInfo;
        }

        public void DeleteBillInfoByFoodId(int id)
        {
            DataProvider.Instance.ExecuteNonQuery("DELETE dbo.BillInfo WHERE idFood = " + id);
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
    }
}
