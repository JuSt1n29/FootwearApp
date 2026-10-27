using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class MaterialRep
    {
        DB db = DB.getInstance();

        public MaterialTable GetbyID(int id)
        {
            foreach (MaterialTable k in GetAll())
                if (k.id == id) return k;
            return null;
        }

        public IEnumerable<MaterialTable> GetAll()
        {
            List<MaterialTable> list = new List<MaterialTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM material"))
                {
                    if (reader != null)
                        while (reader.Read())
                            list.Add(new MaterialTable(reader.GetInt32("id"), reader.GetString("name")));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(MaterialTable entity)
        {
            try { return db.execSql($"INSERT INTO material (id, name) VALUES ({entity.id}, '{entity.nazva}')"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(MaterialTable entity)
        {
            try { return db.execSql($"DELETE FROM material WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(MaterialTable entity)
        {
            try { return db.execSql($"UPDATE material SET name = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}   