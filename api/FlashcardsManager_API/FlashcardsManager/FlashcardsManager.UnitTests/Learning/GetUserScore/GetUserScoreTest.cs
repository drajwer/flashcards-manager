using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using Xunit;

namespace FlashcardsManager.UnitTests.Learning.GetUserScore
{
    public class GetUserScoreForCategoryTest : LearningServiceTestBase
    {

        [Fact]
        public async Task NullUserTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            Assert.ThrowsAny<ArgumentException>( () =>
                 Service.GetUserScore(null));
        }

        [Fact]
        public async Task NoUserProgressesTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            Assert.Equal(0, Service.GetUserScore(user).SumPoints);
        }

        [Fact]
        public async Task AllCategoriesTest()
        {
            int count = 20;
            var categories = (await AddCategories(2));
            var flashcards1 = await AddFlashcards(categories[0], count);
            var flashcards2 = await AddFlashcards(categories[1], count);
            var user = (await AddUsers()).First();
            var progresses1 = await AddUserProgress(flashcards1, new[] {user});
            var progresses2 = await AddUserProgress(flashcards2, new[] { user });
            progresses1.ForEach(up => up.Progress = 1);
            progresses2.ForEach(up => up.Progress = 2);

            int sum = count * 3;
            Assert.Equal(sum, Service.GetUserScore(user).SumPoints);
        }
    }
}
