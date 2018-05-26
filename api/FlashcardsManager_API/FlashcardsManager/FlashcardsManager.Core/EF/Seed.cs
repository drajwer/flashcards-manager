using FlashcardsManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FlashcardsManager.Core.Enums;
using FlashcardsManager.Core.Options;
using Microsoft.Extensions.Options;

namespace FlashcardsManager.Core.EF
{
    public class Seed
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleNamesOptions _roleNamesOptions;

        public Seed(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<RoleNamesOptions> roleNamesOptions)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleNamesOptions = roleNamesOptions.Value;
        }

        public static async Task Run(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var instance = serviceScope.ServiceProvider.GetService<Seed>();
                await instance.Initialize();
            }
        }

        public async Task Initialize()
        {
            _context.Database.Migrate();
            if (_context.Categories.Any()) return;
            var polEngFlashcards = new List<Flashcard>
            {
                new Flashcard()
                {
                    Key = "bezzałogowy",
                    Value = "unmanned",
                    KeyDescription = "Statek kosmiczny, który eksplodował, na szczęście był bezzałogowy",
                    ValueDescription = "Fortunately, the spacecraft that exploded was unmanned",
                },
                new Flashcard()
                {
                    Key = "niezliczony",
                    Value = "innumerable",
                    ValueDescription = "I've tried to make it display properly innumerable times",
                    KeyDescription = "Liczba prób potrzebna, by to zaczęło działać, była niezliczona",
                },
                new Flashcard()
                {
                    Key = "oczekujący",
                    Value = "pending",
                    ValueDescription = "You've got 2 pending requests",
                },
                new Flashcard()
                {
                    Key = "odmówić",
                    Value = "refuse",
                    KeyDescription = "Złożę mu taką ofertę, że nie będzie mógł odmówić.",
                    ValueDescription = "I'll make him offer, he cannot refuse.",
                },
                new Flashcard()
                {
                    Key = "bochenek",
                    Value = "loaf",
                },
                new Flashcard()
                {
                    Key = "skomplikowany",
                    Value = "sophisticated",
                },
                new Flashcard()
                {
                    Key = "piękna",
                    Value = "beautiful",
                },
                new Flashcard()
                {
                    Key = "okrucieństwo",
                    Value = "ferocity",
                },
                new Flashcard()
                {
                    Key = "metro",
                    Value = "subway",
                },
                new Flashcard()
                {
                    Key = "próg",
                    Value = "threshold",
                },
                new Flashcard()
                {
                    Key = "jeż",
                    Value = "hedgehog",
                },
                new Flashcard()
                {
                    Key = "żywopłot",
                    Value = "hedge",
                },
                new Flashcard()
                {
                    Key = "kastrować",
                    Value = "geld",
                },
                new Flashcard()
                {
                    Key = "ogier",
                    Value = "stallion",
                },
                new Flashcard()
                {
                    Key = "pióro",
                    Value = "quill",
                },
                new Flashcard()
                {
                    Key = "różdżka",
                    Value = "wand",
                }

            };

            var category = new Category()
            {
                Name = "Fiszki polsko-angielskie",
                Flashcard = polEngFlashcards,
                Availability = AvailabilityEnum.Public
            };

            var category2 = new Category()
            {
                Name = "Anatomia",
                Flashcard = new List<Flashcard>()
                {
                    new Flashcard("Tętnica główna", "Aorta", 0, "nazwa łaciñska", "Aorta"),
                    new Flashcard("Talerz kości biodrowej", "Wing of ilium", 0, "nazwa łaciñska", "Ala ossis ilii"),
                    new Flashcard("Mięsień dwugłowy uda", "Biceps femoris", 0, "nazwa łaciñska",
                        "Musculus biceps femoris")
                },
                Availability = AvailabilityEnum.Private

            };

            var category3 = new Category
            {
                Name = "Wydarzenia historyczne",
                Availability = AvailabilityEnum.Pending
            };

            if (_context.Users.Any())
            {
                await _context.SaveChangesAsync();
                return;
            }

            var user = new User()
            {
                Name = "Janusz",
                Surname = "Kowalski",
                UserName = "janusz",
            };
            _userManager.CreateAsync(user, "Password!1234").Wait();

            var regularUser = new User
            {
                Name = "Bill",
                Surname = "Gates",
                UserName = "ImTooRichToUseFlashcards"
            };
            _userManager.CreateAsync(regularUser, "Password!1234").Wait();

            category.CreatorId = category2.CreatorId = user.Id;
            category3.CreatorId = regularUser.Id;

            await _context.Categories.AddRangeAsync(category, category2, category3);

            if (_context.Roles.Any())
            {
                await _context.SaveChangesAsync();
                return;
            }
            var adminRole = new IdentityRole(_roleNamesOptions.AdminRoleName);
            _roleManager.CreateAsync(adminRole).Wait();
            _userManager.AddToRoleAsync(user, adminRole.Name).Wait();

            var regularUserRole = new IdentityRole(_roleNamesOptions.RegularUserRoleName);
            _roleManager.CreateAsync(regularUserRole).Wait();
            _userManager.AddToRoleAsync(regularUser, regularUserRole.Name).Wait();

            if (_context.UserProgress.Any())
            {
                await _context.SaveChangesAsync();
                return;
            }
            var progressList = new[]
            {
                new UserProgress()
                {
                    Flashcard = polEngFlashcards[0],
                    User = user,
                    Progress = 1,
                },
                new UserProgress()
                {
                    Flashcard = polEngFlashcards[1],
                    User = user,
                    Progress = -1,
                },
                new UserProgress()
                {
                    Flashcard = polEngFlashcards[2],
                    User = user,
                    Progress = 5,
                },
            };

            await _context.UserProgress.AddRangeAsync(progressList);

            await _context.SaveChangesAsync();
        }

    }
}
