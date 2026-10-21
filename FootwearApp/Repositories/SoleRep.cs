using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class SoleRep
    {
        DB db = DB.getInstance();

        public SoleTable GetbyID(int id)
        {
            foreach (SoleTable k in GetAll())
            {
                if (k.id == id)
                    return k;
            }
            return null;
        }

        public IEnumerable<SoleTable> GetAll()
        {
            List<SoleTable> list = new List<SoleTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM sole"))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            list.Add(new SoleTable(
                                reader.GetInt32("id"),
                                reader.GetString("name")
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

        public bool Add(SoleTable entity)
        {
            try
            {
                return db.execSql($"INSERT INTO sole (name) VALUES ('{entity.nazva}')");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete(SoleTable entity)
        {
            try
            {
                return db.execSql($"DELETE FROM sole WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update(SoleTable entity)
        {
            try
            {
                return db.execSql($"UPDATE sole SET name = '{entity.nazva}' WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}