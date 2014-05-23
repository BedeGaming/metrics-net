using System;
using NUnit.Framework;
using metrics.Core;
using metrics.Reporting;
using metrics.Tests.Core;

namespace metrics.Tests.Reporting
{
    [TestFixture]
    public class JsonReportFormatterTests
    {
        [Test]
        public void Can_serialize_metrics_with_changes()
        {
            var name = new MetricName("MeterTests", "Can_serialize_metrics_with_changes");
            var meter = Metrics.Meter("MeterTests", "Can_serialize_metrics_with_changes", "test", TimeUnit.Seconds);
            Assert.IsNotNull(Metrics.All[name], "Metric not found in central registry");

            meter.Mark(3);

            var reporter = new JsonReportFormatter();
            var json = reporter.GetSample();
            Console.WriteLine(json);
        }
    }
}
