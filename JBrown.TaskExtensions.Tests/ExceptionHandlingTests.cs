using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace JBrown.TaskExtensions.Tests
{
	[TestFixture]
	public class ExceptionHandlingTests
	{
		[Test]
		public void Can_Handle_Exceptions()
		{
			Func<int> alwaysFails = () =>
			{
				throw new OverflowException();
			};

			var task = Task.Run(alwaysFails)
				.Except()
				.On<OverflowException>(() => 2);
			Assert.AreEqual(2, task.Result);
		}

		[Test]
		public void Other_Exceptions_Pass_Through()
		{
			Func<int> alwaysFails = () =>
			{
				throw new InvalidOperationException();
			};

			var task = Task.Run(alwaysFails)
				.Except()
				.On<OverflowException>(() => 2);
			Assert.Throws<AggregateException>(() => task.Wait());
		}

		[Test]
		public void Can_Replace_Canceled_Tasks()
		{
			var ct = new CancellationToken(canceled: true);
			var canceled = Task.Run(() => 1, ct);
			var task = canceled
				.Except()
				.WhenCanceled(() => 2);
			Assert.AreEqual(2, task.Result);
		}
	}
}
