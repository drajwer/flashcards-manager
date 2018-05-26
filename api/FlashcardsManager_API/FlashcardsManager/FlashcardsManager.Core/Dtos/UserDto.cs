using System.ComponentModel.DataAnnotations;
using FlashcardsManager.Core.Models;

namespace FlashcardsManager.Core.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
        }

        public UserDto(string name, string surname, string userName, Score score)
        {
            Name = name;
            Surname = surname;
            UserName = userName;
            Score = score;
        }

        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string UserName { get; set; }
        public Score Score { get; set; }

        public User ToEntity()
        {
            return new User
            {
                Name = Name,
                Surname = Surname,
                UserName = UserName
            };
        }
    }
}
