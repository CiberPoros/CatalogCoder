using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace CatalogCoder
{
    class Program
    {
        private const string STORAGE_PATH = "secured.sec";

        private static void Main()
        {
            var workMode = ReadWorkMode();

            switch (workMode)
            {
                case WorkMode.NONE:
                    break;
                case WorkMode.ENCRYPT:
                    HandleEncrypt();
                    break;
                case WorkMode.DECRYPT:
                    HandleDecrypt();
                    break;
                default:
                    break;
            }
        }

        private static void HandleEncrypt()
        {
            var path = ReadPath();
            var key = ReadKey();
            var filesStorage = FilesStorage.CreateByCatalogName(path);

            var json = JsonSerializer.Serialize(filesStorage);
            var byteJson = Encoding.Default.GetBytes(json);
            var EncryptedByteJson = Securer.VisinerTransform(byteJson, key, WorkMode.ENCRYPT);

            File.WriteAllBytesAsync(STORAGE_PATH, EncryptedByteJson);
            filesStorage.DeleteCatalog();
        }

        private static void HandleDecrypt()
        {
            var key = ReadKey();

            var EncryptedByteJson = File.ReadAllBytes(STORAGE_PATH);
            var byteJson = Securer.VisinerTransform(EncryptedByteJson, key, WorkMode.DECRYPT);
            var json = Encoding.Default.GetString(byteJson);

            var filesStorage = JsonSerializer.Deserialize<FilesStorage>(json);
            filesStorage.RestoreCatalog();
        }

        private static byte[] ReadKey()
        {
            Console.WriteLine("Введите ключ шифрования...");

            return Encoding.Default.GetBytes(Console.ReadLine());
        }

        private static string ReadPath()
        {
            Console.WriteLine("Введите полный путь каталога...");

            for (;;)
            {
                var path = Console.ReadLine();

                if (Directory.Exists(path))
                    return path;

                Console.WriteLine("Введен некорректный путь, или указанный каталог отсутствует. Повторите попытку...");
            }
        }

        private static WorkMode ReadWorkMode()
        {
            Console.WriteLine("1 - шифрование;");
            Console.WriteLine("2 - дешифрование...");

            for (;;)
            {
                var key = Console.ReadKey();
                ClearLastKeyInConsole();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        return WorkMode.ENCRYPT;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        return WorkMode.DECRYPT;
                    default:
                        Console.WriteLine();
                        Console.WriteLine("Неверный ввод! Повторите попытку...");
                        break;
                }
            }
        }

        private static void ClearLastKeyInConsole()
        {
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            Console.Write(" ");
            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
        }
    }
}
