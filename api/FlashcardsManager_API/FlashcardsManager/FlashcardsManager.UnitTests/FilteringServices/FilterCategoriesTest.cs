using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using Xunit;

namespace FlashcardsManager.UnitTests.FilteringServices
{
    public class FilterCategoriesTest : TestBase
    {

        [Theory]
        [InlineData(true, 3)]
        [InlineData(false, 3)]
        public void InvalidFilteringModel(bool descending, CategoriesSortingCriterion criterion)
        {
            var model = new CategoriesFilteringModel("", criterion, descending, 1, 1);
            var query = Context.Categories.AsQueryable();
            Assert.Throws<ArgumentOutOfRangeException>(() => FilteringServices.Filter(query, model));
        }

        [Theory]
        [InlineData("Du*aDrivenDevelopment", CategoriesSortingCriterion.Name, true, 1, 10)]
        [InlineData("", CategoriesSortingCriterion.Name, true, 1, 10)]
        [InlineData("3", CategoriesSortingCriterion.Name, true, 0, 5)]
        [InlineData("3", CategoriesSortingCriterion.None, true, 0, 1)]
        [InlineData("3", CategoriesSortingCriterion.None, false, 0, 1)]
        [InlineData("", CategoriesSortingCriterion.None, false, 6, 3)]
        public void ReturnsProperlyFilteredCategories(string searchText, CategoriesSortingCriterion criterion,
            bool descending, int pageIndex, int pageSize)
        {
            var model = new CategoriesFilteringModel(searchText, criterion, descending, pageIndex, pageSize);
            var categories = AddCategories(30);
            var query = Context.Categories.AsQueryable();

            var filteredCategories = FilteringServices.Filter(query, model).ToList();

            var expected = categories.Where(c => c.Name.Contains(searchText));
            var lambda = GetLambda(criterion);
            expected = descending ? expected.OrderByDescending(lambda) : expected.OrderBy(lambda);
            var expectedList = expected.Skip(model.PageIndex * model.PageSize).Take(model.PageSize).ToList();

            Assert.Equal(expectedList.Count, filteredCategories.Count);
            for(var i = 0; i < expectedList.Count; i++)
                Assert.True(expectedList[i].Id == (filteredCategories[i].Id));
        }

        private Func<Category, object> GetLambda(CategoriesSortingCriterion criterion)
        {
            switch(criterion)
            {
                case CategoriesSortingCriterion.None:
                    return (c => c.Id);
                case CategoriesSortingCriterion.Name:
                    return (c => c.Name);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

