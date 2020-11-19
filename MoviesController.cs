using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Movie_API
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        // GET: api/Movies/5
        [HttpGet("{id}", Name = "Get")]
        public IEnumerable<Movie> Get(int id)
        {
            return DataAccess.GetMovie(id).ToArray();
        }

        // POST: api/Movies
        [HttpPost]
        public void Post([FromBody] JsonElement value)
        {            
            DataAccess.SaveMovie(new Movie { 
                MovieId = Int32.Parse(value.GetProperty("movieId").ToString()),
                Title = value.GetProperty("title").ToString(),
                Language = value.GetProperty("language").ToString(),
                Duration = value.GetProperty("duration").ToString(),
                ReleaseYear = Int32.Parse(value.GetProperty("releaseYear").ToString())
            });
        }
    }
}
