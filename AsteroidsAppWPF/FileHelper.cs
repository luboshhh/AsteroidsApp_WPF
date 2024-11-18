using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidsAppWPF
{
    public static class FileHelper
    {
        public static string ReadApiKey(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path).Trim() : null;
        }

        public static void WriteApiKey(string path, string apiKey)
        {
            File.WriteAllText(path, apiKey);
        }
    }
}
