using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expanses.Register;

public class RegisterExpanseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    
    private readonly string _token;

    public RegisterExpanseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();

        var result = await DoPost(requestUri:METHOD, request:request, token:_token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_empty(string cultureInfo)
    {
        var request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPost(requestUri:METHOD, request:request, token:_token, cultureInfo:cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage =
            ResourcesErrorMessage.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}