namespace FootwearApp.Tables
{
    public class ShoeTypeTable
    {
        public int id { get; set; }
        public string nazva { get; set; }

        public ShoeTypeTable(int id, string nazva)
        {
            this.id = id;
            this.nazva = nazva;
        }
    }
}