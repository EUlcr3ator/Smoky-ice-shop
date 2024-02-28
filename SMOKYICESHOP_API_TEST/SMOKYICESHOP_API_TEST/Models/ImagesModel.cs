using Microsoft.EntityFrameworkCore;
using SMOKYICESHOP_API_TEST.Entities;

namespace SMOKYICESHOP_API_TEST.Models
{
    public class ImagesModel
    {
        private readonly SmokyIceDbContext _dbcontext;

        public ImagesModel(SmokyIceDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public byte[] GetImage(Guid imageId)
        {
            return _dbcontext.Images.First(x => x.Id == imageId).ImageBinary;
        }

        public Guid AddImage(byte[] binary)
        {
            List<Image> images = _dbcontext.Images.ToList();

            foreach (var item in images)
                if (IsArraysEqual(item.ImageBinary, binary))
                    return item.Id;

            Image image = new Image { ImageBinary = binary };
            _dbcontext.Images.Add(image);
            _dbcontext.SaveChanges();
            return image.Id;
        }

        public void DeleteNotUsed()
        {
            RebindImages();

            Guid defaultImageId = new Guid("00000000-0000-0000-0000-000000000001");
            List<Image> images = _dbcontext.Images
                .Include(x => x.Producers)
                .Include(y => y.Goods)
                .ToList();

            foreach (Image image in images)
                if (image.Goods.Count == 0 && image.Producers.Count == 0 && image.Id != defaultImageId)
                    _dbcontext.Images.Remove(image);

            _dbcontext.SaveChanges();
        }

        private void RebindImages()
        {

            List<Image> images = _dbcontext.Images.ToList();
            List<Good> goods = _dbcontext.Goods.ToList();
            List<Producer> producers = _dbcontext.Producers.ToList();

            Console.WriteLine("Total images" + images.Count.ToString());

            for (int i = 0; i < images.Count; i++)
            {
                Image image = images[i];
                IEnumerable<Image> sameImages = images.Where(x => IsArraysEqual(x.ImageBinary, image.ImageBinary) && x.Id != image.Id).ToList();
                Console.WriteLine("SameImages" + sameImages.Count().ToString());
                foreach (var sameImage in sameImages)
                {
                    foreach (Good good in goods)
                    {
                        if (good.ImageId == sameImage.Id)
                        {
                            good.ImageId = image.Id;
                            _dbcontext.Goods.Update(good);
                        }
                    }
                    foreach (Producer producer in producers)
                    {
                        if (producer.ImageId == sameImage.Id)
                        {
                            producer.ImageId = image.Id;
                            _dbcontext.Producers.Update(producer);
                        }
                    }
                    _dbcontext.Images.Remove(sameImage);
                    images.Remove(sameImage);
                }
            }

            Console.WriteLine("Total images" + images.Count.ToString());
            _dbcontext.SaveChanges();
        }

        private bool IsArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return false;

            return true;
        }
    }
}
