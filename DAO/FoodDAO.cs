using System.Collections.Generic;
using System.Data;
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
    }
}
