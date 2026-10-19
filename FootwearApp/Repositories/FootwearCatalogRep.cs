using System;
using System.Collections.Generic;
using System.Globalization;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class FootwearCatalogRep : IRepository<FootwearCatalogTable>
    {
        public FootwearCatalogTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(FootwearCatalogTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(article), 0) + 1 FROM `footwear_catalog`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string priceStr = entity.price.ToString(CultureInfo.InvariantCulture);

            string sql = $@"INSERT INTO `footwear_catalog` 
                (article, name, id_shoe_type, id_audience, id_brand, id_upper_material, id_lining_material, id_sole_material, color, sizes, season, price, id_category, id_country, id_sole_type, shaft_height, width_fit, fastener, has_membrane, id_style, collection, warranty_months) 
                VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}', {entity.id_shoe_type}, {entity.id_audience}, {entity.id_brand}, {entity.id_upper_material}, {entity.id_lining_material}, {entity.id_sole_material}, '{entity.color}', '{entity.sizes}', '{entity.season}', {priceStr}, {entity.id_category}, {entity.id_country}, {entity.id_sole_type}, '{entity.shaft_height}', '{entity.width_fit}', '{entity.fastener}', {entity.has_membrane}, {entity.id_style}, '{entity.collection}', {entity.warranty_months})";

            return DB.getInstance().execSql(sql);
        }

        public bool Delete(FootwearCatalogTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `footwear_catalog` WHERE article = {entity.id}");
        }

        public bool Update(FootwearCatalogTable entity) => throw new NotImplementedException();

        public IEnumerable<FootwearCatalogTable> GetAll()
        {
            var list = new List<FootwearCatalogTable>();
            var reader = DB.getInstance().execSelect("SELECT * FROM `footwear_catalog`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new FootwearCatalogTable(
                        reader.GetInt32("article"),
                        reader.GetString("name"),
                        reader.GetInt32("id_shoe_type"),
                        reader.GetInt32("id_audience"),
                        reader.GetInt32("id_brand"),
                        reader.GetInt32("id_upper_material"),
                        reader.GetInt32("id_lining_material"),
                        reader.GetInt32("id_sole_material"),
                        reader.GetString("color"),
                        reader.GetString("sizes"),
                        reader.GetString("season"),
                        reader.GetDecimal("price"),
                        reader.GetInt32("id_category"),
                        reader.GetInt32("id_country"),
                        reader.GetInt32("id_sole_type"),
                        reader.GetString("shaft_height"),
                        reader.GetString("width_fit"),
                        reader.GetString("fastener"),
                        reader.GetInt32("has_membrane"),
                        reader.GetInt32("id_style"),
                        reader.GetString("collection"),
                        reader.GetInt32("warranty_months")
                    ));
                }
                reader.Close();
            }
            return list;
        }
    }
}