namespace PDH.Application.Interfaces;

public interface IIntegrationFactory
{
    IIntegrationService GetService(string provider);
}
