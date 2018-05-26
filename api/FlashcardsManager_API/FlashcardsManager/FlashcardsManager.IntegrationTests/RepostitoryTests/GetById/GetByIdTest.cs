using System;
using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlashcardsManager.IntegrationTests.RepostitoryTests.GetById
{
    [TestClass]
    public class GetByIdTest : TestBase
    {
        [TestMethod]
        public async Task NullArgumentTest()
        {
            using (var dbContext = CreateContext())
            {
                var flashcardRepository = new Repository<Flashcard>(dbContext);
                await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () => await flashcardRepository.GetById(null));
            }
        }

        [TestMethod]
        public async Task NoArgumentTest()
        {
            using (var dbContext = CreateContext())
            {
                var flashcardRepository = new Repository<Flashcard>(dbContext);
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await flashcardRepository.GetById());
            }
        }

        [TestMethod]
        public async Task WrongKeyTest()
        {
            using (var dbContext = CreateContext())
            {
                var repository = new Repository<UserProgress>(dbContext);
                var flashcard = dbContext.Flashcards.First();
                await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await repository.GetById(flashcard.Id));
            }
        }

        [TestMethod]
        public async Task FlashcardTest()
        {
            Flashcard entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Flashcards.First();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<Flashcard>(dbContext);
                var getByIdEntity = await repository.GetById(entity.Id);
                Assert.AreEqual(entity.Key, getByIdEntity.Key);
            }
        }

        

        [TestMethod]
        public async Task CategoryTest()
        {
            Category entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Categories.First();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<Category>(dbContext);
                var getByIdEntity = await repository.GetById(entity.Id);
                Assert.AreEqual(entity.Name, getByIdEntity.Name);
            }
        }

        

        [TestMethod]
        public async Task UserTest()
        {
            User entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.Users.First();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<User>(dbContext);
                var getByIdEntity = await repository.GetById(entity.Id);
                Assert.AreEqual(entity.UserName, getByIdEntity.UserName);
            }
        }

       

        [TestMethod]
        public async Task UserProgressTest()
        {
            UserProgress entity;
            using (var dbContext = CreateContext())
            {
                entity = dbContext.UserProgress.First();
            }

            using (var dbContext = CreateContext())
            {
                var repository = new Repository<UserProgress>(dbContext);
                var getByIdEntity = await repository.GetById(entity.UserId, entity.FlashcardId);
                Assert.AreEqual(entity.Progress, getByIdEntity.Progress);
            }
        }
    }
}
