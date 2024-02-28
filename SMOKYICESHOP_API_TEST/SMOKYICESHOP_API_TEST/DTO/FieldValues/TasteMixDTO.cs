namespace SMOKYICESHOP_API_TEST.DTO.FieldValues
{
    public class TasteMixDTO<T>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<T> Tastes { get; set; } = null!;
    }
}
