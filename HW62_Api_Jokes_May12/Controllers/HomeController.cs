using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HW62_Api_Jokes_May12.Models;
using ClassLibrary1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace HW62_Api_Jokes_May12.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private IHostingEnvironment _environment;

        public HomeController(IConfiguration configuration,
             IHostingEnvironment environment)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new JokesManager(_connectionString);
            IndexViewModel vm = new IndexViewModel();
            Joke joke = repo.GetJoke();
            List<Joke> jokes = repo.GetJokes();
            Joke test = jokes.FirstOrDefault(j => j.JokeDataId == joke.Id);
            if (test == null)
            {
                repo.AddJoke(joke);
            }
            else
            {
                joke.Id = test.Id;
                joke.JokeDataId = test.JokeDataId;
            }
            vm.Joke = joke;            
            return View(vm);
        }
        public void LikeJoke(int id)
        {
            var repo = new JokesManager(_connectionString);          
            UserLikeJoke ulj = new UserLikeJoke();
            ulj.JokeId = id;
            User user = repo.GetByEmail(User.Identity.Name);
            ulj.UserId = user.Id;
            ulj.Like = true;
            ulj.DateTime = DateTime.Now;
            List<UserLikeJoke> likeJokes = repo.GetUserLikeJoke();
         UserLikeJoke ul =   likeJokes.Where(lj => lj.JokeId == id).FirstOrDefault(lj => lj.UserId == user.Id);
            if(ul == null)
            {
                repo.AddUserLikeJoke(ulj);
            }
            
        }
        public void UpdateJoke(int id, bool like)
        {
            var repo = new JokesManager(_connectionString);
            User user = repo.GetByEmail(User.Identity.Name);
            repo.UpdateLike(user.Id, id, like);

        }
        public IActionResult ViewJokes()
        {
            var repo = new JokesManager(_connectionString);
            List<Joke> jokes = repo.GetJokes();
            List<ViewJokeViewModel> vm = new List<ViewJokeViewModel>();
            foreach(Joke joke in jokes)
            {
                vm.Add(new ViewJokeViewModel
                {
                    Joke = joke,
                    Likes = repo.GetCountLikesOrDislike(true, joke.Id),
                    Dislikes = repo.GetCountLikesOrDislike(false, joke.Id),
                });
            }
            return View(vm);
        }


    }
}
//Create an application that displays a list of random programming jokes to the user.
//    Use this api to get the random jokes:

//https://official-joke-api.appspot.com/jokes/programming/random

//This application will also have a login system where users can login/signup to the site.

//When a logged in user visits the site, they will see a like/dislike option next to the joke. 
//    If they've previously liked the joke, then the like button should be disabled, and the dislike should be enabled. 
//    If they've disliked it in the past, then the opposite should happen.
//    A user should only be able to change their mind within 10 minutes (or whatever time interval you choose).

//Finally, the application should have a page where it displays a list of all jokes ever displayed to any user,
//    with a count of how many likes and dislikes each joke has.

//Use Entity Framework for all the db code. As a guide for how to do the many to many relationship, see here:

//https://github.com/LITW06/QASite/blob/master/QASite.Data/QASiteContext.cs#L28

//There will be three tables in this application, Jokes, Users and UserLikedJokes.
//        The UserLikedJokes will have a UserId, JokeId, DateTime and a boolean Liked.

//Good luck!