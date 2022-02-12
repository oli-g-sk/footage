namespace Footage.Service.SourceScoped
{
    using System.Threading.Tasks;
    using Footage.Model;

    public interface IMediaInfoService
    {
        Task<long> GetDuration(string uri);
    }
}