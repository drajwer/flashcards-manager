using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlashcardsManager.IntegrationTests
{
    [TestClass]
    public class MigrationTest : TestBase
    {
        [TestMethod]
        public void MainTest()
        {
            using (var dbContext = CreateContext())
            {
                Assert.IsFalse(dbContext.Database.EnsureCreated());
                Assert.IsTrue(dbContext.Categories.Any());
                Assert.IsTrue(dbContext.Flashcards.Any());
                Assert.IsTrue(dbContext.Users.Any());
            }

        }
    }
}
