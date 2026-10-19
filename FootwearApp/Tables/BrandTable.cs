namespace FootwearApp.Tables
{
    internal class BrandTable
    {
        public int id;
        public string nazva;
        public int id_country;

        public BrandTable(int id, string nazva, int id_country)
        {
            this.id = id;
            this.nazva = nazva;
            this.id_country = id_country;
        }
    }
}