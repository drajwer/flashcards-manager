using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using Xunit;

namespace FlashcardsManager.UnitTests.Learning.GetFlashcards
{
    public class GetFlashcardsTest : LearningServiceTestBase
    {
        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        public async Task AllCollectionReturnedTest(int serviceCount)
        {
            int count = 20;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var flashcardsFromService = await Service.GetFlashcards(user, category.Id, FlashcardsSearchCriterionEnum.All, serviceCount);
            int realCount = Math.Min(serviceCount, count);
            Assert.Equal(realCount, flashcardsFromService.Count);
        }

        [Fact]
        public async Task NullUserTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            await AddFlashcards(category, count);
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await Service.GetFlashcards(null, category.Id, FlashcardsSearchCriterionEnum.All, count));
        }

        [Fact]
        public async Task NullCategoryTest()
        {
            int count = 20;
            var category = (await AddCategories()).First();
            await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            await Assert.ThrowsAnyAsync<ArgumentException>(async () => await Service.GetFlashcards(user, count*2, FlashcardsSearchCriterionEnum.All, count));
        }

        [Fact]
        public async Task OnlySelectedCategoryReturned()
        {
            int count = 20;
            var categories = (await AddCategories(2));
            var category = categories.First();
            var flashcards = await AddFlashcards(category, count);
            var otherFlashcards = await AddFlashcards(categories[1], count);

            var user = (await AddUsers()).First();
            var flashcardsFromService = await Service.GetFlashcards(user, category.Id, FlashcardsSearchCriterionEnum.All, count);
            Assert.Equal(count, flashcardsFromService.Count);
            Assert.DoesNotContain(flashcardsFromService, f => otherFlashcards.Contains(f));
        }

        [Theory]
        [InlineData(FlashcardsSearchCriterionEnum.New)]
        [InlineData(FlashcardsSearchCriterionEnum.Old)]

        public async Task NewOrOldReturnedTest(FlashcardsSearchCriterionEnum criterion)
        {
            int count = 20;
            int oldCount = 2;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var progress = await AddUserProgress(flashcards.Take(oldCount), new[] { user });
            user.UserProgress = progress;
            flashcards.ForEach(f => f.UserProgress = new List<UserProgress>());
            flashcards[0].UserProgress = progress.Where(up => up.FlashcardId == flashcards[0].Id).ToList();
            flashcards[1].UserProgress = progress.Where(up => up.FlashcardId == flashcards[1].Id).ToList();
            var flashcardsFromService = await Service.GetFlashcards(user, category.Id, criterion, count);
            if (criterion == FlashcardsSearchCriterionEnum.New)
            {
                int realCount = count - oldCount;
                Assert.Equal(realCount, flashcardsFromService.Count);
                Assert.DoesNotContain(flashcards[0], flashcardsFromService);
                Assert.DoesNotContain(flashcards[1], flashcardsFromService);
            }
            else
            {
                Assert.Equal(oldCount, flashcardsFromService.Count);
                Assert.Contains(flashcards[0], flashcardsFromService);
                Assert.Contains(flashcards[1], flashcardsFromService);
            }

        }


        [Theory]
        [InlineData(FlashcardsSearchCriterionEnum.Hard)]
        [InlineData(FlashcardsSearchCriterionEnum.Known)]
        public async Task HardReturnedTest(FlashcardsSearchCriterionEnum criterion)
        {
            int count = 20;
            int properCount = 2;
            SetDefaultOpts();
            int progressValue = criterion == FlashcardsSearchCriterionEnum.Hard ? Options.MaxHardProgress : Options
                .MinKnownProgress;
            int oppositeProgressValue = criterion == FlashcardsSearchCriterionEnum.Known ? Options.MaxHardProgress : Options
                .MinKnownProgress;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var progress = await AddUserProgress(flashcards.Take(properCount), new[] { user });
            var oppositeProgress = await AddUserProgress(flashcards.Skip(properCount + 1), new[] { user });

            progress.ForEach(p => p.Progress = progressValue);
            oppositeProgress.ForEach(p => p.Progress = oppositeProgressValue);
            user.UserProgress = new List<UserProgress>();
            foreach (var userProgress in oppositeProgress)
            {
                foreach (var flashcard in flashcards)
                {
                    if (userProgress.FlashcardId == flashcard.Id)
                        flashcard.UserProgress = new List<UserProgress> { userProgress };
                }
                user.UserProgress.Add(userProgress);
            }
            flashcards[0].UserProgress = progress.Where(up => up.FlashcardId == flashcards[0].Id).ToList();
            flashcards[1].UserProgress = progress.Where(up => up.FlashcardId == flashcards[1].Id).ToList();
            flashcards[2].UserProgress = new List<UserProgress>();
            var flashcardsFromService = await Service.GetFlashcards(user, category.Id, criterion, count);

            Assert.Equal(properCount, flashcardsFromService.Count);
            Assert.Contains(flashcards[0], flashcardsFromService);
            Assert.Contains(flashcards[1], flashcardsFromService);

        }

        [Theory]
        [InlineData(FlashcardsSearchCriterionEnum.New)]
        [InlineData(FlashcardsSearchCriterionEnum.Old)]

        public async Task Tmp(FlashcardsSearchCriterionEnum criterion)
        {
            int count = 20;
            int oldCount = 2;
            var category = (await AddCategories()).First();
            var flashcards = await AddFlashcards(category, count);
            var user = (await AddUsers()).First();
            var progress = await AddUserProgress(flashcards.Take(oldCount), new[] { user });
            user.UserProgress = progress;
            flashcards.ForEach(f => f.UserProgress = new List<UserProgress>());
            flashcards[0].UserProgress = progress.Where(up => up.FlashcardId == flashcards[0].Id).ToList();
            flashcards[1].UserProgress = progress.Where(up => up.FlashcardId == flashcards[1].Id).ToList();
            var flashcardsFromService = await Service.GetFlashcards(user, category.Id, criterion, count);
            if (criterion == FlashcardsSearchCriterionEnum.New)
            {
                int realCount = count - oldCount;
                Assert.Equal(realCount, flashcardsFromService.Count);
                Assert.DoesNotContain(flashcards[0], flashcardsFromService);
                Assert.DoesNotContain(flashcards[1], flashcardsFromService);
            }
            else
            {
                Assert.Equal(oldCount, flashcardsFromService.Count);
                Assert.Contains(flashcards[0], flashcardsFromService);
                Assert.Contains(flashcards[1], flashcardsFromService);
            }

        }
    }
}
