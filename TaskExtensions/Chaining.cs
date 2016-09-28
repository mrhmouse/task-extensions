using System;
using System.Threading.Tasks;

namespace TaskExtensions
{
	public static class Chaining
	{
		public static Task Then(this Task source, Action afterwards)
		{
			return source.ContinueWith(s =>
			{
				s.Wait();
				afterwards();
			});
		}

		public static Task Then(this Task source, Func<Task> afterwards)
		{
			return source.ContinueWith(s =>
			{
				s.Wait();
				return afterwards();
			});
		}

		public static Task<T> Then<T>(this Task source, Func<T> afterwards)
		{
			return source.ContinueWith(s =>
			{
				s.Wait();
				return afterwards();
			});
		}

		public static Task<T> Then<T>(this Task source, Func<Task<T>> afterwards)
		{
			return source.ContinueWith(s =>
			{
				s.Wait();
				return afterwards();
			}).Unwrap();
		}
	}
}
