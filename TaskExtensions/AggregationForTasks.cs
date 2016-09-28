using System.Linq;
using System.Threading.Tasks;
using TaskExtensions.Linq;

namespace TaskExtensions
{
	public static class AggregationForTasks
	{
		public static Task<T[]> And<T>(
			this Task<T> source,
			Task<T> other)
		{
			return Task.WhenAll(source, other);
		}

		public static Task<T[]> And<T>(
			this Task<T[]> source,
			Task<T> other)
		{
			return
				from list in source
				from tail in other
				select list.Concat(new[] { tail }).ToArray();
		}

		public static Task<T[]> And<T>(
			this Task<T[]> source,
			Task<T[]> other)
		{
			return
				from list in source
				from tail in other
				select list.Concat(tail).ToArray();
		}
	}
}
