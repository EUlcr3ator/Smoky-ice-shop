namespace SMOKYICESHOP_API_TEST.Entities
{
    public interface IHasGood
    {
        public Guid GoodId { get; set; }
        public Good Good { get; set; }
    }
}
