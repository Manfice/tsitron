using System.Web;

namespace tsitron.Domain.Models
{
    public class ImageViewModel
    {
        public int Bouquet { get; set; }
        public int Customer { get; set; }
        public HttpPostedFileBase UploadImage { get; set; }
        public string Descr { get; set; }
        public bool Avatar { get; set; } 
    }
}