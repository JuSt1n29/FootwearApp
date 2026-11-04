namespace FootwearApp.Tables
{
    public class BrandTable
    {
        public int id { get; set; }
        public string nazva { get; set; }
        public int id_country { get; set; }

        public BrandTable(int id, string nazva, int id_country)
        {
            this.id = id;
            this.nazva = nazva;
            this.id_country = id_country;
        }
    }
}