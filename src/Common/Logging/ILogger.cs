namespace Decoherence.Logging
{
    public interface ILogger
    {
        void Write(LogLevel logLevel, string message);
    }
}