using TrafiguraTestModels;

namespace TrafiguraTestBlazorApp.Components.Pages;

public partial class Positions
{
    private TransactionDTO TransactionData = new TransactionDTO();
    private bool isLoaded = false;
    public HttpClient _httpClient = new HttpClient();

    protected override async Task OnInitializedAsync()
    {
        TransactionData.Positions = new List<TransactionModel>();
        string baseUrl = "https://localhost:7183/";
        TransactionData = await _httpClient.GetFromJsonAsync<TransactionDTO>($"{baseUrl}api/Transactions/GetPositions");
        isLoaded = true;
    }
}
