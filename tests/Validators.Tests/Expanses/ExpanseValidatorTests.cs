using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expanses;

public class ExpanseValidatorTests
{
    [Fact]
    public void Success()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("            ")]
    [InlineData(null)]
    public void ErrorTitleEmpty(string title)
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = title;
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourcesErrorMessage.TITLE_REQUIRED));
    }

    [Fact]
    public void ErrorDateFuture()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourcesErrorMessage.EXPENSES_CANNOT_FOR_THE_FUTURE));
    }
    
    [Fact]
    public void ErrorPaymentTypeInvalid()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.PaymentType = (PaymentType)700;
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourcesErrorMessage.PAYMENT_TYPE_INVALID));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-7)]
    public void ErrorAmountInvalid(decimal amount)
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Amount = amount;
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourcesErrorMessage.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
    
    [Fact]
    public void Error_Tag_Invalid()
    {
        //Arrange
        var validator = new ExpanseValidator();
        var request = RequestExpenseJsonBuilder.Build();
        request.Tags.Add((Tag)1000);
        
        //Act
        var result = validator.Validate(request);
        
        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(error => error.ErrorMessage.Equals(ResourcesErrorMessage.TAG_TYPE_NOT_SUPPORTED));
    }
}