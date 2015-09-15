using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using RateDocker.Models;
using RateDocker.Repositories;

namespace RateDocker.Controllers
{
    public class HomeController : Controller
    {
        IVotingRepository _votingRepository;

        public HomeController(IVotingRepository votingRepository)
        {
            _votingRepository = votingRepository;
        }
        
        public IActionResult Index()
        {
            return View(new Choices());
        }

        public async Task<IActionResult> Results()
        {
            ViewData["Message"] = "Your application description page.";
            
            return View(await _votingRepository.Votes());
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
