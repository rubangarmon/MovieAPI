using MovieAPI.Core.Models;

namespace MovieAPI.Core.HttpClients
{
    public interface IHttpMediaRepository<TMedia> where TMedia : MediaBase
    {
        Task<Response<TMedia>> SearchMediaItemsByNameAsync(string name, int page);
    }
}
