using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using Xunit;

namespace FlashcardsManager.UnitTests.FilteringServices
{
    public class FilterUsersTest : TestBase
    {
        [Theory]
        [InlineData(true, 6)]
        [InlineData(false, 6)]
        public void InvalidFilteringModel(bool descending, UsersSortingCriterion criterion)
        {
            var model = new UsersFilteringModel("", criterion, descending, 1, 1);
            var query = Context.Users.AsQueryable();
            Assert.Throws<ArgumentOutOfRangeException>(() => FilteringServices.Filter(query, model));
        }

        [Theory]
        [InlineData("Du*aDrivenDevelopment", UsersSortingCriterion.None, true, 1, 10)]
        [InlineData("Du*aDrivenDevelopment", UsersSortingCriterion.None, false, 1, 10)]
        [InlineData("Mariusz", UsersSortingCriterion.None, false, 1, 10)]
        [InlineData("Mariusz", UsersSortingCriterion.None, true, 1, 10)]
        [InlineData("Mariusz", UsersSortingCriterion.Name, true, 5, 5)]
        [InlineData("Mariusz", UsersSortingCriterion.Name, false, 3, 9)]
        [InlineData("3", UsersSortingCriterion.Surname, true, 3, 9)]
        [InlineData("3", UsersSortingCriterion.Surname, false, 3, 2)]
        [InlineData("9", UsersSortingCriterion.UserName, true, 0, 5)]
        [InlineData("9", UsersSortingCriterion.UserName, false, 0, 5)]
        [InlineData("4", UsersSortingCriterion.Points, true, 6, 6)]
        [InlineData("4", UsersSortingCriterion.Points, false, 6, 4)]

        public void ReturnsProperlyFilteredCategories(string searchText, UsersSortingCriterion criterion,
            bool descending, int pageIndex, int pageSize)
        {
            var model = new UsersFilteringModel(searchText, criterion, descending, pageIndex, pageSize);
            var users = AddUsers(31);
            var categories = AddCategories();
            var flashards = AddFlashcards(categories[0], 50);
            var userProgress = AddUserProgress(flashards, users.GetRange(0, 20));

            var query = Context.Users.AsQueryable();
            var filteredUserDtos = FilteringServices.Filter(query, model).ToList();

            var expected = users.Where(u =>
                u.Name.Contains(model.SearchText) || u.Surname.Contains(model.SearchText) || u.UserName.Contains(model.SearchText));
            var lambda = GetLambda(criterion);
            expected = descending ? expected.OrderByDescending(lambda) : expected.OrderBy(lambda);

            var expectedList = expected.Skip(model.PageIndex * model.PageSize).Take(model.PageSize)
                .Select(u => new UserDto(u.Name, u.Surname, u.UserName, 
                new Score(userProgress.Where(up => up.UserId == u.Id).Sum(progress => progress.Progress))))
                .ToList();

            Assert.Equal(expectedList.Count, filteredUserDtos.Count);
            for (var i = 0; i < expectedList.Count; i++)
                Assert.True(expectedList[i].UserName.Equals(filteredUserDtos[i].UserName));
        }

        private Func<User, object> GetLambda(UsersSortingCriterion criterion)
        {
            switch (criterion)
            {
                case UsersSortingCriterion.None:
                    return (u => u.UserName);
                case UsersSortingCriterion.Name:
                    return (u => u.Name);
                case UsersSortingCriterion.Surname:
                    return (u => u.Surname);
                case UsersSortingCriterion.UserName:
                    return (u => u.UserName);
                case UsersSortingCriterion.Points:
                    return (u => u.UserProgress.Sum(up => up.Progress));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
