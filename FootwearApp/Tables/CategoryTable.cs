namespace FootwearApp.Tables
{
    public class CategoryTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public CategoryTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}