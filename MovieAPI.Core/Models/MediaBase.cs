using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAPI.Core.Models
{
    public abstract class MediaBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsAdult { get; set; }
        public string? PosterPath { get; set; }
        public string Overview { get; set; }

    }
}
