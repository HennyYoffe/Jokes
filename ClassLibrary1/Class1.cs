using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace ClassLibrary1
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<UserLikeJoke> LikeJokes { get; set; }
    }
    public class Joke
    {
        public int? Id { get; set; }  
        public int JokeDataId { get; set; }
        public string Setup{ get; set; }
        public string Punchline { get; set; }
        public List<UserLikeJoke> LikeJokes { get; set; }

    }
    public class UserLikeJoke
    {
        public int JokeId { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public bool Like { get; set; }
        public Joke Joke { get; set; }
        public User User { get; set; }
    }
   
    public class JokesManager
    {
        private string _connectionString;

        public JokesManager(string connectionString)
        {
            _connectionString = connectionString;
        }      

        #region User
        public User GetUserById(int id)
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                return ctx.Users.Include(u=> u.LikeJokes).FirstOrDefault(u => u.Id == id);
            }
        }
        public User GetByEmail(string email)
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                return ctx.Users.Include(u => u.LikeJokes).FirstOrDefault(u => u.Email == email);
            }
        }
        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            bool isCorrectPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (isCorrectPassword)
            {
                return user;
            }

            return null;
        }

       

        public void AddUser(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            using (var ctx = new JokesContext(_connectionString))
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }
        #endregion
        #region Joke
        public Joke GetJoke()
        {
            var client = new HttpClient();
            string url = $"https://official-joke-api.appspot.com/jokes/programming/random";
            string json = client.GetStringAsync(url).Result;
            var result = JsonConvert.DeserializeObject<List<Joke>>(json);
            return result.FirstOrDefault();
        }
        public Joke GetJokeById(int id)
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                return ctx.Jokes.Include(u => u.LikeJokes).FirstOrDefault(u => u.Id == id);
            }
        }
        public void AddJoke(Joke joke)
        {            
            joke.JokeDataId = (int)joke.Id;
            joke.Id = null;
            using (var ctx = new JokesContext(_connectionString))
            {
                ctx.Jokes.Add(joke);
                ctx.SaveChanges();
            }
        }
        public List<Joke> GetJokes()
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                return ctx.Jokes.Include(u => u.LikeJokes).ToList();
            }
        }
        public int GetCountLikesOrDislike(bool like,int? jokeid)
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                return ctx.LikeJokes.Where(lj => lj.JokeId == jokeid).Count(lj => lj.Like == like);
            }
        }
        #endregion
        #region UserLikeJoke
        public void AddUserLikeJoke(UserLikeJoke likeJoke)
        {
            using (var ctx = new JokesContext(_connectionString))
            {
                ctx.LikeJokes.Add(likeJoke);
                ctx.SaveChanges();
            }
        }
        public List<UserLikeJoke> GetUserLikeJoke()
        {
            using (var ctx = new JokesContext(_connectionString))
            {
               return ctx.LikeJokes.ToList();
            }
        }

        public void UpdateLike(int userid, int jokeid, bool like)
        {           
                using (var context = new JokesContext(_connectionString))
                {
                    context.Database.ExecuteSqlCommand("UPDATE LikeJokes SET [Like] = @like WHERE JokeId = @jokeid And UserId = @userid",
                        new SqlParameter("@like", like),
                        new SqlParameter("@jokeid", jokeid),
                new SqlParameter("@userid", userid));
            }            
        }
        #endregion
    }
    public class JokesContext : DbContext
    {
        private string _connectionString;

        public JokesContext(string connectionString)
        {
            _connectionString = connectionString;
        }
       
        public DbSet<User> Users { get; set; }
        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserLikeJoke> LikeJokes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserLikeJoke>()
                .HasKey(ulj => new { ulj.JokeId, ulj.UserId });


            modelBuilder.Entity<UserLikeJoke>()
                .HasOne(ulj => ulj.Joke)
                .WithMany(j => j.LikeJokes)
                .HasForeignKey(j => j.JokeId);


            modelBuilder.Entity<UserLikeJoke>()
                .HasOne(ulj => ulj.User)
                .WithMany(u => u.LikeJokes)
                .HasForeignKey(u => u.UserId);
        }
    }
    public class JokesContextFactory : IDesignTimeDbContextFactory<JokesContext>
    {
        public JokesContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}HW62_Api_Jokes_May12"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new JokesContext(config.GetConnectionString("ConStr"));
        }
    }
}
