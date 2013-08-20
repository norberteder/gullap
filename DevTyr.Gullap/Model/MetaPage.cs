
namespace DevTyr.Gullap.Model
{
    public class MetaPage
    {
        public MetaPage(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; private set; }
        public Page Page { get; set; }
    }
}
