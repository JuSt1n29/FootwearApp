using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class StyleRep
    {
        DB db = DB.getInstance();

        public StyleTable GetbyID(int id)
        {
            foreach (StyleTable k in GetAll())
                if (k.id == id) return k;
            return null;
        }

        public IEnumerable<StyleTable> GetAll()
        {
            List<StyleTable> list = new List<StyleTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM style"))
                {
                    if (reader != null)
                        while (reader.Read())
                            list.Add(new StyleTable(reader.GetInt32("id"), reader.GetString("name")));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(StyleTable entity)
        {
            try { return db.execSql($"INSERT INTO style (id, name) VALUES ({entity.id}, '{entity.nazva}')"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(StyleTable entity)
        {
            try { return db.execSql($"DELETE FROM style WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(StyleTable entity)
        {
            try { return db.execSql($"UPDATE style SET name = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}