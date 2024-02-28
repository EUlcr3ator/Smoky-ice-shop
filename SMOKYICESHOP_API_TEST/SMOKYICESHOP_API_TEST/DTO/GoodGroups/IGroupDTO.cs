using SMOKYICESHOP_API_TEST.DTO.Info;

namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public interface IGroupDTO
    {
        public Guid GroupId { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public Guid ImageId { get; set; }
        public ProducerDTO Producer { get; set; }
    }
}
