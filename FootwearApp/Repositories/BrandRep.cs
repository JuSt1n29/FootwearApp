using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class BrandRep
    {
        DB db = DB.getInstance();

        public BrandTable GetbyID(int id)
        {
            foreach (BrandTable k in GetAll())
                if (k.id == id) return k;
            return null;
        }

        public IEnumerable<BrandTable> GetAll()
        {
            List<BrandTable> list = new List<BrandTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name, id_country FROM brand"))
                {
                    if (reader != null)
                        while (reader.Read())
                            list.Add(new BrandTable(
                                reader.GetInt32("id"),
                                reader.GetString("name"),
                                reader.GetInt32("id_country")
                            ));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(BrandTable entity)
        {
            try { return db.execSql($"INSERT INTO brand (id, nazva, id_country) VALUES ({entity.id}, '{entity.nazva}', {entity.id_country})"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(BrandTable entity)
        {
            try { return db.execSql($"DELETE FROM brand WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(BrandTable entity)
        {
            try { return db.execSql($"UPDATE brand SET nazva = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}