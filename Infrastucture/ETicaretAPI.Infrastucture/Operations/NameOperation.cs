using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastucture.StaticService
{
    public static class NameOperation
    {
        public static string ChracterRegulatory(string name)
        {
            name.Replace("\"", "")
                .Replace("!", "")
                .Replace("'", "")
                .Replace("^", "")
                .Replace("%", "")
                .Replace("+", "")
                .Replace("&", "")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("=", "")
                .Replace("?", "")
                .Replace(">", "")
                .Replace("£", "")
                .Replace("#", "")
                .Replace("$", "")
                .Replace("½", "")
                .Replace("{", "")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("}", "")
                .Replace("\"", "")
                .Replace(":", "")
                .Replace(";", "")
                .Replace(",", "")
                .Replace("`", "")
                .Replace(".", "-")
                .Replace("Ö", "o")
                .Replace("ö", "o")
                .Replace("Ü", "u")
                .Replace("ü", "u")
                .Replace("ı", "i")
                .Replace("İ", "i")
                .Replace("ğ", "g")
                .Replace("Ğ", "g")
                .Replace("æ", "")
                .Replace("ß", "")
                .Replace("Ş", "s")
                .Replace("ş", "s")
                .Replace("Ç", "c")
                .Replace("ç", "c");

            return name;
        }

    }
}
