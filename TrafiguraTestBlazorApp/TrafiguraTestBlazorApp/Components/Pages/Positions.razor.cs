using Microsoft.AspNetCore.Components;
using TrafiguraTestModels;

namespace TrafiguraTestBlazorApp.Components.Pages;

public partial class Positions
{
    private TransactionDTO TransactionData = new TransactionDTO();
    private bool isLoaded = false;

    [Inject]
    public HttpClient _httpClient { get; set; }

    protected override async Task OnInitializedAsync()
    {
        TransactionData.Positions = new List<TransactionModel>();
        //string baseUrl = "https://localhost:7183/";
        //TransactionData = await _httpClient.GetFromJsonAsync<TransactionDTO>($"{baseUrl}api/Transactions/GetPositions");
        TransactionData = await _httpClient.GetFromJsonAsync<TransactionDTO>("/api/Transactions/GetPositions");
        isLoaded = true;
    }
}
