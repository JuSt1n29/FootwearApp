using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class CountryRep
    {
        DB db = DB.getInstance();

        public CountryTable GetbyID(int id)
        {
            foreach (CountryTable k in GetAll())
            {
                if (k.id == id)
                    return k;
            }
            return null;
        }

        public IEnumerable<CountryTable> GetAll()
        {
            List<CountryTable> list = new List<CountryTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM country"))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            list.Add(new CountryTable(
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

        public bool Add(CountryTable entity)
        {
            try
            {
                return db.execSql($"INSERT INTO country (name) VALUES ('{entity.nazva}')");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Delete(CountryTable entity)
        {
            try
            {
                return db.execSql($"DELETE FROM country WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Update(CountryTable entity)
        {
            try
            {
                return db.execSql($"UPDATE country SET name = '{entity.nazva}' WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}