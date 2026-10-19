using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class CategoryRep : IRepository<CategoryTable>
    {
        public CategoryTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(CategoryTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `category`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `category` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(CategoryTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `category` WHERE id = {entity.id}");
        }

        public bool Update(CategoryTable entity) => throw new NotImplementedException();

        public IEnumerable<CategoryTable> GetAll()
        {
            var list = new List<CategoryTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `category`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new CategoryTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}