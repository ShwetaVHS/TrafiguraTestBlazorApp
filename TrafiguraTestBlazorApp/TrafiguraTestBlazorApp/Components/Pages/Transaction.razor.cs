using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Formatting;
using TrafiguraTestModels;

namespace TrafiguraTestBlazorApp.Components.Pages;

public partial class Transaction
{
    TransactionDTO transactionForm = new() { Transaction = new(), Positions = new() };
    private Dictionary<int, string> options = new Dictionary<int, string>();
    private ValidationMessageStore _validationMessages;

    [Inject]
    public HttpClient _httpClient { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        options.Clear();
        options.Add(1, "Buy");
        options.Add(2, "Sell");
    }

    protected async Task Save(EditContext formContext)
    {
        transactionForm.Transaction.IsActive = true;
        await OnSubmit(formContext);
    }

    protected async Task Delete(EditContext formContext)
    {
        transactionForm.Transaction.IsActive = false;
        await OnSubmit(formContext);
    }

    private async Task OnSubmit(EditContext formContext)
    {
        _validationMessages = new(formContext);
        var postalCodeField = formContext.Field(nameof(TransactionModel.SubmitAction));
        try
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
            {
                return;
            }

            var response = await _httpClient.PostAsync("/api/Transactions/SaveTransactions", transactionForm, new JsonMediaTypeFormatter());
            if (response.IsSuccessStatusCode)
            {
                NavigationManager.NavigateTo("/positions", false);
            }
            else
            {
                _validationMessages.Add(postalCodeField, GenericMothod.GetErrorMessage(Convert.ToInt32(response.StatusCode)));
            }
        }
        catch (Exception ex)
        {
            _validationMessages.Add(postalCodeField, ex.Message);
        }
    }
}