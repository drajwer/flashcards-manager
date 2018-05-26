using System;
using System.Linq;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlashcardsManager.IntegrationTests.RepostitoryTests.Delete
{
    [TestClass]
    public class DeleteTest : TestBase
    {
        [TestMethod]
        public void NullArgumentTest()
        {
            using (var dbContext = CreateContext())
            {
                var flashcardRepository = new Repository<Flashcard>(dbContext);
                Assert.ThrowsException<ArgumentNullException>(() => flashcardRepository.Delete(null));
            }
        }

        [TestMethod]
        public void FlashcardTest()
        {
            Flashcard entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Flashcards.First();
                var repository = new Repository<Flashcard>(dbContext);

                repository.Delete(entity);
                dbContext.SaveChanges();

            }

            using (var dbContext = CreateContext())
            {
                Assert.IsFalse(dbContext.Flashcards.Any(e => e.Key == entity.Key));
            }
        }

        

        [TestMethod]
        public void CategoryTest()
        {
            Category entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Categories.First();
                var repository = new Repository<Category>(dbContext);

                repository.Delete(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsFalse(dbContext.Categories.Any(e => e.Name == entity.Name));
            }
        }

        

        [TestMethod]
        public void UserTest()
        {
            User entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Users.Last(u => u.UserName != "janusz");
                var repository = new Repository<User>(dbContext);

                repository.Delete(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsFalse(dbContext.Users.Any(e => e.UserName == entity.UserName));
            }
        }

       

        [TestMethod]
        public void UserProgressTest()
        {
            UserProgress entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.UserProgress.First();
                var repository = new Repository<UserProgress>(dbContext);

                repository.Delete(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsFalse(dbContext.UserProgress.Any(e => e.UserId == entity.UserId && e.FlashcardId == entity.FlashcardId));
            }
        }
    }
}
