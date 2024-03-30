using Hub.Domain.Entities;
using Hub.Domain.Repositories;
using Hub.Infra.Data.Context;

namespace Hub.Infra.Data.Repositories;

public class FileRepository : BaseRepository<FileE>, IFileRepository
{
    public FileRepository(AppDbContext context) : base(context) { }
}
