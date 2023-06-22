using System.ComponentModel.DataAnnotations;
using MyJournalist.App.Abstract;

namespace MyJournalist.App.Config;

public class FileConfig : IFileConfig
{
    [DirectoryLocation(ErrorMessage = "Invalid directory location")]
    public string JsonFileLocation { get; set; }

    [DirectoryLocation(ErrorMessage = "Invalid directory location")]
    public string TxtFileLocation { get; set; }

    [Required]
    [StringLength(100)]
    [TxtFileName(ErrorMessage = "File name must end with .txt")]
    public string TxtName { get; set; }

    public FileConfig()
    {
        JsonFileLocation = "";
        TxtFileLocation = "";
        TxtName = "";
    }
}
