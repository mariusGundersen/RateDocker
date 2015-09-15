namespace RateDocker.Models
{
    public class Vote
	{
		public Vote(string name, int count, int total){
			Name = name;
			Count = count;
			Percentage = total == 0 ? 0 : count*100m/total;
		}
		
		public string Name { get; set; }
		
		public int Count {get; set;}
		
		public decimal Percentage {get; set;}
	}
}