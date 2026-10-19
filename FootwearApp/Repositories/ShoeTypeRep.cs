using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class ShoeTypeRep : IRepository<ShoeTypeTable>
    {
        public ShoeTypeTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(ShoeTypeTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `shoe_type`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `shoe_type` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(ShoeTypeTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `shoe_type` WHERE id = {entity.id}");
        }

        public bool Update(ShoeTypeTable entity) => throw new NotImplementedException();

        public IEnumerable<ShoeTypeTable> GetAll()
        {
            var list = new List<ShoeTypeTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `shoe_type`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new ShoeTypeTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}