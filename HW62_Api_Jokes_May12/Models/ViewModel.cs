using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW62_Api_Jokes_May12.Models
{
    public class IndexViewModel
    {
        public Joke Joke { get; set; }              
        public bool? WithinTime { get; set; }
        public bool LikedJoke { get; set; }
        
    }
    public class ViewJokeViewModel
    {
        public Joke Joke { get; set; }
        public int? Likes { get; set; }
        public int? Dislikes { get; set; }
    }
}
