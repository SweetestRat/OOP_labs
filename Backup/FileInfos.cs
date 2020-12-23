
namespace Backup
{
    public class FileInfos
    {
        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }
        
        private double size;
        public double Size
        {
            get => size;
            set => size = value;
        }
        
        private string path;
        public string Path
        {
            get => path;
            set => path = value;
        }

        public FileInfos(string name, double size, string path)
        {
            Name = name;
            Size = size;
            Path = path;
        }
    }
}