using Hub.Domain.Entities;

namespace Hub.Domain.DTOs;

public class Items
{
    public List<DirectoryE> Directories { get; set; } = [];
    public List<FileE> Files { get; set; } = [];
    public int TotalCount { get; set; }

    public Items() { }

    public Items(List<DirectoryE> directories, List<FileE> files, int? totalCount)
    {
        Directories = directories;
        Files = files;
        TotalCount = totalCount ?? 0;
    }
}
