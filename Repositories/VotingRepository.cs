using System.Collections.Generic;
using System.Linq;
using RateDocker.Models;

namespace RateDocker.Repositories
{
    public interface IVotingRepository
	{
		void Vote(int vote);
		
		Votes Votes();
	}
	
	public class VotingRepository : IVotingRepository
	{
		private static IList<int> votes = new List<int>();
		
		public void Vote(int vote){
			votes.Add(vote);
		}
		
		public Votes Votes(){
			var choices = new Choices();
			
            return new Votes
			{
				Results = Enumerable.Range(1,5).Select(x => new Vote(choices.Names[x-1], votes.Count(v => v == x), votes.Count)).ToList()
			};
		}
    }
}