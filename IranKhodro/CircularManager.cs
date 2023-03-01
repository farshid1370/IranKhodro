

using Newtonsoft.Json;
using Redis.OM.Searching;
using Redis.OM;
using System;
using System.Text.Json.Serialization.Metadata;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IranKhodro;

public class CircularManager : ICircularManager
{
    private readonly RedisCollection<Circular> _circular;
    private readonly RedisConnectionProvider _provider;
    private readonly IOptions<Settings> _options;

    public CircularManager(RedisConnectionProvider provider, IOptions<Settings> options)
    {
        _provider = provider;
        _options = options;
        _circular = (RedisCollection<Circular>)provider.RedisCollection<Circular>();
    }

    public async Task<List<string>> GetCircularList()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("cookie", _options.Value.Cookie);

            var response = await client.GetAsync("https://esale.ikco.ir/api/services/OnlineSales/priceList/getGoyaList");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var parsedResponse = JsonConvert.DeserializeObject<Response>(responseBody);

            var list = _circular.ToList();
            var notifyList = new List<string>();
            parsedResponse?.Result.CurrentSales.ForEach(async item =>
            {
                if (list.Any(x => x.Id == item.Id)) return;
                await _circular.InsertAsync(item);
                notifyList.Add(item.Title);

            });


            return notifyList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }
    
}