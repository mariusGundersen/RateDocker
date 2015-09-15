using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RateDocker.Repositories;

namespace RateDocker.Controllers
{
    public class VoteController : Controller
    {
        IVotingRepository _votingRepository;

        public VoteController(IVotingRepository votingRepository)
        {
            _votingRepository = votingRepository;
        }
        
		[HttpPost]
        public async Task<IActionResult> Index(int awesomeness)
        {
            await _votingRepository.Vote(awesomeness);
            return new RedirectToActionResult("Results", "Home", null);
        }
    }
}
