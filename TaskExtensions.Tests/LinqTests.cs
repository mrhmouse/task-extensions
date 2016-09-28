using System.Threading.Tasks;
using NUnit.Framework;
using TaskExtensions.Linq;

namespace TaskExtensions.Tests
{
	[TestFixture]
	public class LinqTests
	{
		[Test(Description = "LINQ 'select' syntax works")]
		public void Select_Syntax_Works()
		{
			var projected =
				from x in Task.FromResult(2)
				select x * 2;
			Assert.AreEqual(4, projected.Result);
		}

		[Test(Description = "LINQ chained 'select' syntax works")]
		public void Chained_Select_Syntax_Works()
		{
			var projected =
				from x in Task.FromResult(2)
				from y in Task.FromResult(x)
				select x * y;
			Assert.AreEqual(4, projected.Result);
		}

		[Test(Description = "LINQ nested 'select' syntax works")]
		public void Nested_Select_Syntax_Works()
		{
			var projected =
				from x in Task.FromResult(2)
				select
					(from y in Task.FromResult(3)
					 select x + y);
			Assert.AreEqual(5, projected.Result);
		}
	}
}
