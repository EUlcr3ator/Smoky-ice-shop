namespace SMOKYICESHOP_API_TEST.DTO.GoodGroups
{
    public class GroupsCollection
    {
        public short MaxPrice { get; set; }
        public short MinPrice { get; set; }
        public int TotalPages { get; set; }
        public bool IsLastPage { get; set; }

        public IEnumerable<ShortGroupDTO> Groups { get; set; } = null!;
    }
}
