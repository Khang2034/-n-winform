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
            Id = Convert.ToInt32(row["id"]);
            Name = row["name"].ToString();
            Price = Convert.ToSingle(row["price"]);
            CategoryId = Convert.ToInt32(row["idCategory"]);

            Description = row.Table.Columns.Contains("description") && row["description"] != DBNull.Value
                ? row["description"].ToString()
                : "";

            ResourceName = row.Table.Columns.Contains("resourcename") && row["resourcename"] != DBNull.Value
                ? row["resourcename"].ToString()
                : "";

            Image = row.Table.Columns.Contains("image") && row["image"] != DBNull.Value
                ? (byte[])row["image"]
                : null;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string ResourceName { get; set; }
        public byte[] Image { get; set; }
    }
}
