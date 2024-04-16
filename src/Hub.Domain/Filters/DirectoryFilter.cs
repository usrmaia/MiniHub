namespace Hub.Domain.Filters;

public class DirectoryFilter
{
    public string? Search { get; set; }

    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Flag { get; set; }
    public string? Role { get; set; }
    public string? UserId { get; set; }

    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = int.MaxValue;

    public string? NameOrderSort { get; set; }
    public string? CreatedAtOrderSort { get; set; }
    public string? UpdatedAtOrderSort { get; set; }
}
