using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DTO;

namespace WindowsFormsApp1.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<DTO.Food> GetListFoodByCategory(int id)
        {
            List<DTO.Food> listFood = new List<DTO.Food>();
            string query = "SELECT * FROM dbo.Food WHERE idcategory = " + id;//không biết có cần dbo. không
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                DTO.Food food = new DTO.Food(item);
                listFood.Add(food);
            }
            return listFood;
        }
    }
}
