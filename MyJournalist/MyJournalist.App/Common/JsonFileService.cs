using Microsoft.Extensions.Configuration;
using MyJournalist.App.Abstract;
using MyJournalist.Domain.Common;
using Newtonsoft.Json;

namespace MyJournalist.App.Common;

public class JsonFileService<T> : IFileCollectionService<T> where T : BaseEntity
{
    private readonly string _filesPath;
    private readonly JsonSerializerSettings _jsonSettings;

    public JsonFileService(IConfiguration config)
    {
        _jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            CheckAdditionalContent = false,
            DateParseHandling = DateParseHandling.DateTimeOffset
        };
        _filesPath = config["DirSettings:JsonFileLocation"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Data");

    }

    public bool CheckFileExists(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_filesPath, $"{fileName}.json");

            if (File.Exists(fullPath))
                return true;

            else
                return false;

        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while checking file existence for {fileName}.json", ex);
        }
    }


    public void CreateFile(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_filesPath, $"{fileName}.json");
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                // Empty file is created
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {fileName}.json", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {fileName}.json is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while creating file {fileName}.json", ex);
        }
    }

    public ICollection<T> ReadFile(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_filesPath, $"{fileName}.json");

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))

            using (StreamReader reader = new StreamReader(fileStream))
            {
                string json = reader.ReadToEnd();
                var items = JsonConvert.DeserializeObject<ICollection<T>>(json, _jsonSettings);

                return items ?? new List<T>();
            }
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {fileName}.json", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {fileName}.json is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while reading file {fileName} .json", ex);
        }
    }

    public void UpdateFile(ICollection<T> obj, string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_filesPath, $"{fileName}.json");

            using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            using (var writer = new StreamWriter(fileStream))
            {
                var json = JsonConvert.SerializeObject(obj, Formatting.Indented, _jsonSettings);
                writer.Write(json);
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {fileName} .json", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {fileName}.json is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while updating file {fileName} .json", ex);
        }
    }

    public void ClearFile(string fileName)
    {
        try
        {
            var fullPath = Path.Combine(_filesPath, $"{fileName}.json");
            using (var fileStream = new FileStream(fullPath, FileMode.Truncate, FileAccess.Write, FileShare.Read))
            {
                // File size is truncated to 0 bytes
            }
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {fileName} .json", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {fileName}.json is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while clearing file {fileName} .json", ex);
        }
    }
}

