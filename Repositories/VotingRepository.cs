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
            return new Votes
			{
				Results = Enumerable.Range(1,5).Select(x => new Vote(votes.Count(v => v == x), votes.Count)).ToList()
			};
		}
    }
}