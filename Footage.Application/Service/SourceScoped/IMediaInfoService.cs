namespace Footage.Application.Service.SourceScoped
{
    using System.Threading.Tasks;

    public interface IMediaInfoService
    {
        Task<long> GetDuration(string uri);
    }
}