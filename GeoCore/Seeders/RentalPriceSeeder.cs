namespace GeoCore.Seeders
{
    public static class RentalPriceSeeder
    {
        // Diccionario: ciudad, zona o código postal -> precio medio de alquiler mensual (en euros)
        public static readonly Dictionary<string, decimal> AverageRentalPricesByCity = new()
        {
            // Ciudades (de los buildings)
            { "Madrid", 1200m },
            { "Barcelona", 1100m },
            { "Sevilla", 800m },
            { "Valencia", 900m },
            { "Bilbao", 950m },
            { "Granada", 700m },
            { "Zaragoza", 850m },
            { "Malaga", 950m },
            { "Alicante", 800m },
            { "Pamplona", 750m },
            // Zonas (de los rentals)
            { "Centro", 1300m },
            { "Eixample", 1200m },
            { "Ruzafa", 950m },
            // Códigos postales (de los rentals)
            { "28001", 1400m },
            { "08009", 1250m },
            { "46004", 1000m }
        };
    }
}
