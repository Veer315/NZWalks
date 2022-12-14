namespace NZWalks.API.Models.DTO
{
    internal class Region
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
        public IEnumerable<Domain.Region> Regions { get; internal set; }
    }
}