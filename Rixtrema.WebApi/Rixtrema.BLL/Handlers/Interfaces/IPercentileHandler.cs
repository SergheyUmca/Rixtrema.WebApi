using System.Threading.Tasks;

namespace Rixtrema.BLL.Handlers.Interfaces
{
    public interface IPercentileHandler
    {
        Task<string> CompletePercentile(int type = 0);
    }
}