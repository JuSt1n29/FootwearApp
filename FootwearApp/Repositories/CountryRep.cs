using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class CountryRep : IRepository<CountryTable>
    {
        public CountryTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(CountryTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `country`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `country` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(CountryTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `country` WHERE id = {entity.id}");
        }

        public bool Update(CountryTable entity) => throw new NotImplementedException();

        public IEnumerable<CountryTable> GetAll()
        {
            var list = new List<CountryTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `country`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new CountryTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}