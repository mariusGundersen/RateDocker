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
				Ones = votes.Where(v => v == 1).Count(),
				Twos = votes.Where(v => v == 2).Count(),
				Threes = votes.Where(v => v == 3).Count(),
				Fours = votes.Where(v => v == 4).Count(),
				Fives = votes.Where(v => v == 5).Count()
			};
		}
	}
	
	public class Votes
	{
		public int Ones { get; set; }
		
		public int Twos { get; set; }
		
		public int Threes { get; set; }
		
		public int Fours { get; set; }
		
		public int Fives { get; set; }
	}
}