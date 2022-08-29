namespace NZWalks.API.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Double Length { get; set; }

        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation property

        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }

    }
}
