using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.DTO.Goods
{
    public class SearchCollection
    {
        public bool IsLastPage { get; set; }
        public int TotalPages { get; set; }

        public string SearchText { get; set; } = null!;
        public IEnumerable<SearchGoodDTO> Goods { get; set; } = null!;
    }
}
