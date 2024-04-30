namespace Hub.Domain.Filters;

public class ItemFilter
{
    public string? Name { get; set; }
    public string? UserId { get; set; }
    public string? FileId { get; set; }
    public string? DirectoryId { get; set; }
    public string? ParentId { get; set; }
    public string? FlagId { get; set; }
    public string? RoleId { get; set; }

    public string? NameOrderSort { get; set; }
    public string? FlagOrderSort { get; set; }
    public string? CreatedAtOrderSort { get; set; }
    public string? UpdatedAtOrderSort { get; set; }
    public string? UserNameOrderSort { get; set; }
}
