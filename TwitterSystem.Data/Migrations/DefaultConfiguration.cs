namespace TwitterSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class DefaultConfiguration : DbMigrationsConfiguration<TwitterDbContext>
    {
        public DefaultConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TwitterDbContext context)
        {
            if (!context.Users.Any())
            {
                this.CreateAdmin(context);
                this.AddData(context);
            }
        }

        private void CreateAdmin(TwitterDbContext context)
        {
            var adminUserName = "admin";
            var adminEmail = "admin@admin.bg";
            var adminFullName = "System Administrator";
            var adminPass = "admin";
            var adminRole = "Administrator";

            var adminUser = new User()
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail,
            };

            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            var userCreateResult = userManager.Create(adminUser, adminPass);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create the "Administrator" role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            // Add the "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }

        private void AddData(TwitterDbContext context)
        {
            var tweet1 = new Tweet
             {
                 TweetedAt = DateTime.Now,
                 Text = "Нямам нищо против съседите си, просто не харесвам адреса, на който живеят.",
                 RetweetCount = 0,
                 SharedCount = 0,
                 Owner = context.Users.First()
             };

            var tweet2 = new Tweet
            {
                TweetedAt = DateTime.Now,
                Text = "“Любовта не се търси, тя сама те намира.” Гинка, 48 годишна с 9 котки...",
                RetweetCount = 0,
                SharedCount = 0,
                Owner = context.Users.First()
            };

            var tweet3 = new Tweet()
            {
                TweetedAt = DateTime.Now,
                Text = "Нямам търпение да стана пенсионер, за да ставам рано и да опипвам хляба в магазина.",
                RetweetCount = 0,
                SharedCount = 0,
                Owner = context.Users.First()
            };

            var tweet4 = new Tweet()
            {
                TweetedAt = DateTime.Now,
                Text = "Всеки път като видя гоблен си казвам ебати ниската резолюция.",
                RetweetCount = 0,
                SharedCount = 0,
                Owner = context.Users.First()
            };

            var tweet5 = new Tweet()
            {
                TweetedAt = DateTime.Now,
                Text = "Имаш лице за радио водещ",
                RetweetCount = 0,
                SharedCount = 0,
                Owner = context.Users.First()
            };

            context.Tweets.Add(tweet1);
            context.Tweets.Add(tweet2);
            context.Tweets.Add(tweet3);
            context.Tweets.Add(tweet4);
            context.Tweets.Add(tweet5);
        }
    }
}
