using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class AudienceRep : IRepository<AudienceTable>
    {
        public AudienceTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(AudienceTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `audience`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `audience` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(AudienceTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `audience` WHERE id = {entity.id}");
        }

        public bool Update(AudienceTable entity) => throw new NotImplementedException();

        public IEnumerable<AudienceTable> GetAll()
        {
            var list = new List<AudienceTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `audience`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new AudienceTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}