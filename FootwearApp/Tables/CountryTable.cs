namespace FootwearApp.Tables
{
    public class CountryTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public CountryTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}