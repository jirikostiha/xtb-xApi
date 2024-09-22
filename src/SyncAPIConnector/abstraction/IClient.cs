using System.Threading.Tasks;
using System.Threading;

namespace Xtb.XApi;

/// <summary>
/// Remote client interface.
/// </summary>
public interface IClient : IConnectable, ISender, IReceiver
{
}