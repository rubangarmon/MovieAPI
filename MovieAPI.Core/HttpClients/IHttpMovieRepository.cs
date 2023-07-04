using MovieAPI.Core.Models;

namespace MovieAPI.Core.HttpClients
{
    public interface IHttpMediaRepository<TMedia> where TMedia : MediaBase
    {
        Task<Response<TMedia>> GetMediaItemsByNameAsync(string name);
    }
}
