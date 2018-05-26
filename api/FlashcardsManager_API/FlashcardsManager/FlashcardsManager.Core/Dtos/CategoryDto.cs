using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlashcardsManager.Core.Dtos
{
    public class CategoryDto
    {
        public CategoryDto(Category category)
        {
            Name = category.Name;
            Availability = category.Availability;
            Id = category.Id;
        }

        public CategoryDto() { }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public AvailabilityEnum Availability { get; set; }

        public Category ToEntity()
        {
            return new Category { Name = Name, Availability = Availability };
        }
    }
}
