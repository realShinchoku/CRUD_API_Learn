namespace api.DTOs.CharacterDTO
{
    public class AddCharacterDTO
    {
        public string Name { get; set; } = "Prodo";
        public int HitPoints { get; set; } = 100;
        public int Ammor { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
    }
}