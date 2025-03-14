
using Bean_API.Dtos;
using Bean_API.Models;
using Bean_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Bean_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CoffeeBeanController : ControllerBase
    {
        private readonly ILogger<CoffeeBeanController> _logger;
        private readonly ICoffeeBeanService _coffeeBeanService;

        public CoffeeBeanController(ILogger<CoffeeBeanController> logger, ICoffeeBeanService coffeeBeanService)
        {
            _logger = logger;
            _coffeeBeanService = coffeeBeanService;
        }

        #region Create

        [HttpPost]
        public async Task<IActionResult> CreateCoffeeBean([FromBody] CreateCoffeeBeanDto coffeeBean)
        {
            try
            {
                var createdCoffeeBean = await _coffeeBeanService.CreateCoffeeBean_Async(coffeeBean);
                var fullCoffeeBean = await _coffeeBeanService.GetCoffeeBean_ByID_Async(createdCoffeeBean.Id);
                return CreatedAtAction(nameof(CreateCoffeeBean), fullCoffeeBean); //201 Created with Full CoffeeBean model
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }

        [HttpPost("GenerateBeanOfTheDay")]
        public async Task<IActionResult> GenerateBeanOfTheDay()
        {
            try
            {
                var botd = await _coffeeBeanService.GenerateBeanOfTheDay();

                return CreatedAtAction(nameof(GenerateBeanOfTheDay), botd); //201 Created with BeanOfTheDay model
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }

        #endregion

        #region Read
       
        [HttpGet]
        public async Task<IActionResult> GetCoffeeBean_All_Async()
        {
            try
            {
                var coffeeBeans = await _coffeeBeanService.GetCoffeeBean_All_Async();

                if (coffeeBeans == null) return NotFound(); //404 if not found
                return Ok(coffeeBeans); //200 OK with updated object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetCoffeeBean_ByID_Async(string id)
        {
            try
            {
                var coffeeBean = await _coffeeBeanService.GetCoffeeBean_ByID_Async(id);

                if (coffeeBean == null) return NotFound(); //404 if not found
                return Ok(coffeeBean); //200 OK with updated object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetCoffeeBean_BySearch_Async([FromQuery] SearchCoffeeBeanDto search)
        {
            try
            {
                var coffeeBean = await _coffeeBeanService.GetCoffeeBean_BySearch_Async(search);

                if (coffeeBean == null) return NotFound(); //404 if not found
                return Ok(coffeeBean); //200 OK with updated object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }
        #endregion

        #region Update

        [HttpPut("id")]
        public async Task<IActionResult> UpdateCoffeeBean(string id, [FromBody] CreateCoffeeBeanDto coffeeBean)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState); //400 Bad Request if model validation fails

                var updatedCofeeBean = await _coffeeBeanService.UpdateCoffeeBean_ByID_Async(id, coffeeBean);
                if (updatedCofeeBean == null)
                    return NotFound(); //404 if not found

                return Ok(updatedCofeeBean); //200 OK with updated object
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }          
        }

        #endregion

        #region Delete

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteCoffeeBean(string id)
        {
            try 
            {
                var result = await _coffeeBeanService.DeleteCoffeeBean_ByID_Async(id);
                if (!result)
                    return NotFound(); //404 if not found

                return Ok("Deleted successfully"); //200 OK with message of deletion (I'm sending 200, instead of 204, to keep the response body consistent with the other endpoints)
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return StatusCode(500); //500 Internal Server Error if something goes wrong  
            }
        }

        #endregion
    }
}
