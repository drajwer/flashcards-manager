using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using FlashcardsManager.Core.Enums;

namespace FlashcardsManager.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public AvailabilityEnum Availability { get; set; }

        public string CreatorId { get; set; }

        public virtual User Creator { get; set; }

        [JsonIgnore]
        public virtual ICollection<Flashcard> Flashcard { get; set; }
    }
}
