using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class FootwearCatalogRep
    {
        DB db = DB.getInstance();

        public FootwearCatalogTable GetbyID(int id)
        {
            foreach (FootwearCatalogTable k in GetAll())
            {
                if (k.id == id)
                    return k;
            }
            return null;
        }

        public IEnumerable<FootwearCatalogTable> GetAll()
        {
            List<FootwearCatalogTable> list = new List<FootwearCatalogTable>();
            try
            {
                using (var reader = db.execSelect("SELECT * FROM footwear_catalog"))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            list.Add(new FootwearCatalogTable(
                                reader.GetInt32("id"),
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
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return list;
        }

        public bool Add(FootwearCatalogTable entity)
        {
            try
            {
                return db.execSql($@"INSERT INTO footwear_catalog 
                    (name, id_shoe_type, id_audience, id_brand, id_upper_material, id_lining_material, id_sole_material, color, sizes, season, price, id_category, id_country, id_sole_type, shaft_height, width_fit, fastener, has_membrane, id_style, collection, warranty_months) 
                    VALUES 
                    ('{entity.nazva}', {entity.id_shoe_type}, {entity.id_audience}, {entity.id_brand}, {entity.id_upper_material}, {entity.id_lining_material}, {entity.id_sole_material}, '{entity.color}', '{entity.sizes}', '{entity.season}', {entity.price}, {entity.id_category}, {entity.id_country}, {entity.id_sole_type}, '{entity.shaft_height}', '{entity.width_fit}', '{entity.fastener}', {entity.has_membrane}, {entity.id_style}, '{entity.collection}', {entity.warranty_months})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete(FootwearCatalogTable entity)
        {
            try
            {
                return db.execSql($"DELETE FROM footwear_catalog WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update(FootwearCatalogTable entity)
        {
            try
            {
                return db.execSql($@"UPDATE footwear_catalog SET 
                    name = '{entity.nazva}', 
                    id_shoe_type = {entity.id_shoe_type}, 
                    id_audience = {entity.id_audience}, 
                    id_brand = {entity.id_brand}, 
                    id_upper_material = {entity.id_upper_material}, 
                    id_lining_material = {entity.id_lining_material}, 
                    id_sole_material = {entity.id_sole_material}, 
                    color = '{entity.color}', 
                    sizes = '{entity.sizes}', 
                    season = '{entity.season}', 
                    price = {entity.price}, 
                    id_category = {entity.id_category}, 
                    id_country = {entity.id_country}, 
                    id_sole_type = {entity.id_sole_type}, 
                    shaft_height = '{entity.shaft_height}', 
                    width_fit = '{entity.width_fit}', 
                    fastener = '{entity.fastener}', 
                    has_membrane = {entity.has_membrane}, 
                    id_style = {entity.id_style}, 
                    collection = '{entity.collection}', 
                    warranty_months = {entity.warranty_months} 
                    WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}