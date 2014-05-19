using System.Threading;
using NUnit.Framework;

namespace metrics.Tests.Core
{
   [TestFixture]
   public class TimerTests : MetricTestBase
   {
      [Test, Ignore]
      public void CallbackTimerTestBasic()
      {
         var timer = Metrics.CallbackTimer(typeof(TimerTests), "test", TimeUnit.Milliseconds, TimeUnit.Milliseconds);

         for (int i = 0; i < 10; i++)
         {
            var ctx = timer.Time();
            Thread.Sleep(250);
            ctx.Stop();
         }

         Assert.AreEqual(250d, timer.Mean, 5); // made-up delta values that "seems" right
         Assert.AreEqual(1d, timer.StdDev, 1);
      }

      [Test]
      public void ManualTimerTestBasic()
      {
         var timer = Metrics.ManualTimer(typeof(TimerTests), "test", TimeUnit.Milliseconds, TimeUnit.Milliseconds);

         for (int i = 0; i < 10; i++)
         {
            timer.RecordElapsedMillis(250);
         }

         Assert.AreEqual(250d, timer.Mean, 5); // made-up delta values that "seems" right
         Assert.AreEqual(1d, timer.StdDev, 1);
      }


   }
}