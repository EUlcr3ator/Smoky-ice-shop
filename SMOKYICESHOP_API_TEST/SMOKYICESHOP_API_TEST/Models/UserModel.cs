using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class UserModel
    {
        private readonly SmokyIceDbContext _dbcontext;

        public UserModel(SmokyIceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public bool HasPhoneNumber(string number)
        {
            return _dbcontext.Clients.Any(x => x.PhoneNumber == number && x.Password == null);
        }
    }
}
