using System.Data;
using Fundamenta.Models;

namespace Fundamenta.DataAccess.Interfaces;

public interface IFundRepository
{
    // Task<object> GetByIdAsync(id int);
    Task<Fund?> GetByIsinAsync(string isin, IDbTransaction? transaction = null);
    // Task<IEnumerable<User>> ListAsync();
    Task AddHoldingsAsync(int fundId, IEnumerable<InstrumentHolding> holdings, IDbTransaction? transaction = null);
    // Task UpdateAsync(User user);
    // Task DeleteAsync(Guid id);
}
