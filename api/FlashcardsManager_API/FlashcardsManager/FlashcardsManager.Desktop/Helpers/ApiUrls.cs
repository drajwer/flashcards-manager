using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsManager.Desktop.Helpers
{
    public static class ApiUrls
    {
        public static string Host = "https://localhost:44380";//"https://flashcards-manager.azurewebsites.net";
        public static string ApiUrl = Host + "/api";
        public static string UserEndpoint = ApiUrl + "/users";
        public static string CategoriesEndpoint = ApiUrl + "/categories";
        public static string FlashcardsEndpoint = ApiUrl + "/flashcards";
        public static string LearningFlashcardsEndpoint = ApiUrl + "/learning/flashcards";
        public static string LearningResultEndpoint = ApiUrl + "/learning/result";
        public static string BearerToken { get; set; }
        public static string ClientName = "wpf";
        public static string ClientSecret = "flashcardsSecret";
        public static string TokenEndpoint = Host + "/connect/token";
        public static string Scope = "flashcardsScope";
        public static string RegisterAction = ApiUrl + "/account/register";


    }
}
