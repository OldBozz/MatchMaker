namespace MatchMakerAPI.Models
{
    public record Player
    {
        public long Id { get; }
        public string? Name { get; set; }
        public DateOnly Dob { get; set; }
    }
}
