using Microsoft.AspNetCore.Mvc;
using WebApp_Epimakhov_v4.Models;

namespace WebApp_Epimakhov_v4.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly WebApiDatabaseContext _context;

        public CarController(WebApiDatabaseContext context)
        {
            _context = context;
        }



        [HttpGet("{id}")]
        public IActionResult GetId([FromRoute]int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }



        [HttpGet]
        public IActionResult GetParamets([FromQuery] string? brand, [FromQuery] decimal? maxPrice)
        {
            var queryCar = _context.Cars.ToList();

            if (brand != null)
            {
                queryCar = queryCar.Where(car => car.Brand == brand).ToList();
            }

            if (maxPrice.HasValue)
            {
                queryCar = queryCar.Where(car => car.Price <= maxPrice.Value).ToList();
            }

            return Ok(queryCar);
        }


        [HttpPost]
        public IActionResult PostCreateCar([FromBody] Car car)
        {
            _context.Cars.Add(car);

            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetId),
                new
                {id = car.Id},
                car
                );

        }


        [HttpPut("{id}")]
        public IActionResult PutUpdateCar([FromRoute] int id, [FromBody] Car car)
        {
            if (id != car.Id)
            {
                return BadRequest();
            }

            var exCar = _context.Cars.Find(id);

            if (exCar == null)
            {
                return NotFound();
            }

            exCar.Brand = car.Brand;
            exCar.Model = car.Model;
            exCar.ReleaseYear = car.ReleaseYear;
            exCar.Price = car.Price;
            exCar.IsAvailable = car.IsAvailable;

            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult PatchUpdateCar([FromRoute]int id, [FromBody]ClassCar car)
        {
            var exCar = _context.Cars.Find(id);
            if (exCar == null)
            {
               return NotFound();
            }

            if (car.Brand != null)
            {
                exCar.Brand = car.Brand;
            }

            if (car.Model != null)
            {
                exCar.Model = car.Model;
            }

            if (car.ReleaseYear.HasValue)
            {
                exCar.ReleaseYear = car.ReleaseYear.Value;
            }

            if (car.Price.HasValue)
            {
                exCar.Price = car.Price.Value;
            }

            if (car.IsAvailable.HasValue)
            {
                exCar.IsAvailable = car.IsAvailable.Value;
            }

            _context.SaveChanges();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteCar([FromRoute] int id)
        {
            var deleteCar = _context.Cars.Find(id);
            
            if (deleteCar == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(deleteCar);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
