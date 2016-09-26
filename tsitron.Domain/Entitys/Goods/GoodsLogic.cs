using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using tsitron.Domain.Context;

namespace tsitron.Domain.Entitys.Goods
{
    public class ArtGenerator
    {
        public static string Atricul(Type type)
        {
            var db = ContextFabric.Context;

            var art = type == typeof (Flower) ? db.Flowers.Any() : db.Bouquets.Any();
            if (!art) return "00000001";
            var z = type == typeof (Flower) ? db.Flowers.Max(i=>i.Id) : db.Bouquets.Max(i => i.Id);
            var result = new StringBuilder();
            z++;
            for (var i = 0; i < 8-z.ToString().Length; i++)
            {
                result.Append("0");
            }
            return result.Append(z.ToString()).ToString();
        }

        public enum ImageType
        {
            Avatar=1, Photo
        }

        public enum GoodsType
        {
            Bouquet, Flower, Accessory
        }
    }

    public class FileUploda
    {
        public string GetSetError { get; set; }
        public decimal FileRatio { get; set; }
        public decimal FileSize { get; set; }

        public string UploadFile(HttpPostedFileBase file)
        {
            try
            {
                var supportedTypes = new[] {".jpg",".jpeg",".png",".bmp"};
                var fileExtention = Path.GetExtension(file.FileName);
                var img = Image.FromStream(file.InputStream);

                var fu = ((decimal)img.Height/img.Width);

                if (file.ContentLength>FileSize)
                {
                    GetSetError = "Файл слишком большой, уменьшите размер до 512кб.(0,5мб)";
                    return GetSetError;
                }
                if (!supportedTypes.Contains(fileExtention))
                {
                    GetSetError = "Допускаются файлы ра расширением: *.jpg, *.jpeg, *.png, *.bmp";
                    return GetSetError;
                }
                //if (fu != FileRatio)
                //{
                //    return
                //        GetSetError =
                //            "Пропорции картинки должны быть ширина: 400 высота: 600.";
                //}
                return GetSetError = "OK";
            }
            catch (Exception ex)
            {
                GetSetError = ex.Message;
                return GetSetError;
            }
        }
    }
}