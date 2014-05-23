using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using metrics.Reporting;
using metrics.Tests.Core;

namespace metrics.Tests.Reporting
{
    [TestFixture]
    public class SampleFileReporterTests
    {
        [Test]
        public void Can_run_with_known_counters_and_human_readable_format()
        {
            RegisterMetrics();

            var reporter = new SampledFileReporter(Path.GetTempPath());
            reporter.Run();
        }

        [Test]
        public void Can_run_with_known_counters_and_json_format()
        {
            RegisterMetrics();

            var reporter = new SampledFileReporter(Path.GetTempPath(), new JsonReportFormatter());
            reporter.Run();
        }

        [Test]
        public void Can_run_in_background()
        {
            const int ticks = 3;
            var block = new ManualResetEvent(false);

            RegisterMetrics();

            var directory = Path.GetTempPath();
            var samples = Directory.GetFiles(directory, "*.sample");
            foreach (var file in samples)
            {
                File.Delete(file);
            }

            ThreadPool.QueueUserWorkItem(
                s =>
                {
                    var reporter = new SampledFileReporter(directory, new JsonReportFormatter());
                    reporter.Start(1, TimeUnit.Seconds);
                    while (true)
                    {
                        Thread.Sleep(1000);
                        var runs = reporter.Runs;
                        if (runs == ticks)
                        {
                            block.Set();
                            reporter.Stop();
                            reporter.Dispose();
                        }
                    }
                });

            block.WaitOne(TimeSpan.FromSeconds(5));
            samples = Directory.GetFiles(directory, "*.sample");
            Assert.GreaterOrEqual(samples.Length, 3);
        }

        [Test]
        public void Can_stop()
        {
            var block = new ManualResetEvent(false);

            RegisterMetrics();

            ThreadPool.QueueUserWorkItem(
                s =>
                {
                    var reporter = new SampledFileReporter(Path.GetTempPath());
                    reporter.Start(1, TimeUnit.Seconds);
                    reporter.Stopped += delegate { block.Set(); };
                    Thread.Sleep(2000);
                    reporter.Stop();
                });

            block.WaitOne();
        }

        private static void RegisterMetrics()
        {
            Metrics.Clear();

            var counter = Metrics.Counter("CounterTests", "Can_run_with_known_counters_counter");
            counter.Increment(100);

            var queue = new Queue<int>();
            Metrics.Gauge("GaugeTests", "Can_run_with_known_counters_gauge", () => queue.Count);
            queue.Enqueue(1);
            queue.Enqueue(2);
        }
    }
}
