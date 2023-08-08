using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models
{
    public class Response<T> where T : MediaBase
    {
        public int Page { get; set; } = 1;
        public IEnumerable<T> Movies{ get; set; } = Enumerable.Empty<T>();

        public int TotalPages { get; set; } = 0;

        public int TotalResults { get; set; } = 0;
    }
}
