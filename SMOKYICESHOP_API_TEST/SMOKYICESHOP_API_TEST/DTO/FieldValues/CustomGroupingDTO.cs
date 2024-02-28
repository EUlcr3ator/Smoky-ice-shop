namespace SMOKYICESHOP_API_TEST.DTO.FieldValues
{
    public class CustomGroupingDTO<TKey, TIds> : ICustomGrouping<TKey, TIds>
    {
        public TKey Key { get; set; }
        public IEnumerable<TIds> Ids { get; set; } = null!;
    }
}
