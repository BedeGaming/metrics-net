﻿using NUnit.Framework;

namespace metrics.Tests.Core
{
    [TestFixture]
    public class CounterTests : MetricTestBase
    {
        [Test]
        public void Can_count()
        {
            var counter = Metrics.Counter("CounterTests", "Can_count");
            Assert.IsNotNull(counter);
            
            counter.Increment(100);
            Assert.AreEqual(100, counter.Count);
        }

        [TearDown]
        public void TearDown()
        {
            Metrics.Clear();
        }
    }
}
