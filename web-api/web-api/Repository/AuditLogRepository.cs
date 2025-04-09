using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class AuditLogRepository(FMSContext context) : BaseRepository<AuditLog>(context), IAuditLogRepository
{
    private new readonly FMSContext _context = context;
}
