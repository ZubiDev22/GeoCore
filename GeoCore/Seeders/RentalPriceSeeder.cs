namespace GeoCore.Seeders
{
    public static class RentalPriceSeeder
    {
        // Diccionario: ciudad -> precio medio de alquiler mensual (en euros)
        public static readonly Dictionary<string, decimal> AverageRentalPricesByCity = new()
        {
            { "Madrid", 1200m },
            { "Barcelona", 1100m },
            { "Valencia", 900m },
            { "Sevilla", 800m },
            { "Bilbao", 950m },
            { "Granada", 700m },
            { "Zaragoza", 850m },
            { "Malaga", 950m },
            { "Alicante", 800m },
            { "Pamplona", 750m }
        };
    }
}
