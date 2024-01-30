using System.Text.Json;
using FruitWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitWebApp.Pages;

public class IndexModel : PageModel
{
    // IHttpClientFactory set using dependency injection 
    private readonly IHttpClientFactory _httpClientFactory;

    public IndexModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Add the data model and bind the form data to the page model properties
    // Enumerable since an array is expected as a response
    [BindProperty] public IEnumerable<FruitModel>? FruitModels { get; set; }

    // Begin GET operation code
    public async Task OnGetAsync()
    {
        var client = _httpClientFactory.CreateClient("FruitAPI");
        using var response = await client.GetAsync(string.Empty);

        if (response.IsSuccessStatusCode)
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            FruitModels = await JsonSerializer.DeserializeAsync<IEnumerable<FruitModel>>(responseStream);
        }
        else
        {
            FruitModels = Array.Empty<FruitModel>();
        }
    }
    // End GET operation code
}