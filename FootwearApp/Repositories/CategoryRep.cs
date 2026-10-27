using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class CategoryRep
    {
        DB db = DB.getInstance();

        public CategoryTable GetbyID(int id)
        {
            foreach (CategoryTable k in GetAll())
                if (k.id == id) return k;
            return null;
        }

        public IEnumerable<CategoryTable> GetAll()
        {
            List<CategoryTable> list = new List<CategoryTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM category"))
                {
                    if (reader != null)
                        while (reader.Read())
                            list.Add(new CategoryTable(reader.GetInt32("id"), reader.GetString("name")));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(CategoryTable entity)
        {
            try { return db.execSql($"INSERT INTO category (id, name) VALUES ({entity.id}, '{entity.nazva}')"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(CategoryTable entity)
        {
            try { return db.execSql($"DELETE FROM category WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(CategoryTable entity)
        {
            try { return db.execSql($"UPDATE category SET name = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}