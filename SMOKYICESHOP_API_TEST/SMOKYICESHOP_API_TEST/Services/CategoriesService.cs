using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Services
{
    public class CategoriesService
    {
        public Category GetCartrigeCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "CartrigesAndVaporizers");
        }

        public Category GetCoalCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "Coals");
        }

        public Category GetEcigaretteCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "ECigarettes");
        }

        public Category GetTobaccoCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "HookahTobacco");
        }

        public Category GetLiquidCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "Liquids");
        }

        public Category GetPodCategory(SmokyIceDbContext dbcontext)
        {
            return dbcontext.Categories.First(x => x.Name == "Pods");
        }
    }
}
