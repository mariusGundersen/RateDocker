namespace RateDocker.Models
{
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