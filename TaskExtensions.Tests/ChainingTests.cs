using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TaskExtensions.Tests
{
	[TestFixture]
	public class ChainingTests
	{
		[Test]
		public void Actions_Chained_To_Canceled_Tasks_Dont_Run()
		{
			var counter = 1;
			var ct = new CancellationToken(canceled: true);
			var task = Task.Run(() => 1, ct);

			var t = task.Then(() => { counter += 1; });
			Assert.Throws<AggregateException>(() => t.Wait());
			Assert.AreEqual(1, counter);
		}

		[Test]
		public void Actions_Chained_To_Failed_Tasks_Dont_Run()
		{
			var counter = 1;
			var task = new Task(() =>
			{
				throw new Exception("always fails");
			});

			var t = task.Then(() => { counter += 1; });
			task.Start();

			Assert.Throws<AggregateException>(() => t.Wait());
			Assert.AreEqual(1, counter);
		}

		[Test]
		public void Actions_Can_Be_Chained_To_Tasks()
		{
			var dummy = false;
			var counter = 1;
			var task = new Task(() => dummy = true);

			var t = task.Then(() => { counter += 1; });
			task.Start();
			t.Wait();
			Assert.AreEqual(2, counter);
		}

		[Test]
		public void Actions_Can_Be_Chained_To_Completed_Tasks()
		{
			var dummy = false;
			var counter = 1;
			var task = Task.Run(() => dummy = true);

			task.Wait();
			task.Then(() => counter += 1).Wait();
			Assert.AreEqual(2, counter);
		}
	}
}
