using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TaskExtensions.Tests
{
	[TestFixture]
	public class AggregationTests
	{
		[Test]
		public void Canceled_Tasks_Cant_Be_Concatenated()
		{
			var ct = new CancellationToken(canceled: true);
			var one = Task.Run(() => 1, ct);
			var two = Task.FromResult(2);
			var pair = one.And(two);
			var exception = Assert.Catch<AggregateException>(() => pair.Wait());
			Assert.IsInstanceOf<TaskCanceledException>(exception.InnerException);
		}

		[Test]
		public void Tasks_Can_Be_Concatenated()
		{
			var one = Task.FromResult(1);
			var two = Task.FromResult(2);
			var sum =
				from pair in one.And(two)
				select pair[0] + pair[1];
			Assert.AreEqual(3, sum.Result);
		}

		[Test]
		public void Task_Lists_Can_Be_Appended_To()
		{
			var nums = Task.FromResult(new[] { 1, 2, 3, 4 });
			var last = Task.FromResult(5);
			var task = nums.And(last);
			Assert.AreEqual(5, task.Result.Last());
		}

		[Test]
		public void Task_Lists_Of_Lists_Can_Be_Appended_To()
		{
			var nums = Task.FromResult(new[] {
				new[] { 1, 2, 3, 4 }
			});
			var last = Task.FromResult(new[] { 5 });
			var task = nums.And(last);
			Assert.AreEqual(5, task.Result.Last().Last());
		}

		[Test]
		public void Task_Lists_Of_Lists_Can_Be_Concatenated()
		{
			var nums = Task.FromResult(new[] {
				new[] { 1, 2, 3, 4 }
			});
			var last = Task.FromResult(new[] {
				new[] { 5 }
			});
			var task = nums.And(last);
			Assert.AreEqual(5, task.Result.Last().Last());
		}

		[Test]
		public void Task_Lists_Can_Be_Concatenated()
		{
			var nums = Task.FromResult(new[] { 1, 2, 3, 4 });
			var tail = Task.FromResult(new[] { 5, 6, 7, 8 });
			var task = nums.And(tail);
			Assert.AreEqual(8, task.Result.Last());
		}
	}
}
