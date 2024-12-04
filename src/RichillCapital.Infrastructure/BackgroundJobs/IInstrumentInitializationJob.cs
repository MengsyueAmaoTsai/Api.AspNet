namespace RichillCapital.Infrastructure.BackgroundJobs;

public interface IInstrumentInitializationJob
{
    Task ProcessAsync();
}
