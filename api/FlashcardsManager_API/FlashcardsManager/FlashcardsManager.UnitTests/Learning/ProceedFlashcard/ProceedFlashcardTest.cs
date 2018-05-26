using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using Xunit;

namespace FlashcardsManager.UnitTests.Learning.ProceedFlashcard
{
    public class ProceedFlashcardTest : LearningServiceTestBase
    {
        public ProceedFlashcardTest()
        {
            SetDefaultOpts();
        }

        [Fact]
        public async Task NullUserTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
                await Service.ProceedFlashcard(flashcards[0].Id, null, FlashcardResult.Success));
        }

        [Fact]
        public async Task NullCategoryTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
                await Service.ProceedFlashcard(-100, user, FlashcardResult.Success));
        }

        [Theory]
        [InlineData(FlashcardResult.Success)]
        [InlineData(FlashcardResult.Fail)]
        [InlineData(FlashcardResult.Partial)]
        public async Task NewUserProgressTest(FlashcardResult result)
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            await Service.ProceedFlashcard(flashcards[0].Id, user, result);

            var userProgresses = UnitOfWork.UserProgressRepository.GetAll().ToList();
            Assert.Equal(userProgresses.Count, 1);
            Assert.Contains(userProgresses, up => up.FlashcardId == flashcards[0].Id);
            int expectedProgress = CalculateExpectedProgressForNew(result);
            Assert.Equal(userProgresses[0].Progress, expectedProgress);

        }

        [Theory]
        [InlineData(FlashcardResult.Success)]
        [InlineData(FlashcardResult.Fail)]
        [InlineData(FlashcardResult.Partial)]
        public async Task ModifyActualUserProgressTest(FlashcardResult result)
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var progress = (await AddUserProgress(new[] { flashcards[0] }, new[] { user })).First();
            int oldValue = result == FlashcardResult.Fail ? Options.MaxProgress : Options.MinProgress;
            progress.Progress = oldValue;
            await Service.ProceedFlashcard(flashcards[0].Id, user, result);

            var userProgresses = UnitOfWork.UserProgressRepository.GetAll().ToList();
            Assert.Equal(userProgresses.Count, 1);
            Assert.Contains(userProgresses, up => up.FlashcardId == flashcards[0].Id);
            int expectedProgress = CalculateExpectedProgressForNew(result) + oldValue;
            Assert.Equal(userProgresses[0].Progress, expectedProgress);

        }


        [Theory]
        [InlineData(FlashcardResult.Success)]
        [InlineData(FlashcardResult.Fail)]
        [InlineData(FlashcardResult.Partial)]
        public async Task NoOverflowTest(FlashcardResult result)
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var progress = (await AddUserProgress(new[] { flashcards[0] }, new[] { user })).First();
            int oldValue = result == FlashcardResult.Fail ? Options.MinProgress : Options.MaxProgress;
            progress.Progress = oldValue;
            await Service.ProceedFlashcard(flashcards[0].Id, user, result);

            var userProgresses = UnitOfWork.UserProgressRepository.GetAll().ToList();
            Assert.Equal(userProgresses.Count, 1);
            Assert.Contains(userProgresses, up => up.FlashcardId == flashcards[0].Id);
            int expectedProgress = oldValue;
            Assert.Equal(userProgresses[0].Progress, expectedProgress);

        }

        private int CalculateExpectedProgressForNew(FlashcardResult result)
        {
            switch (result)
            {
                case FlashcardResult.Success:
                    return Options.OnSuccess;
                case FlashcardResult.Fail:
                    return Options.OnFailure;
                case FlashcardResult.Partial:
                    return Options.OnPartial;
            }
            throw new ArgumentException(nameof(result));
        }

    }
}
