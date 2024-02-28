using SMOKYICESHOP_API_TEST.DTO.FieldValues;

namespace SMOKYICESHOP_API_TEST.DTO.CreateGroups
{
    public class CreateHookahTobaccoGroupDTO
    {
        public byte Sweet { get; set; }
        public byte Sour { get; set; }
        public byte Fresh { get; set; }
        public byte Spicy { get; set; }
        public string Strength { get; set; } = null!;
        public string? Line { get; set; }
        public string Name { get; set; } = null!;

        public Guid ImageId { get; set; }
        public Guid ProducerId { get; set; }
        public string TasteMixName { get; set; }
    }
}
