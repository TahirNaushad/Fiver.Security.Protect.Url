using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Fiver.Security.Protect.Url.Models;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using System;

namespace Fiver.Security.Protect.Url.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly IDataProtector protector;
        //private readonly ITimeLimitedDataProtector protector;

        public MoviesController(IDataProtectionProvider provider)
        {
            this.protector = provider.CreateProtector("protect_my_query_string");
            //this.protector = provider.CreateProtector("protect_my_query_string")
            //                         .ToTimeLimitedDataProtector();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var model = GetMovies(); // simulate call to repository
            
            var outputModel = model.Select(item => new
            {
                //item.Id,
                Id = this.protector.Protect(item.Id.ToString()),
                //Id = this.protector.Protect(item.Id.ToString(), TimeSpan.FromSeconds(10)),
                item.Title,
                item.ReleaseYear,
                item.Summary
            });

            return Ok(outputModel);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var orignalId = int.Parse(this.protector.Unprotect(id));

            var model = GetMovies(); // simulate call to repository
            
            var outputModel = model.Where(item => item.Id == orignalId);

            return Ok(outputModel);
        }

        #region " Private "

        public List<Movie> GetMovies()
        {
            return new List<Movie>
            {
                new Movie { Id = 1, Title = "Never Say Never Again", ReleaseYear = 1983, Summary = "A SPECTRE agent has stolen two American nuclear warheads, and James Bond must find their targets before they are detonated." },
                new Movie { Id = 2, Title = "Diamonds Are Forever ", ReleaseYear = 1971, Summary = "A diamond smuggling investigation leads James Bond to Las Vegas, where he uncovers an evil plot involving a rich business tycoon." },
                new Movie { Id = 3, Title = "You Only Live Twice ", ReleaseYear = 1967, Summary = "Agent 007 and the Japanese secret service ninja force must find and stop the true culprit of a series of spacejackings before nuclear war is provoked." }
            };
        }

        #endregion
    }
}
