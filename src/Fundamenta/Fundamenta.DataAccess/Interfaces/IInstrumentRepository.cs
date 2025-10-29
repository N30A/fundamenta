using System.Data;
using Fundamenta.Models;

namespace Fundamenta.DataAccess.Interfaces;

public interface IInstrumentRepository
{
    // Task<object> GetByIdAsync(id int);
    // Task<object> GetByISINAsync(string isin);
    // Task<IEnumerable<User>> ListAsync();
    Task UpsertMultipleAsync(IEnumerable<Instrument> instruments, IDbTransaction? transaction = null);
    // Task UpdateAsync(User user);
    // Task DeleteAsync(Guid id);
}
