
namespace IniParser
{
    public class Grammar
    {
        public string section = "^\\[[a-zA-Z_0-9]*\\]$";
        public string key = "^[a-zA-Z_0-9]* \\=";
        public string valueString = "[a-zA-Z\\.]*$";
        public string valueFloat = "[0-9]*[.,][0-9]*$";
        public string valueInt = "[0-9]*$";
    }
}