namespace FootwearApp.Tables
{
    public class StyleTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public StyleTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}