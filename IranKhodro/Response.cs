namespace IranKhodro;

public class Response
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public bool UnAuthorizedRequest { get; set; }
    public Result Result { get; set; }
}