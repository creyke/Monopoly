using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monopoly.Providers.Spaces
{
    public interface ISpacesProvider
    {
        Task<IEnumerable<Space>> GetSpacesAsync();
    }
}
