using MyJournalist.App.Abstract;

namespace MyJournalist.App.Common;

public class TxtFileService : ITxtFileService
{
    private readonly string _filePath;
    private readonly string _fileName;
    private readonly string _fullPath;

    public TxtFileService()
    {
        _filePath = @"D:\temp";
        _fileName = "myNotes.txt";
        _fullPath = Path.Combine(_filePath, _fileName);
    }

    public string ReadData()
    {
        try
        {
            using FileStream fileStream = new FileStream(_fullPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fileStream);

            return streamReader.ReadToEnd();
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {_filePath}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {_filePath} is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while accessing file {_filePath}", ex);
        }
    }
    public DateTimeOffset GetLastWriteTime()
    {
        try
        {
            return File.GetLastWriteTime(_fullPath);
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {_filePath}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {_filePath} is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while accessing file {_filePath}", ex);
        }
    }
    public void ClearData()
    {
        try
        {
            using FileStream fileStream = new FileStream(_fullPath, FileMode.Truncate, FileAccess.Write, FileShare.Read);
        }
        catch (IOException ex)
        {
            throw new IOException($"An I/O error occurred while accessing file {_filePath}", ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException($"Access to the file {_filePath} is not authorized", ex);
        }
        catch (Exception ex)
        {
            throw new Exception($"An unexpected error occurred while accessing file {_filePath}", ex);
        }
    }
}

