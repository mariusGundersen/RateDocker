using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace RateDocker.Controllers
{
    public class VoteController : Controller
    {
		[HttpPost]
        public IActionResult Index([FromBody]int awesomeness)
        {
            Voting.Vote(awesomeness);
            return new RedirectToActionResult("Results", "Home", null);
        }
    }
}
