using System;
using System.Data;

namespace WindowsFormsApp1.DTO
{
    public class Food
    {
        public Food(int id, string name, float price, int categoryId, string description = null, byte[] image = null)
        {
            Id = id;
            Name = name;
            Price = price;
            CategoryId = categoryId;
            Description = description;
            Image = image;
        }

        public Food(DataRow row)
        {
            Id = (int)row["id"];
            Name = row["name"].ToString();
            Price = Convert.ToSingle(row["price"]);
            CategoryId = (int)row["idCategory"];

            if (row.Table.Columns.Contains("description") && row["description"] != DBNull.Value)
                Description = row["description"].ToString();

            if (row.Table.Columns.Contains("image") && row["image"] != DBNull.Value)
                Image = (byte[])row["image"];
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
    }
}
