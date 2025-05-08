using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace WebApi.Test.Users.GetProfile;

public class GetUserProfileTest : CashFlowClassFixture
{
    private readonly string METHOD = "api/User";
    
    private readonly string _token;
    private readonly string _username;
    private readonly string _useremail;
    
    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _username = webApplicationFactory.User_Team_Member.GetName();
        _useremail = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(METHOD, _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var body = await result.Content.ReadAsStreamAsync();
        
        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("name").GetString().Should().Be(_username);
        response.RootElement.GetProperty("email").GetString().Should().Be(_useremail);
    }
}