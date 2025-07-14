using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance => instance ?? (instance = new FoodDAO());

        private FoodDAO() { }

        public List<Food> GetListFoodByCategory(int id)
        {
            List<Food> listFood = new List<Food>();
            string query = $"SELECT * FROM dbo.Food WHERE idCategory = {id}";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
                listFood.Add(new Food(row));

            return listFood;
        }

        public List<Food> GetListFood()
        {
            List<Food> listFood = new List<Food>();
            string query = "SELECT * FROM dbo.Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
                listFood.Add(new Food(row));

            return listFood;
        }

        public bool InsertFood(string name, int id, float price)
        {
            string query = string.Format("INSERT dbo.Food (name, idCategory, price) VALUES (N'{0}', {1}, {2})", name, id, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateFood(int idFood, string name, int id, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, id, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;  
        }
        
        public bool DeleteFood(int idFood)
        {
            BillInfoDao.Instance.DeleteBillInfoByFoodId(idFood);
                
            string query = string.Format("DELETE Food where id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        
    }
}
