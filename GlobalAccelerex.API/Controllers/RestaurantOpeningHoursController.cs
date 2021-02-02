using GlobalAccelerex.API.Data;
using GlobalAccelerex.API.Services;
using GlobalAccelerex.API.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GlobalAccelerex.API.Controllers
{
    /// <summary>
    /// Resource for Restaurant Opening Hours
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOpeningHoursController : ControllerBase
    {
        private IRestaurantOpeningHourService service;

        /// <summary>
        /// Defaulr consructor
        /// </summary>
        /// <param name="service"></param>
        public RestaurantOpeningHoursController(IRestaurantOpeningHourService service)
        {
            this.service = service;
        }

        // POST api/<RestaurantOpeningHoursController>
        [HttpPost]
        public ActionResult<Response<List<string>>> Post([FromBody] RestaurantOpeningHour model)
        {
            try
            {
                var response = service.ProcessRestaurantOpeningHours(model);
                if (response.Successful)
                {
                    return Ok(response);
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return BadRequest(Response<List<string>>.Failed("An unidentified error occured. Contact System Administrator"));
            }
        }
    }
}
