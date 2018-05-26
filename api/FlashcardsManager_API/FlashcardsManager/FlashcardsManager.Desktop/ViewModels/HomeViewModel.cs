using System.Threading.Tasks;
using FlashcardsManager.Desktop.Helpers;

namespace FlashcardsManager.Desktop
{
    public class HomeViewModel : ObservableObject, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "Home Page";
            }
        }

        public async Task Update()
        {
        }
    }
}
