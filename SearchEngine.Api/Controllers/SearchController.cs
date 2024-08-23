using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace search.SearchEngine.Api.Controllers
{
    /// <summary>
    /// Controller responsible for handling search operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        /// <summary>
        /// Handles search requests.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> representing the result of the search operation.</returns>

        public IActionResult Search() {
      return Ok();
        }
    }
}