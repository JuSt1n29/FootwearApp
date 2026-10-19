using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class StyleRep : IRepository<StyleTable>
    {
        public StyleTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(StyleTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `style`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `style` (id, name) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}')";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(StyleTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `style` WHERE id = {entity.id}");
        }

        public bool Update(StyleTable entity) => throw new NotImplementedException();

        public IEnumerable<StyleTable> GetAll()
        {
            var list = new List<StyleTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name FROM `style`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new StyleTable(reader.GetInt32("id"), reader.GetString("name")));
                }
                reader.Close();
            }
            return list;
        }
    }
}