using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expanses.Update;

public class UpdateExpenseTest : CashFlowClassFixture
{
    private readonly string METHOD = "api/Expenses";
    private readonly long _expensesId;
    private readonly string _token;


    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _expensesId = webApplicationFactory.Expense.GetExpenseId();
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Sucess()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_expensesId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string cultureInfo)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;
        
        var result = await DoPut(requestUri: $"{METHOD}/{_expensesId}", request: request, token: _token, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage =
            ResourcesErrorMessage.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureInfo)
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage =
            ResourcesErrorMessage.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));
        
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
        
    }
    
}