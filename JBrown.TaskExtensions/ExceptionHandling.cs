using System;
using System.Threading.Tasks;

namespace JBrown.TaskExtensions
{
	public static class ExceptionHandling
	{
		public static ExceptionHandler<TResult> Except<TResult>(
			this Task<TResult> source)
		{
			return new ExceptionHandler<TResult>(source);
		}
	}

	public class ExceptionHandler<TResult>
	{
		private readonly Task<TResult> Source;
		public ExceptionHandler(Task<TResult> source)
		{
			Source = source;
		}

		public Task<TResult> WhenCanceled(Func<TResult> replacement)
		{
			return Source.ContinueWith(s =>
			{
				if (s.IsCanceled)
				{
					return replacement();
				}

				return s.Result;
			});
		}

		public Task<TResult> WhenCanceled(Func<Task<TResult>> replacement)
		{
			return Source.ContinueWith(s =>
			{
				if (s.IsCanceled)
				{
					return replacement();
				}

				return s;
			}).Unwrap();
		}

		public Task<TResult> On<TException>(Func<Task<TResult>> replacement)
			where TException : Exception
		{
			return Source.ContinueWith(s =>
			{
				if (s.Exception != null
					&& s.Exception.InnerException is TException)
				{
						return replacement();
				}

				return s;
			}).Unwrap();
		}

		public Task<TResult> On<TException>(Func<TResult> replacement)
			where TException : Exception
		{
			return Source.ContinueWith(s =>
			{
				if (s.Exception != null
					&& s.Exception.InnerException is TException)
				{
					return replacement();
				}

				return s.Result;
			});
		}
	}
}
