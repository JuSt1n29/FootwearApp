namespace FootwearApp.Tables
{
    internal class FootwearCatalogTable
    {
        public int id; // Мапиться на article
        public string nazva;
        public int id_shoe_type;
        public int id_audience;
        public int id_brand;
        public int id_upper_material;
        public int id_lining_material;
        public int id_sole_material;
        public string color;
        public string sizes;
        public string season;
        public decimal price;
        public int id_category;
        public int id_country;
        public int id_sole_type;
        public string shaft_height;
        public string width_fit;
        public string fastener;
        public int has_membrane;
        public int id_style;
        public string collection;
        public int warranty_months;

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