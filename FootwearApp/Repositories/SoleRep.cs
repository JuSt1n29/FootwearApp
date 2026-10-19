using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class SoleRep : IRepository<SoleTable>
    {
        public SoleTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(SoleTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `sole`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `sole` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(SoleTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `sole` WHERE id = {entity.id}");
        }

        public bool Update(SoleTable entity) => throw new NotImplementedException();

        public IEnumerable<SoleTable> GetAll()
        {
            var list = new List<SoleTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `sole`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new SoleTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}