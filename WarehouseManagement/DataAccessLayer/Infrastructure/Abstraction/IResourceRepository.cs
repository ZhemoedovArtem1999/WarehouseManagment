using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Infrastructure.Abstraction
{
    public interface IResourceRepository
    {
        Task<IEnumerable<Resource>> GetAllAsync(bool? isArchive = null, IEnumerable<int>? ids = null, CancellationToken cancellationToken = default);
        Task<Resource?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Resource?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task CreateAsync(Resource resource, CancellationToken cancellationToken);
        Task UpdateAsync(Resource resource, CancellationToken cancellationToken);
        Task DeleteAsync(Resource resource, CancellationToken cancellationToken);
    }
}
