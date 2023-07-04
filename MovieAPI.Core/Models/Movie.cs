using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models
{
    public class Movie : MediaBase
    {
        public string OriginalTitle { get; set; } = string.Empty;
        public string? ReleaseDate { get; set; }
        public string Title { get; set; } = string.Empty;
    }
}
