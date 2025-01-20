namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    //Classe que guarda as respostas dos erros.
    
    public List<string> ErrorMessages { get; set; }

    public ResponseErrorJson(string errorMessage)
    {
        ErrorMessages = [errorMessage];
    }

    public ResponseErrorJson(List<string> errorMessage)
    {
        ErrorMessages = errorMessage;  
    }
}