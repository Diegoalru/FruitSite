using System.Text;
using System.Text.Json;
using FruitWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitWebApp.Pages;

public class EditModel : PageModel
{
    // IHttpClientFactory set using dependency injection 
    private readonly IHttpClientFactory _httpClientFactory;

    public EditModel(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    // Add the data model and bind the form data to the page model properties
    [BindProperty] public FruitModel? FruitModels { get; set; }

    // Retrieve the data to populate the form for editing
    public async Task OnGet(int id)
    {
        // Create the HTTP client using the FruitAPI named factory
        var httpClient = _httpClientFactory.CreateClient("FruitAPI");

        // Retrieve record information to populate the form
        using var response = await httpClient.GetAsync(id.ToString());

        if (response.IsSuccessStatusCode)
        {
            // Deserialize the response to populate the form
            await using var contentStream = await response.Content.ReadAsStreamAsync();
            FruitModels = await JsonSerializer.DeserializeAsync<FruitModel>(contentStream);
        }
    }

    // Begin PUT operation code
    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("FruitAPI");
        var json = JsonSerializer.Serialize(FruitModels);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await client.PutAsync(FruitModels!.Id.ToString(), content);

        if (response.IsSuccessStatusCode)
        {
            TempData["success"] = "Data was updated successfully.";
            return RedirectToPage("Index");
        }

        TempData["failure"] = "Oops! Something went wrong. Please try again.";
        return RedirectToPage("Index");
    }
    // End PUT operation code
}