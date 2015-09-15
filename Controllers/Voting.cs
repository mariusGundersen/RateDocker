using System.Collections.Generic;
using System.Linq;
namespace RateDocker.Controllers
{
	public static class Voting
	{
		private static IList<int> votes = new List<int>();
		
		public static void Vote(int vote){
			votes.Add(vote);
		}
		
		public static Votes Votes(){
            return new Votes
			{
				Results = Enumerable.Range(1,5).Select(x => new Vote(votes.Count(v => v == x), votes.Count)).ToList()
			};
		}
	}
	
	public class Votes
	{
		public IReadOnlyCollection<Vote> Results {get; set;}
	}
	
	public class Vote
	{
		public Vote(int count, int total){
			Count = count;
			Percentage = total == 0 ? 0 : count*100m/total;
		}
		
		public int Count {get; set;}
		
		public decimal Percentage {get; set;}
	}
}