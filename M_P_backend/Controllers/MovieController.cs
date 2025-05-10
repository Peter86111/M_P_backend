using M_P_backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Digests;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace M_P_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpGet("/api/{name}")]
        public IActionResult Get(string name)
        {
            try
            {
                using (var cx = new CinemadbContext())
                {
                    var response = cx.Actors
                        .Include(m => m.Movies)
                        .FirstOrDefault(a => a.ActorName == name);

                    if (response != null)
                    {
                        return StatusCode(200, response);
                    }
                    else
                    {
                        return StatusCode(404);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/GetAllMovies")]
        public IActionResult GetAllMovies()
        {
            try
            {
                using (var cx = new CinemadbContext())
                {
                    var response = cx.Movies
                        .ToList();

                    if (response != null)
                    {
                        return StatusCode(200, response);
                    }
                    else
                    {
                        return StatusCode(400);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/GetAllFilmType")]
        public IActionResult GetAllFilmType()
        {
            try
            {
                using (var cx = new CinemadbContext())
                {
                    var response = cx.FilmTypes
                        .Include(m => m.Movies)
                        .ToList();

                    if (response != null)
                    {
                        return StatusCode(200, response);
                    }
                    else
                    {
                        return StatusCode(400);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/GetAllActors")]
        public IActionResult GetAllActors()
        {
            try
            {
                using (var cx = new CinemadbContext())
                {
                    var response = JsonSerializer.Serialize(cx.Actors.Count());

                    if (response != null)
                    {
                        return StatusCode(200, $"Színészek száma: {response}");
                    }
                    else
                    {
                        return StatusCode(400, "Nem lehet csatlakozni az adatbázishoz!");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/{UID}")]
        public IActionResult New(string UID, Movie movie)
        {
            try
            {
                if (UID == Program.UID)
                {
                    movie.MovieId = 0;

                    using (var cx = new CinemadbContext())
                    {
                        cx.Movies.Add(movie);
                        cx.SaveChanges();
                    }
                    return StatusCode(201, "Film hozzáadása sikeresen megtörtént.");
                }
                else
                {
                    return StatusCode(401, "Nincs jogosultsága új film felvételéhez!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

    }
}
