namespace IranKhodro;

public interface IEmailManager
{
    void Send(string address, string message);
}