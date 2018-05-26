using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlashcardsManager.Desktop.ViewModels
{
    public interface IAuthViewModel
    {
        ICommand ChangePageCommand { get; set; }
    }
}
