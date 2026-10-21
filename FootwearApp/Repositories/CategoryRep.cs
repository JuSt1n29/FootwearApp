using System;
using System.Collections.Generic;
using FootwearApp.Tables;

namespace FootwearApp.Repositories
{
    internal class CategoryRep
    {
        DB db = DB.getInstance();

        public CategoryTable GetbyID(int id)
        {
            foreach (CategoryTable k in GetAll())
            {
                if (k.id == id)
                    return k;
            }
            return null;
        }

        public IEnumerable<CategoryTable> GetAll()
        {
            List<CategoryTable> list = new List<CategoryTable>();
            try
            {
                using (var reader = db.execSelect("SELECT id, name FROM category"))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            list.Add(new CategoryTable(
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

        public bool Add(CategoryTable entity)
        {
            try
            {
                string cleanName = entity.nazva?.Trim().ToLower() ?? "";
                return db.execSql($"INSERT INTO category (name) VALUES ('{cleanName}')");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // РОБОЧЕ ВИДАЛЕННЯ БЕЗ ID (Через LOWER, щоб точно знайшло і видалило)
        public bool Delete(CategoryTable entity)
        {
            try
            {
                string cleanName = entity.nazva?.Trim().ToLower() ?? "";
                
                // Виводимо в консоль для дебагу, щоб ти бачив що саме відправляється в базу
                Console.WriteLine($"[DEBUG REPO] Видалення з БД запису з ім'ям: '{cleanName}'");

                // Використовуємо LOWER(name), щоб база знайшла рядок навіть якщо він там був з великої літери
                return db.execSql($"DELETE FROM category WHERE LOWER(name) = '{cleanName}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR REPO DELETE] {e.Message}");
                return false;
            }
        }

        public bool Update(CategoryTable entity)
        {
            try
            {
                string cleanName = entity.nazva?.Trim().ToLower() ?? "";
                return db.execSql($"UPDATE category SET name = '{cleanName}' WHERE id = {entity.id}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}