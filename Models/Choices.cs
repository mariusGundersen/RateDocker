using System.Collections.Generic;

namespace RateDocker.Models
{
	public class Choices
	{
		public IReadOnlyList<string> Names {
			get {
				return new List<string>{
					"Cool",
					"Really cool",
					"Really fricking cool!",
					"Fricking incredible!",
					"Mindblowingly awesome!"
				};
			}
		}
	}
}