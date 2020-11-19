using System;
using System.Collections.Generic;
using System.IO;

namespace CatalogCoder
{
    public class FilesStorage
    {
        public string CatalogName { get; set; }
        public Dictionary<string, byte[]> FilesByName { get; set; }
        public string[] DirectoryesNames { get; set; }

        private FilesStorage()
        {
            FilesByName = new Dictionary<string, byte[]>();
        }

        private FilesStorage(string catalogName) : this()
        {
            CatalogName = catalogName;
            DirectoryesNames = Directory.GetDirectories(catalogName, "*", SearchOption.AllDirectories);
        }

        public static FilesStorage CreateByCatalogName(string catalogName, bool skipEx = true)
        {
            FilesStorage filesStorage = new FilesStorage(catalogName);

            foreach (var fileName in Directory.GetFiles(catalogName, "*", SearchOption.AllDirectories))
            {
                try
                {
                    var body = File.ReadAllBytes(fileName);
                    filesStorage.FilesByName.Add(fileName, body);
                }
                catch
                {
                    if (skipEx)
                    {
                        Console.WriteLine($"Ошибка чтения файла (файл пропущен): {fileName}");
                    }
                    else
                        throw;
                }
            }

            return filesStorage;
        }

        public void DeleteCatalog() => Directory.Delete(CatalogName, true);

        public void RestoreCatalog()
        {
            foreach (var directoryName in DirectoryesNames)
                Directory.CreateDirectory(directoryName);

            foreach (var kvp in FilesByName)
                File.WriteAllBytes(kvp.Key, kvp.Value);
        }
    }
}
