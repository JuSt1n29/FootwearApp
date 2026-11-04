namespace FootwearApp.Tables
{
    public class AudienceTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public AudienceTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}