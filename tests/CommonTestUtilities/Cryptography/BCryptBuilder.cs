using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class BCryptBuilder
{
    private readonly Mock<IPasswordEncripter> _encripterMock;
    
    public BCryptBuilder()
    {
        _encripterMock = new Mock<IPasswordEncripter>();
        
        _encripterMock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("fk2mfoKMOK#2ok");
    }

    public BCryptBuilder Verify(string? password)
    {
        if (string.IsNullOrEmpty(password) == false)
        { 
            _encripterMock.Setup(verify => verify.Verify(password, It.IsAny<string>())).Returns(true);
        }
        
        return this;
    }
    
    public IPasswordEncripter Build()
    {
        return _encripterMock.Object;
    }
}