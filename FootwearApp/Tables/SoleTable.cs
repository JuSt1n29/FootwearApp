namespace FootwearApp.Tables
{
    public class SoleTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public SoleTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}