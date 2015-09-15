using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Configuration.Client;
using RateDocker.Models;

namespace RateDocker.Repositories
{
    public interface IVotingRepository
	{
		Task Vote(int vote);
		
		Task<Votes> Votes();
	}
	
	public class VotingRepository : IVotingRepository
	{
    	private readonly Cluster _cluster;
		
		public VotingRepository(Cluster cluster){
			_cluster = cluster;
		}
		
		public async Task Vote(int vote){
			var document = new Document<List<int>>
			{
				Id = "Votes",
				Content = await GetVotes()
			};
			
			document.Content.Add(vote);
			
			using(var bucket = _cluster.OpenBucket()){
				await bucket.UpsertAsync(document);
			}
		}
		
		public async Task<Votes> Votes(){
			var choices = new Choices();
			var votes = await GetVotes();
			return new Votes
			{
				Results = Enumerable.Range(1,5).Select(x => new Vote(choices.Names[x-1], votes.Count(v => v == x), votes.Count)).ToList()
			};
		}
		
		private async Task<List<int>> GetVotes(){
			using(var bucket = _cluster.OpenBucket()){
				var result = await bucket.GetDocumentAsync<List<int>>("Votes");
				if(result.Success){
					return result.Content;
				}else {
					return new List<int>();
				}
			}
		}
    }
}