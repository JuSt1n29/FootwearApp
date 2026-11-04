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
                if (k.id == id) return k;
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
                                reader.GetInt32("article"),
                                reader.GetString("name"),
                                reader.GetInt32("id_brand"),
                                reader.GetInt32("id_category"),
                                reader.GetInt32("id_lining_material"),
                                reader.GetInt32("id_shoe_type"),
                                reader.GetInt32("id_sole"),
                                reader.GetInt32("id_style"),
                                reader.GetString("article"),
                                reader.GetString("season"),
                                reader.GetString("gender"),
                                reader.GetDecimal("price"),
                                reader.GetInt32("discount"),
                                reader.GetInt32("rating"),
                                reader.GetInt32("id_audience"),
                                reader.GetString("description"),
                                reader.GetString("image_url"),
                                reader.GetString("status"),
                                reader.GetInt32("views_count"),
                                reader.GetInt32("stock_quantity"),
                                reader.GetString("color"),
                                reader.GetInt32("warranty_months")
                            ));
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(FootwearCatalogTable entity)
        {
            try { return db.execSql($"INSERT INTO footwearcatalog (id, nazva, price, color) VALUES ({entity.id}, '{entity.nazva}', {entity.price}, '{entity.color}')"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(FootwearCatalogTable entity)
        {
            try { return db.execSql($"DELETE FROM footwearcatalog WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(FootwearCatalogTable entity)
        {
            try { return db.execSql($"UPDATE footwearcatalog SET nazva = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}