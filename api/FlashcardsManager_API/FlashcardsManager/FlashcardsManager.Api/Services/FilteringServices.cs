using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashcardsManager.Core.Dtos;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Helpers;
using FlashcardsManager.Core.Models;
using FlashcardsManager.Core.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FlashcardsManager.Api.Services
{
    public class FilteringServices
    {
        public IQueryable<UserDto> Filter(IQueryable<User> q, UsersFilteringModel model)
        {
            var query = q.Include(u => u.UserProgress).AsQueryable();
            query = query.Where(u =>
                u.Name.Contains(model.SearchText) || u.Surname.Contains(model.SearchText) || u.UserName.Contains(model.SearchText));
            if (model.Descending)
                switch (model.SortingCriterion)
                {
                    case UsersSortingCriterion.None:
                        query = query.OrderByDescending(u => u.UserName);
                        break;
                    case UsersSortingCriterion.Name:
                        query = query.OrderByDescending(u => u.Name);
                        break;
                    case UsersSortingCriterion.Surname:
                        query = query.OrderByDescending(u => u.Surname);
                        break;
                    case UsersSortingCriterion.UserName:
                        query = query.OrderByDescending(u => u.UserName);
                        break;
                    case UsersSortingCriterion.Points:
                        query = query.OrderByDescending(u => u.UserProgress.Sum(up => up.Progress));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
            {
                switch (model.SortingCriterion)
                {
                    case UsersSortingCriterion.None:
                        query = query.OrderBy(u => u.UserName);
                        break;
                    case UsersSortingCriterion.Name:
                        query = query.OrderBy(u => u.Name);
                        break;
                    case UsersSortingCriterion.Surname:
                        query = query.OrderBy(u => u.Surname);
                        break;
                    case UsersSortingCriterion.UserName:
                        query = query.OrderBy(u => u.UserName);
                        break;
                    case UsersSortingCriterion.Points:
                        query = query.OrderBy(u => u.UserProgress.Sum(up => up.Progress));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return query.Skip(model.PageIndex * model.PageSize).Take(model.PageSize)
                .Select(u => new UserDto(u.Name, u.Surname, u.UserName, new Score(u.UserProgress.Sum(up => up.Progress))));
        }

        public IQueryable<CategoryDto> Filter(IQueryable<Category> query, CategoriesFilteringModel model)
        {
            query = query.Where(c => c.Name.Contains(model.SearchText));
            if (model.Descending)
                switch (model.SortingCriterion)
                {
                    case CategoriesSortingCriterion.None:
                        query = query.OrderByDescending(c => c.Id);
                        break;
                    case CategoriesSortingCriterion.Name:
                        query = query.OrderByDescending(c => c.Name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
            {
                switch (model.SortingCriterion)
                {
                    case CategoriesSortingCriterion.None:
                        query = query.OrderBy(c => c.Id);
                        break;
                    case CategoriesSortingCriterion.Name:
                        query = query.OrderBy(c => c.Name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return query.Skip(model.PageIndex * model.PageSize).Take(model.PageSize).Select(c => new CategoryDto(c));
        }

        public IQueryable<FlashcardDto> Filter(IQueryable<Flashcard> q, FlashcardsFilteringModel model)
        {
            var query = q.Include(f => f.Category).AsQueryable();
            query = query.Where(f =>
                f.Key.Contains(model.SearchText) || f.Value.Contains(model.SearchText) || f.KeyDescription.Contains(model.SearchText)
                || f.ValueDescription.Contains(model.SearchText) || f.Category.Name.Contains(model.SearchText));
            if (model.Descending)
                switch (model.SortingCriterion)
                {
                    case FlashcardsSortingCriterion.None:
                        query = query.OrderByDescending(f => f.Id);
                        break;
                    case FlashcardsSortingCriterion.Key:
                        query = query.OrderByDescending(f => f.Key);
                        break;
                    case FlashcardsSortingCriterion.Value:
                        query = query.OrderByDescending(f => f.Value);
                        break;
                    case FlashcardsSortingCriterion.KeyDescription:
                        query = query.OrderByDescending(f => f.KeyDescription);
                        break;
                    case FlashcardsSortingCriterion.ValueDescription:
                        query = query.OrderByDescending(f => f.ValueDescription);
                        break;
                    case FlashcardsSortingCriterion.Category:
                        query = query.OrderByDescending(f => f.Category.Name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else
            {
                switch (model.SortingCriterion)
                {
                    case FlashcardsSortingCriterion.None:
                        query = query.OrderBy(f => f.Id);
                        break;
                    case FlashcardsSortingCriterion.Key:
                        query = query.OrderBy(f => f.Key);
                        break;
                    case FlashcardsSortingCriterion.Value:
                        query = query.OrderBy(f => f.Value);
                        break;
                    case FlashcardsSortingCriterion.KeyDescription:
                        query = query.OrderBy(f => f.KeyDescription);
                        break;
                    case FlashcardsSortingCriterion.ValueDescription:
                        query = query.OrderBy(f => f.ValueDescription);
                        break;
                    case FlashcardsSortingCriterion.Category:
                        query = query.OrderBy(f => f.Category.Name);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return query.Skip(model.PageIndex * model.PageSize).Take(model.PageSize).Select(f => new FlashcardDto(f));
        }
    }
}
