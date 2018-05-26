using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using Xunit;
using FlashcardsManager.Core.Dtos;

namespace FlashcardsManager.UnitTests.FilteringServices
{
    public class FilterFlashcardsTest : TestBase
    {
        [Theory]
        [InlineData(true, 7)]
        [InlineData(false, 7)]
        public void InvalidFilteringModel(bool descending, FlashcardsSortingCriterion criterion)
        {
            var model = new FlashcardsFilteringModel("", criterion, descending, 1, 1);
            var query = Context.Flashcards.AsQueryable();
            Assert.Throws<ArgumentOutOfRangeException>(() => FilteringServices.Filter(query, model));
        }

        [Theory]
        [InlineData("Du*aDrivenDevelopment", FlashcardsSortingCriterion.None, true, 1, 10)]
        [InlineData("Du*aDrivenDevelopment", FlashcardsSortingCriterion.None, false, 15, 1)]
        [InlineData("3", FlashcardsSortingCriterion.Key, true, 15, 1)]
        [InlineData("3", FlashcardsSortingCriterion.Key, false, 15, 1)]
        [InlineData("3", FlashcardsSortingCriterion.Value, true, 15, 3)]
        [InlineData("3", FlashcardsSortingCriterion.Value, false, 15, 3)]
        [InlineData("9", FlashcardsSortingCriterion.KeyDescription, true, 3, 15)]
        [InlineData("9", FlashcardsSortingCriterion.KeyDescription, false, 3, 15)]
        [InlineData("y 1", FlashcardsSortingCriterion.ValueDescription, true, 300, 15)]
        [InlineData("y 1", FlashcardsSortingCriterion.ValueDescription, false, 2, 15)]
        [InlineData("y ", FlashcardsSortingCriterion.Category, true, -1, 50)]
        [InlineData("y ", FlashcardsSortingCriterion.Category, false, 0, 50)]

        public void ReturnsProperlyFilteredCategories(string searchText, FlashcardsSortingCriterion criterion,
            bool descending, int pageIndex, int pageSize)
        {
            var model = new FlashcardsFilteringModel(searchText, criterion, descending, pageIndex, pageSize);
            var categories = AddCategories();
            var flashards = AddFlashcards(categories[0], 50);

            var query = Context.Flashcards.AsQueryable();
            var filteredFlashcards = FilteringServices.Filter(query, model).ToList();

            var expected = flashards.Where(f =>
                f.Key.Contains(model.SearchText) || f.Value.Contains(model.SearchText) || f.KeyDescription.Contains(model.SearchText)
                || f.ValueDescription.Contains(model.SearchText) || f.Category.Name.Contains(model.SearchText));

            var lambda = GetLambda(criterion);
            expected = descending ? expected.OrderByDescending(lambda) : expected.OrderBy(lambda);

            var expectedList = expected.Skip(model.PageIndex * model.PageSize).Take(model.PageSize)
                .ToList();

            Assert.Equal(expectedList.Count, filteredFlashcards.Count);
            for (var i = 0; i < expectedList.Count; i++)
            {
                var returned = filteredFlashcards[i];
                var exp = new FlashcardDto(expectedList[i]);
                Assert.True(exp.Id == returned.Id);
            }
        }

        private Func<Flashcard, object> GetLambda(FlashcardsSortingCriterion criterion)
        {
            switch (criterion)
            {
                case FlashcardsSortingCriterion.None:
                    return (f => f.Id);
                case FlashcardsSortingCriterion.Key:
                    return (f => f.Key);
                case FlashcardsSortingCriterion.Value:
                    return (f => f.Value);
                case FlashcardsSortingCriterion.KeyDescription:
                    return (f => f.KeyDescription);
                case FlashcardsSortingCriterion.ValueDescription:
                    return (f => f.ValueDescription);
                case FlashcardsSortingCriterion.Category:
                    return (f => f.Category.Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
