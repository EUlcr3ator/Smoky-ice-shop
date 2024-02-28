namespace SMOKYICESHOP_API_TEST.DTO.FieldValues
{
    public class IdGroup<T> : ICustomGrouping<T, Guid>
    {
        public T Key { get; set; }
        public IEnumerable<Guid> Ids { get; set; } = null!;
    }
}
