using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class AudienceRep
    {
        DB db = DB.getInstance();

        public AudienceTable GetbyID(int id)
        {
            foreach (AudienceTable k in GetAll())
                if (k.id == id) return k;
            return null;
        }

        public IEnumerable<AudienceTable> GetAll()
        {
            List<AudienceTable> list = new List<AudienceTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM audience"))
                {
                    if (reader != null)
                        while (reader.Read())
                            list.Add(new AudienceTable(reader.GetInt32("id"), reader.GetString("name")));
                }
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            return list;
        }

        public bool Add(AudienceTable entity)
        {
            try { return db.execSql($"INSERT INTO audience (id, name) VALUES ({entity.id}, '{entity.nazva}')"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Delete(AudienceTable entity)
        {
            try { return db.execSql($"DELETE FROM audience WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool Update(AudienceTable entity)
        {
            try { return db.execSql($"UPDATE audience SET name = '{entity.nazva}' WHERE id = {entity.id}"); }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }
    }
}