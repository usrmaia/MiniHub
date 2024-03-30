namespace Hub.Domain.Entities;

public class DirectoryFlag
{
    public string DirectoryId { get; set; } = string.Empty;
    public DirectoryE? Directory { get; set; }
    public string FlagId { get; set; } = string.Empty;
    public Flag? Flag { get; set; }
}
