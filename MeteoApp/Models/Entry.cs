using SQLite;

namespace MeteoApp
{
    public class Entry
    {
        [PrimaryKey]
        [AutoIncrement]
        [NotNull]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}