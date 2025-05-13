using CashFlow.Application.UseCases.Users.UpdatePassword;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Users.UpdatePassword;

public class UpdatePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdatePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_NewPassword_Empty(string newPassword)
    {
        var validator = new UpdatePasswordValidator();

        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(e => e.ErrorMessage.Equals(ResourcesErrorMessage.INVALID_PASSWORD));
    }
    
}