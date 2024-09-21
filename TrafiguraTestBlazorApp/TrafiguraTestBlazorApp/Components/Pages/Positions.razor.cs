using System.Text.Json;
using TrafiguraTestModels;

namespace TrafiguraTestBlazorApp.Components.Pages;

public partial class Positions
{
    private TransactionDTO TransactionData = new TransactionDTO();
    private bool isLoaded = false;
    private HttpClient _httpClient;

    protected override async Task OnInitializedAsync()
    {
        TransactionData.Positions = new List<TransactionModel>();
        string baseUrl = "https://localhost:7183/";
        var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}api/Transactions/GetPositions");
        var client = ClientFactory.CreateClient();

        var response = await client.SendAsync(request);

        if (response != null && response.IsSuccessStatusCode)
        {
            using var responseStream = await response.Content.ReadAsStreamAsync();

            //todo: not completed all property mapping, was trying another approach
            TransactionData.Positions = await JsonSerializer.DeserializeAsync<List<TransactionModel>>(responseStream);
            TransactionData.ErrCode = ErrCode.OK; 
            

            //JToken data = JToken.Parse(responseStream);
            //if (data != null && data.HasValues)
            //{
            //    TransactionData.Positions = (data[nameof(TransactionDTO.Positions)].Any())
            //        ? (from dataItem in data[nameof(TransactionDTO.Positions)]
            //           select new TransactionModel
            //           {
            //               SecurityCode = (string)dataItem[nameof(TransactionModel.SecurityCode)],
            //               Quantity = (int)dataItem[nameof(TransactionModel.Quantity)]
            //           }).ToList()
            //        : new List<TransactionModel>();
            //}
            TransactionData.ErrCode = ErrCode.OK;
        }
        else
        {
            TransactionData.ErrCode = ErrCode.InternalServerError;
        }

        isLoaded = true;
    }
}
