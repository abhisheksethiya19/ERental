using ERental.BL;
using ERental.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERental.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityStateWebAPIController : ControllerBase
    {
        private readonly CityStateBL cityStateBL = new CityStateBL();
        public CityStateWebAPIController()
        {

        }

        [HttpGet]
        [Route("City")]
        public ActionResult<IEnumerable<City>> GetCity()
        {
            return new ActionResult<IEnumerable<City>>(cityStateBL.GetCity());
        }

        [HttpGet]
        [Route("State")]
        public ActionResult<IEnumerable<State>> GetState()
        {
            return new ActionResult<IEnumerable<State>>(cityStateBL.GetState());
        }
    }
}
