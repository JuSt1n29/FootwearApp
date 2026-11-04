namespace FootwearApp.Tables
{
    public class FootwearCatalogTable
    {
        public int id { get; set; } // Мапиться на article
        public string nazva { get; set; }
        public int id_shoe_type { get; set; }
        public int id_audience { get; set; }
        public int id_brand { get; set; }
        public int id_upper_material { get; set; }
        public int id_lining_material { get; set; }
        public int id_sole_material { get; set; }
        public string color { get; set; }
        public string sizes { get; set; }
        public string season { get; set; }
        public decimal price { get; set; }
        public int id_category { get; set; }
        public int id_country { get; set; }
        public int id_sole_type { get; set; }
        public string shaft_height { get; set; }
        public string width_fit { get; set; }
        public string fastener { get; set; }
        public int has_membrane { get; set; }
        public int id_style { get; set; }
        public string collection { get; set; }
        public int warranty_months { get; set; }

        public FootwearCatalogTable(
            int id, string nazva, int id_shoe_type, int id_audience, int id_brand,
            int id_upper_material, int id_lining_material, int id_sole_material, string color,
            string sizes, string season, decimal price, int id_category, int id_country,
            int id_sole_type, string shaft_height, string width_fit, string fastener,
            int has_membrane, int id_style, string collection, int warranty_months)
        {
            this.id = id;
            this.nazva = nazva;
            this.id_shoe_type = id_shoe_type;
            this.id_audience = id_audience;
            this.id_brand = id_brand;
            this.id_upper_material = id_upper_material;
            this.id_lining_material = id_lining_material;
            this.id_sole_material = id_sole_material;
            this.color = color;
            this.sizes = sizes;
            this.season = season;
            this.price = price;
            this.id_category = id_category;
            this.id_country = id_country;
            this.id_sole_type = id_sole_type;
            this.shaft_height = shaft_height;
            this.width_fit = width_fit;
            this.fastener = fastener;
            this.has_membrane = has_membrane;
            this.id_style = id_style;
            this.collection = collection;
            this.warranty_months = warranty_months;
        }
    }
}