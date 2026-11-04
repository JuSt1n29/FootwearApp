namespace FootwearApp.Tables
{
    public class MaterialTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public MaterialTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}