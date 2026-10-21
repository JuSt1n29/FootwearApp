using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class ShoeTypeRep
    {
        DB db = DB.getInstance();

        public ShoeTypeTable GetbyID(int id)
        {
            foreach (ShoeTypeTable k in GetAll())
            {
                if (k.id == id)
                    return k;
            }
            return null;
        }

        public IEnumerable<ShoeTypeTable> GetAll()
        {
            List<ShoeTypeTable> list = new List<ShoeTypeTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM shoe_type"))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            list.Add(new ShoeTypeTable(
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

        public bool Add(ShoeTypeTable entity)
        {
            try
            {
                return db.execSql($"INSERT INTO shoe_type (name) VALUES ('{entity.nazva}')");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete(ShoeTypeTable entity)
        {
            try
            {
                return db.execSql($"DELETE FROM shoe_type WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update(ShoeTypeTable entity)
        {
            try
            {
                return db.execSql($"UPDATE shoe_type SET name = '{entity.nazva}' WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}