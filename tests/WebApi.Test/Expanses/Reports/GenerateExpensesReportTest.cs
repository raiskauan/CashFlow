using System.Net;
using System.Net.Mime;
using CashFlow.Domain.Entities;
using FluentAssertions;

namespace WebApi.Test.Expanses.Reports;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private readonly string METHOD = "api/Report";
    
    private readonly string _adminToken;
    private readonly string _teamMemberToken;
    private readonly DateTime _expenseDate;
    
    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _adminToken = webApplicationFactory.User_Admin.GetToken();
        _teamMemberToken = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.Expense_Admin.GetDate();
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var formatedDate = _expenseDate.ToString("yyyy-MM");
        
        var result = await DoGet($"{METHOD}/pdf?month={formatedDate}", token:_adminToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        result.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }
    
    [Fact]
    public async Task Success_Excel()
    {
        var formatedDate = _expenseDate.ToString("yyyy-MM");
        
        var result = await DoGet($"{METHOD}/excel?month={formatedDate}", token:_adminToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        result.Should().NotBeNull();
        result.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }
    
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var formatedDate = _expenseDate.ToString("yyyy-MM");
        
        var result = await DoGet($"{METHOD}/pdf?month={formatedDate}", token:_teamMemberToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var formatedDate = _expenseDate.ToString("yyyy-MM");
        
        var result = await DoGet($"{METHOD}/excel?month={formatedDate}", token:_teamMemberToken);
        
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}