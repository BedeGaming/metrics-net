﻿using System;
using System.Collections.Generic;
using System.Threading;
using metrics.Reporting;
using metrics.Tests.Core;
using NUnit.Framework;

namespace metrics.Tests.Reporting
{
    [TestFixture]
    public class ConsoleReporterTests
    {
        [Test]
        public void Can_run_with_known_counters_and_human_readable_format()
        {
            RegisterMetrics();

            var reporter = new ConsoleReporter();
            reporter.Run();
        }

        [Test]
        public void Can_run_with_known_counters_and_json_format()
        {
            RegisterMetrics();

            var reporter = new ConsoleReporter(new JsonReportFormatter());
            reporter.Run();
        }

        [Test]
        public void Can_run_in_background()
        {
            const int ticks = 3;
            var block = new ManualResetEvent(false);

            RegisterMetrics();

            ThreadPool.QueueUserWorkItem(
                s =>
                    {
                        var reporter = new ConsoleReporter();
                        reporter.Start(3, TimeUnit.Seconds);
                        while(true)
                        {
                            Thread.Sleep(1000);
                            var runs = reporter.Runs;
                            if (runs == ticks)
                            {
                                block.Set();
                            }    
                        }
                    }
                );

            block.WaitOne(TimeSpan.FromSeconds(5));
        }

        [Test]
        public void Can_stop()
        {
            var block = new ManualResetEvent(false);

            RegisterMetrics();

            ThreadPool.QueueUserWorkItem(
                s =>
                {
                    var reporter = new ConsoleReporter();
                    reporter.Start(1, TimeUnit.Seconds);
                    reporter.Stopped += delegate { block.Set(); };
                    Thread.Sleep(2000);
                    reporter.Stop();
                });

            block.WaitOne();
        }

        private static void RegisterMetrics()
        {
            var counter = Metrics.Counter("CounterTests", "Can_run_with_known_counters_counter");
            counter.Increment(100);

            var queue = new Queue<int>();
            Metrics.Gauge("GaugeTests", "Can_run_with_known_counters_gauge", () => queue.Count);
            queue.Enqueue(1);
            queue.Enqueue(2);
        }
    }
}
