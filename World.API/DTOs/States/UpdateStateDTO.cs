namespace World.API.DTOs.States
{
    public class UpdateStateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Population { get; set; }
        public int CountryId { get; set; }
    }
}
