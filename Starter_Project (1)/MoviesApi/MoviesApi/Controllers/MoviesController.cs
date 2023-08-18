using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Repositories;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository repository)
        {
            _movieRepository = repository;
        }

        //Implement controller methods here
        // GET: api/Movies
        [HttpGet]
        public List<Movie> GetMovies()
        {
            return _movieRepository.GetMovies();
        }

        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovieById(int id)
        {
            Movie movie = _movieRepository.GetMovieById(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        public IActionResult InsertMovie(Movie movie)
        {
            bool isSuccess = _movieRepository.InsertMovie(movie);

            if (isSuccess)
            {
                return Ok("Movie inserted successfully.");
            }
            else
            {
                return BadRequest("Failed to insert movie. Try again!");
            }
        }

        [HttpGet("exists/{id}")]
        public IActionResult MovieExists(int id)
        {
            bool exists = _movieRepository.MovieExists(id);

            if (exists)
            {
                return Ok("Movie exists.");
            }
            else
            {
                return NotFound("Movie not found.");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteMovie(int id)
        {
            try
            {
                bool deleted = _movieRepository.DeleteMovie(id);

                if (deleted)
                {
                    return Ok(true); 
                }
                else
                {
                    return NotFound(); 
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine("An error occurred: " + ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }

}

