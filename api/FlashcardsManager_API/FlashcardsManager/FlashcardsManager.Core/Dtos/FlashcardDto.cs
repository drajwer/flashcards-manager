using FlashcardsManager.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlashcardsManager.Core.Dtos
{
    public class FlashcardDto
    {
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public string KeyDescription { get; set; }

        public string ValueDescription { get; set; }

        public int CategoryId { get; set; }

        public FlashcardDto(Flashcard flashcard)
        {
            Id = flashcard.Id;
            Key = flashcard.Key;
            Value = flashcard.Value;
            KeyDescription = flashcard.KeyDescription;
            ValueDescription = flashcard.ValueDescription;
        }

        public FlashcardDto()
        { }

        public Flashcard ToEntity()
        {
            return new Flashcard(Key, Value, CategoryId, KeyDescription, ValueDescription);
        }
    }
}
