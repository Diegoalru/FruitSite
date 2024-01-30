using System.Text;
using System.Text.Json;
using FruitWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FruitWebApp.Pages;

public class AddModel : PageModel
{
    // IHttpClientFactory set using dependency injection 
    private readonly IHttpClientFactory _httpClientFactory;

    public AddModel(IHttpClientFactory httpClientFactory, FruitModel fruitModels)
    {
        _httpClientFactory = httpClientFactory;
        FruitModels = fruitModels;
    }

    // Add the data model and bind the form data to the page model properties
    [BindProperty] public FruitModel FruitModels { get; set; }

    // Begin POST operation code
    public async Task<IActionResult> OnPostAsync()
    {
        var client = _httpClientFactory.CreateClient("FruitAPI");
        var json = JsonSerializer.Serialize(FruitModels);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await client.PostAsync("", content);

        if (response.IsSuccessStatusCode)
        {
            TempData["success"] = "Data was added successfully.";
            return RedirectToPage("Index");
        }

        TempData["failure"] = "Oops! Something went wrong. Please try again.";
        return RedirectToPage("Index");
    }
    // End POST operation code
}