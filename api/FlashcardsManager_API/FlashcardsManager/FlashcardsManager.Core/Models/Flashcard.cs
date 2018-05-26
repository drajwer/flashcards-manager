using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashcardsManager.Core.Repositories.Interfaces;
using Newtonsoft.Json;

namespace FlashcardsManager.Core.Models
{
    public class Flashcard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public string KeyDescription { get; set; }

        public string ValueDescription { get; set; }

        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserProgress> UserProgress { get; set; }

        public Flashcard(string key, string value, int categoryId, string keyDesc = "", string valueDesc = "")
        {
            Key = key;
            Value = value;
            KeyDescription = keyDesc;
            ValueDescription = valueDesc;
            CategoryId = categoryId;
        }

        public Flashcard()
        {
        }
    }
}
