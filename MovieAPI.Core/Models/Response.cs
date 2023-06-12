using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models
{
    public class Response
    {
        public int Page { get; set; } = 1;
        public IEnumerable<Movie> Movies{ get; set; } = Enumerable.Empty<Movie>();

        public int TotalPages { get; set; } = 0;

        public int TotalResults { get; set; } = 0;
    }
}
