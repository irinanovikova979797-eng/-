// Movie.cs 
using System;

namespace SearchAlgorithmsDemo.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public string Director { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }

        public override string ToString()
        {
            return $"{Title} ({ReleaseYear}) - {Director} - ★{Rating}";
        }
    }
}
