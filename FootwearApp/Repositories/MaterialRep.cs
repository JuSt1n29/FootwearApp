using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class MaterialRep : IRepository<MaterialTable>
    {
        public MaterialTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(MaterialTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `material`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `material` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(MaterialTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `material` WHERE id = {entity.id}");
        }

        public bool Update(MaterialTable entity) => throw new NotImplementedException();

        public IEnumerable<MaterialTable> GetAll()
        {
            var list = new List<MaterialTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `material`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new MaterialTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}