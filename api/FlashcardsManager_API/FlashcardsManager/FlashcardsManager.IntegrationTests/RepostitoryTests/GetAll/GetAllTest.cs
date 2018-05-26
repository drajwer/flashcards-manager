using System;
using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlashcardsManager.IntegrationTests.RepostitoryTests.GetAll
{
    [TestClass]
    public class GetAllTest : TestBase
    {

        [TestMethod]
        public async Task FlashcardTest()
        {
            Flashcard entity;
            using (var dbContext = CreateContext())
            {
                var category = dbContext.Categories.First();
                entity = CreateTestFlashcard(category);
                var repository = new Repository<Flashcard>(dbContext);

                await repository.Add(entity);
                dbContext.SaveChanges();

            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<Flashcard>(dbContext);
                Assert.IsTrue(repository.GetAll().Any(e => e.Key == entity.Key));
            }
        }

        

        [TestMethod]
        public async Task CategoryTest()
        {
            Category entity;
            using (var dbContext = CreateContext())
            {
                entity = CreateTestCategory();
                var repository = new Repository<Category>(dbContext);

                await repository.Add(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<Category>(dbContext);
                Assert.IsTrue(repository.GetAll().Any(e => e.Name == entity.Name));
            }
        }

        

        [TestMethod]
        public async Task UserTest()
        {
            User entity;
            using (var dbContext = CreateContext())
            {
                entity = CreateTestUser();
                var repository = new Repository<User>(dbContext);

                await repository.Add(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<User>(dbContext);
                Assert.IsTrue(repository.GetAll().Any(e => e.UserName == entity.UserName));
            }
        }

       

        [TestMethod]
        public async Task UserProgressTest()
        {
            UserProgress entity;
            using (var dbContext = CreateContext())
            {
                var category = dbContext.Categories.First();
                var flashcard = CreateTestFlashcard(category);
                var user = CreateTestUser();
                dbContext.Flashcards.Add(flashcard);
                dbContext.Users.Add(user);
                entity = new UserProgress(user, flashcard);
                var repository = new Repository<UserProgress>(dbContext);

                await repository.Add(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<UserProgress>(dbContext);
                Assert.IsTrue(repository.GetAll().Any(e => e.UserId == entity.UserId && e.FlashcardId == entity.FlashcardId));
            }
        }
    }
}
