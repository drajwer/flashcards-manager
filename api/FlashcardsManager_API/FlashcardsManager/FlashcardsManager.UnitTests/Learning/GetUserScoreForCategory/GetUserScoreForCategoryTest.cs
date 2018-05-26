using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FlashcardsManager.UnitTests.Learning.GetUserScoreForCategory
{
    public class GetUserScoreForCategoryTest : LearningServiceTestBase
    {

        [Fact]
        public async Task NullUserTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
                 await Service.GetUserScoreForCategory(null, category.Id));
        }

        [Fact]
        public async Task NoUserProgressesTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var score = await Service.GetUserScoreForCategory(user, category.Id);
            Assert.Equal(0, score.SumPoints);
        }

        [Fact]
        public async Task AllCategoriesTest()
        {
            int count = 20;
            var categories = (await AddCategories(2));
            var flashcards1 = await AddFlashcards(categories[0], count);
            flashcards1.ForEach(f => f.Category = categories[0]);
            var flashcards2 = await AddFlashcards(categories[1], count);
            flashcards2.ForEach(f => f.Category = categories[1]);

            var user = (await AddUsers()).First();
            var progresses1 = await AddUserProgress(flashcards1, new[] { user });
            var progresses2 = await AddUserProgress(flashcards2, new[] { user });
            progresses1.ForEach(up =>
            {
                up.Progress = 1;
                up.Flashcard = flashcards1.FirstOrDefault(f => f.Id == up.FlashcardId);
            });
            progresses2.ForEach(up =>
            {
                up.Progress = 2;
                up.Flashcard = flashcards2.FirstOrDefault(f => f.Id == up.FlashcardId);
            });

            int sum = count;
            var score = await Service.GetUserScoreForCategory(user, categories[0].Id);
            Assert.Equal(sum, score.SumPoints);
        }
    }
}
