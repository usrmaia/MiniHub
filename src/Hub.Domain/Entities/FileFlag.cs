namespace Hub.Domain.Entities;

public class FileFlag
{
    public string FileId { get; set; } = string.Empty;
    public FileE? File { get; set; }
    public string FlagId { get; set; } = string.Empty;
    public Flag? Flag { get; set; }
}
