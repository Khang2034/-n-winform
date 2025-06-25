using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DTO
{
    public class Food
    {
        public Food(int id, string name, float price, int categoryId)
        {
            this.Id = id;
            this.Name = name;
            this.Price = price;
            this.CategoryId = categoryId;
        }
        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name"].ToString();
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.CategoryId = (int)row["idCategory"];
        }

        private float price;

        public float Price
        {
            get { return price; }
            set { price = value; }
        }

        private int categoryId;

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
