using System.Threading.Tasks;

namespace FlashcardsManager.Desktop
{
    public interface IPageViewModel
    {
        string Name { get; }
        Task Update();
    }
}
