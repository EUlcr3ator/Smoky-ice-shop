using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.DTO.Cart;
using SMOKYICESHOP_API_TEST.DtoService;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class CartsModel
    {
        private readonly SmokyIceDbContext _dbcontext;
        private readonly CartDtoService _cartDtoService;

        public CartsModel(SmokyIceDbContext dbcontext, CartDtoService cartDtoService)
        {
            _dbcontext = dbcontext;
            _cartDtoService = cartDtoService;
        }

        public void ClearCart(Guid clientId)
        {
            Client client = _dbcontext.Clients
                .Include(x => x.CartHasGoods)
                .First(x => x.Id == clientId);

            client.CartHasGoods.Clear();
            _dbcontext.SaveChanges();
        }

        public byte GetProductCount(Guid clientID, Guid goodID)
        {
            try
            {
                return _dbcontext.CartHasGoods
                .First(x => x.CartId == clientID && x.GoodId == goodID)
                .ProductCount;
            }
            catch(InvalidOperationException)
            {
                return 0;
            }
            
        }

        public IEnumerable<CartGoodDTO> GetGoods(Guid clientID)
        {
            return _dbcontext.CartHasGoods
                .Include(x => x.Good)
                    .ThenInclude(x => x.DiscountGood)
                .Where(x => x.CartId == clientID)
                .Select(x => _cartDtoService.CreateGood(x))
                .ToList();
        }

        public int GetGoodsCountInCart(Guid clientID)
        {
            return _dbcontext.CartHasGoods
                .Count(x => x.CartId == clientID);
        }

        public void ChangeCartGood(Guid goodID, Guid clientID, byte count)
        {
            CartHasGood? changedGood = _dbcontext.CartHasGoods
                .FirstOrDefault(x => x.CartId == clientID && x.GoodId == goodID);

            if (changedGood == null)
            {
                changedGood = new CartHasGood();
                changedGood.CartId = clientID;
                changedGood.GoodId = goodID;
                changedGood.ProductCount = count;
                _dbcontext.CartHasGoods.Add(changedGood);
            }
            else
            {
                if (count <= 0)
                {
                    _dbcontext.CartHasGoods.Remove(changedGood);
                }
                else
                {
                    changedGood.ProductCount = count;
                    _dbcontext.CartHasGoods.Update(changedGood);
                }
            }

            _dbcontext.SaveChanges();
        }
    }
}
