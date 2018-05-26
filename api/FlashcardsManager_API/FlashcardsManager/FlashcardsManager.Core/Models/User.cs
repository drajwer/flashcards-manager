using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FlashcardsManager.Core.Models
{
    public class User : IdentityUser<string>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        //[JsonIgnore]
        public virtual ICollection<UserProgress> UserProgress { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
