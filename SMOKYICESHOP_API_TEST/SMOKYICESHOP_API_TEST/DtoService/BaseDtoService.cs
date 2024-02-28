using SMOKYICESHOP_API_TEST.DTO.CreateGoods;
using SMOKYICESHOP_API_TEST.DTO.FieldValues;
using SMOKYICESHOP_API_TEST.DTO.GoodGroups;
using SMOKYICESHOP_API_TEST.DTO.Goods;
using SMOKYICESHOP_API_TEST.DTO.Info;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.DtoService
{
    public abstract class BaseDtoService
    {

        public void FillGoodDto(IGood dto, Good entity, short? discount)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            dto.Id = entity.Id;
            dto.ImageId = entity.ImageId;
            dto.Name = entity.Name;
            dto.Category = GetCategoryName(entity.CategoryId);
            dto.Price = entity.Price;
            dto.DiscountPrice = discount;
            dto.IsSold = entity.IsSold;
        }

        public IdGroup<T> CreateIdGroup<T, TGoodId>(IGrouping<T, TGoodId> grouping) where TGoodId : IHasGood
        {
            return new IdGroup<T>
            {
                Key = grouping.Key,
                Ids = grouping.Select(x => x.GoodId)
            };
        }

        protected ShortGroupDTO BaseCreateShortGroup(IGroupEntity groupEntity, CategoryEnum categoryEnum)
        {
            return new ShortGroupDTO
            {
                GroupId = groupEntity.Id,
                Line = groupEntity.Line,
                Name = groupEntity.Name,
                Category = categoryEnum.ToString(),
                ImageId = groupEntity.ImageId
            };
        }

        public ProducerDTO CreateProducer(Producer producer)
        {
            return new ProducerDTO
            {
                Id = producer.Id,
                Name = producer.Name,
                ImageId = producer.ImageId
            };
        } 

        protected DefaultGoodDTO CreateDefaultGood(Good entity, short? discount, Guid groupId)
        {
            DefaultGoodDTO defaultGoodDTO = new DefaultGoodDTO();
            FillGoodDto(defaultGoodDTO, entity, discount);
            defaultGoodDTO.GroupId = groupId;
            return defaultGoodDTO;
        }

        protected Good CreateGoodEntity(ICreateGood dto, CategoryEnum category)
        {
            return new Good
            {
                ImageId = dto.ImageId,
                Name = dto.Name,
                CategoryId = (byte)category,
                Price = dto.Price,
                IsSold = dto.IsSold,
                CreationDate = DateTime.Now
            };
        }

        private string GetCategoryName(byte categoryId)
        {
            CategoryEnum category = (CategoryEnum)categoryId;
            return category.ToString();
        }
    }
}
