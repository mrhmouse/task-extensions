using System;
using System.Threading.Tasks;

namespace TaskExtensions.Linq
{
	/// <summary>
	/// Provides 'select' methods for tasks, without async/await.
	/// </summary>
	public static class LinqForTasks
	{
		/// <summary>
		/// When the task has completed, project its result into a new task.
		/// </summary>
		/// <typeparam name="TSource">
		/// The underlying type of the source task.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// The type returned by the projection.
		/// </typeparam>
		/// <param name="source">
		/// The source task.
		/// </param>
		/// <param name="projection">
		/// The projection to make on the result of the task.
		/// </param>
		/// <returns>
		/// A new task.
		/// </returns>
		public static Task<TResult> Select<TSource, TResult>(
			this Task<TSource> source,
			Func<TSource, TResult> projection)
		{
			return source.ContinueWith(s => projection(s.Result));
		}

		/// <summary>
		/// When the task has completed, project its result into a new task,
		/// automatically unwrapping the nested tasks.
		/// </summary>
		/// <typeparam name="TSource">
		/// The underlying type of the source task.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// The underlying type of the task returned by the projection.
		/// </typeparam>
		/// <param name="source">
		/// The source task.
		/// </param>
		/// <param name="projection">
		/// The projection to make on the result of the task.
		/// </param>
		/// <returns>
		/// A new task.
		/// </returns>
		public static Task<TResult> Select<TSource, TResult>(
			this Task<TSource> source,
			Func<TSource, Task<TResult>> projection)
		{
			return source.ContinueWith(s => projection(s.Result)).Unwrap();
		}

		/// <summary>
		/// When the task has completed, project its result into a new task,
		/// unwrapping the nested tasks.
		/// </summary>
		/// <typeparam name="TSource">
		/// The underlying type of the source task.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// The underlying type of the task returned by the projection.
		/// </typeparam>
		/// <param name="source">
		/// The source task.
		/// </param>
		/// <param name="projection">
		/// The projection to make on the result of the task.
		/// </param>
		/// <returns>
		/// A new task.
		/// </returns>
		public static Task<TResult> SelectMany<TSource, TResult>(
			this Task<TSource> source,
			Func<TSource, Task<TResult>> projection)
		{
			return source.ContinueWith(s => projection(s.Result)).Unwrap();
		}

		/// <summary>
		/// When the initial task has completed, project its result into a new task.
		/// When the intermediate task has completed, project its result into a
		/// new task as well, unwrapping the nested tasks.
		/// </summary>
		/// <typeparam name="TSource">
		/// The underlying type of the source task.
		/// </typeparam>
		/// <typeparam name="TMiddle">
		/// The underlying type of the task returned by the intermediate projection.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// The type returned by the projection.
		/// </typeparam>
		/// <param name="source">
		/// The source task.
		/// </param>
		/// <param name="intermediate">
		/// The intermediate projection.
		/// </param>
		/// <param name="projection">
		/// The final projection to make on the result of the task.
		/// </param>
		/// <returns>
		/// A new task.
		/// </returns>
		public static Task<TResult> SelectMany<TSource, TMiddle, TResult>(
			this Task<TSource> source,
			Func<TSource, Task<TMiddle>> intermediate,
			Func<TSource, TMiddle, TResult> projection)
		{
			return source
				.SelectMany(s => intermediate(s).Select(i => projection(s, i)));
		}

		/// <summary>
		/// When the initial task has completed, project its result into a new task.
		/// When the intermediate task has completed, project its result into a
		/// new task as well, automatically unwrapping the nested tasks.
		/// </summary>
		/// <typeparam name="TSource">
		/// The underlying type of the source task.
		/// </typeparam>
		/// <typeparam name="TMiddle">
		/// The underlying type of the task returned by the intermediate projection.
		/// </typeparam>
		/// <typeparam name="TResult">
		/// The underlying type of the task returned by the projection.
		/// </typeparam>
		/// <param name="source">
		/// The source task.
		/// </param>
		/// <param name="intermediate">
		/// The intermediate projection.
		/// </param>
		/// <param name="projection">
		/// The final projection to make on the result of the task.
		/// </param>
		/// <returns>
		/// A new task.
		/// </returns>
		public static Task<TResult> SelectMany<TSource, TMiddle, TResult>(
			this Task<TSource> source,
			Func<TSource, Task<TMiddle>> intermediate,
			Func<TSource, TMiddle, Task<TResult>> projection)
		{
			return source
				.SelectMany(s => intermediate(s).Select(i => projection(s, i)));
		}
	}
}
