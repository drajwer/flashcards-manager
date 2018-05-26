using System;
using System.Linq;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlashcardsManager.IntegrationTests.RepostitoryTests.Update
{
    [TestClass]
    public class UpdateTest : TestBase
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
                entity.KeyDescription = "test";
                repository.Update(entity);
                dbContext.SaveChanges();

            }

            using (var dbContext = CreateContext())
            {
                Assert.IsTrue(dbContext.Flashcards.Any(e => e.KeyDescription == entity.KeyDescription));
            }
        }

        

        [TestMethod]
        public void CategoryTest()
        {
            Category entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Categories.First();
                entity.Name = "integration-test-category";
                var repository = new Repository<Category>(dbContext);

                repository.Update(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsTrue(dbContext.Categories.Any(e => e.Name == entity.Name));
            }
        }

        

        [TestMethod]
        public void UserTest()
        {
            User entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Users.First();
                var repository = new Repository<User>(dbContext);
                entity.UserName = "integration-tests";
                repository.Update(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsTrue(dbContext.Users.Any(e => e.UserName == entity.UserName));
            }
        }

       

        [TestMethod]
        public void UserProgressTest()
        {
            UserProgress entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.UserProgress.First();
                entity.Progress = 100;
                var repository = new Repository<UserProgress>(dbContext);
                
                repository.Update(entity);
                dbContext.SaveChanges();
            }

            using (var dbContext = CreateContext())
            {
                Assert.IsTrue(dbContext.UserProgress.Any(e => e.UserId == entity.UserId && e.Progress == entity.Progress));
            }
        }
    }
}
