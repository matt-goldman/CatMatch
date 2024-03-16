using CatMatch.Models;
using System.Net.Http.Json;

namespace CatMatch.Services;

public interface ICatService
{
    Task<List<Cat>> GetCatsAsync();
}

public class CatService : ICatService
{
    private readonly HttpClient _httpClient;

    private int numberOfCats = 4;

    public CatService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<List<Cat>> GetCatsAsync()
    {
        try
        {
            var cats = await _httpClient.GetFromJsonAsync<List<Cat>>(Constants.FetchUri);

            if (cats is not null)
            {
                if (cats.Count > 0)
                {
                    return cats.Take(numberOfCats).ToList();
                }
            }

            // todo: log something
            return new List<Cat>();
        }
        catch (Exception ex)
        {
            // todo: log exception
            return new List<Cat>();
        }
    }
}
