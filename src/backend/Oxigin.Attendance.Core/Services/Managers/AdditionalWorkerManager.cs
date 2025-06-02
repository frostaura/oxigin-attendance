using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oxigin.Attendance.Core.Interfaces.Managers;
using Oxigin.Attendance.Datastore.Interfaces;
using Oxigin.Attendance.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oxigin.Attendance.Core.Services.Managers;

/// <summary>
/// Manager for handling AdditionalWorker use cases.
/// </summary>
public class AdditionalWorkerManager : IAdditionalWorkerManager
{
    private readonly IDatastoreContext _db;
    private readonly ILogger<AdditionalWorkerManager> _logger;

    public AdditionalWorkerManager(IDatastoreContext db, ILogger<AdditionalWorkerManager> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<AdditionalWorker>> GetByJobIdAsync(Guid jobId, CancellationToken token)
    {
        return await _db.AdditionalWorkers.Where(w => w.JobID == jobId).ToListAsync(token);
    }

    public async Task<AdditionalWorker> AddAsync(AdditionalWorker worker, CancellationToken token)
    {
        _db.AdditionalWorkers.Add(worker);
        await _db.SaveChangesAsync(token);
        return worker;
    }

    public async Task RemoveAsync(Guid id, CancellationToken token)
    {
        var worker = await _db.AdditionalWorkers.FirstOrDefaultAsync(w => w.Id == id, token);
        if (worker != null)
        {
            _db.AdditionalWorkers.Remove(worker);
            await _db.SaveChangesAsync(token);
        }
    }
}
