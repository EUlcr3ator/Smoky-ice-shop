namespace SMOKYICESHOP_API_TEST.Entities
{
    public interface IGroupEntity
    {
        public Guid Id { get; set; }
        public string? Line { get; set; }
        public string Name { get; set; }
        public Guid ImageId { get; set; }
    }
}
