using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DbContexts;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;
using SMOKYICESHOP_API_TEST.Services;
using System.Linq;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class ProducersModel
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly CategoriesService _categoriesService;

        public ProducersModel(SmokyIceDbContext dbcontext, CategoriesService categoriesService)
        {
            _dbcontext = dbcontext;
            _categoriesService = categoriesService;
        }

        public IEnumerable<ProducerDTO> GetAllProducers()
        {
            return _dbcontext.Producers
                .OrderBy(x => x.Name)
                .ToList()
                .Select(x => CreateProducerCE(x));
        }

        public IEnumerable<ProducerDTO> GetAllProducers(string category)
        {
            return _dbcontext.Categories
                .Include(x => x.Producers)
                .First(x => x.Name == category)
                .Producers
                    .OrderBy(x => x.Name)
                    .Select(x => CreateProducerCE(x))
                    .ToList();
        }

        public void ConfigureProducersCategories()
        {
            IEnumerable<Producer> producers = _dbcontext.Producers
                .Include(x => x.Categories)
                .Include(x => x.CartrigesAndVaporizersGroups)
                .Include(x => x.CoalsGroups)
                .Include(x => x.EcigarettesGroups)
                .Include(x => x.HookahTobaccoGroups)
                .Include(x => x.LiquidsGroups)
                .Include(x => x.PodsGroups)
                .ToList();

            IEnumerable<Category> categories = _dbcontext.Categories
                .ToList();

            foreach (Producer producer in producers)
            {
                producer.Categories.Clear();

                if (producer.CartrigesAndVaporizersGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetCartrigeCategory(_dbcontext));

                if (producer.CoalsGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetCoalCategory(_dbcontext));

                if (producer.EcigarettesGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetEcigaretteCategory(_dbcontext));

                if (producer.HookahTobaccoGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetTobaccoCategory(_dbcontext));

                if (producer.LiquidsGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetLiquidCategory(_dbcontext));

                if (producer.PodsGroups.Count != 0)
                    producer.Categories.Add(_categoriesService.GetPodCategory(_dbcontext));
            }

            _dbcontext.SaveChanges();
        }

        public ProducerDTO GetProducer(Guid producerId)
        {
            Producer producer = _dbcontext.Producers.First(x => x.Id == producerId);
            return CreateProducerCE(producer);
        }

        public Guid AddProducer(CreateProducerDTO producerDto)
        {
            Producer producer = new Producer
            {
                Name = producerDto.Name,
                ImageId = producerDto.ImageId
            };

            _dbcontext.Producers.Add(producer);
            _dbcontext.SaveChanges();
            Guid producerId = producer.Id;
            return producerId;
        }

        private ProducerDTO CreateProducerCE(Producer producer)
        {
            return new ProducerDTO
            {
                Id = producer.Id,
                Name = producer.Name,
                ImageId = producer.ImageId
            };
        }
    }
}
