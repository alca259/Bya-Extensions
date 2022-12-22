using System.IO.Compression;

namespace ConsoleAppTest;

internal class Program
{
    private static void Main(string[] args)
    {
        var fileLogService = new FileLogService();

        Console.WriteLine("Compresing log files...");
        var stream = fileLogService.GetLogZipFile(new DateTime(2022, 12, 1));
        Console.WriteLine("Zip length: {0}", stream?.Length ?? 0);
        Console.ReadLine();
    }
}

internal class FileLogService
{
    public Stream? GetLogZipFile(DateTime fromDate, DateTime? toDate = null)
    {
        var dirPath = AppDomain.CurrentDomain.BaseDirectory;
        dirPath = Path.Combine(dirPath, "Logs");
        if (!Directory.Exists(dirPath))
            throw new DirectoryNotFoundException($"El directorio {dirPath} no se encuentra.");

        const string FileNameFormat = "api";

        toDate ??= DateTime.Today;
        if (toDate < fromDate) toDate = fromDate;
        fromDate = fromDate.Date;
        toDate = toDate.Value.Date;

        List<string> pathFiles = new();
        for (DateTime d = fromDate; d <= toDate.Value; d = d.AddDays(1))
        {
            Console.WriteLine("Finding log file {0}", $"{FileNameFormat}*{d:yyyyMMdd}.*");
            pathFiles.AddRange(Directory.GetFiles(dirPath, $"{FileNameFormat}*{d:yyyyMMdd}.*"));
        }

        if (pathFiles.Count <= 0) return null;

        var ms = new MemoryStream();
        using var zipFile = new ZipArchive(ms, ZipArchiveMode.Create, true);
        foreach (var filePath in pathFiles)
        {
            Console.WriteLine("Compresing log file {0}", filePath);
            _ = zipFile.CreateEntryFromFile(filePath, Path.GetFileName(filePath), CompressionLevel.Optimal);
        }

        return ms;
    }
}