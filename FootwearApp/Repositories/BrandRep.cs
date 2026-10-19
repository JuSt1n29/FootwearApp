using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class BrandRep : IRepository<BrandTable>
    {
        public BrandTable? GetbyID(int id)
        {
            foreach (var item in GetAll()) if (item.id == id) return item;
            return null;
        }

        public bool Add(BrandTable entity)
        {
            var maxId = DB.getInstance().execScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM `brand`");
            entity.id = maxId != null ? Convert.ToInt32(maxId) : 1;
            string sql = $"INSERT INTO `brand` (id, name, id_country) VALUES ({entity.id}, '{entity.nazva.Replace("'", "''")}', {entity.id_country})";
            return DB.getInstance().execSql(sql);
        }

        public bool Delete(BrandTable entity)
        {
            return DB.getInstance().execSql($"DELETE FROM `brand` WHERE id = {entity.id}");
        }

        public bool Update(BrandTable entity) => throw new NotImplementedException();

        public IEnumerable<BrandTable> GetAll()
        {
            var list = new List<BrandTable>();
            var reader = DB.getInstance().execSelect("SELECT id, name, id_country FROM `brand`");
            if (reader != null)
            {
                while (reader.Read())
                {
                    list.Add(new BrandTable(reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("id_country")));
                }
                reader.Close();
            }
            return list;
        }
    }
}