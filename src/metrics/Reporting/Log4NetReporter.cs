using System.IO;
using Bede.Logging.Models;
using Bede.Logging.Providers;
using log4net;

namespace metrics.Reporting
{
    public class Log4NetReporter : ReporterBase
    {
        private static readonly ILoggingService _loggingService = new Log4NetLoggingService(LogManager.GetLogger("Metrics"));

        public Log4NetReporter(IReportFormatter formatter) : base(formatter)
        {
        }

        public Log4NetReporter(TextWriter writer) : base(writer)
        {
        }

        public Log4NetReporter(TextWriter writer, IReportFormatter formatter) : base(writer, formatter)
        {
        }

        public override void Run()
        {
            _loggingService.Information(new CommonLoggingData(Formatter.GetSample(), "Metrics"));
            Runs++;
        }
    }
}
