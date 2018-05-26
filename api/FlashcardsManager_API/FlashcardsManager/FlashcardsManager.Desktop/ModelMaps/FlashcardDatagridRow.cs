using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlashcardsManager.Core.Models;

namespace FlashcardsManager.Desktop.ModelMaps
{
    public class FlashcardDatagridRow
    {
        public Flashcard Flashcard { get; set; }
        public ICommand DeleteCommand { get; set; }

        public FlashcardDatagridRow(Flashcard flashcard, ICommand deleteCommand)
        {
            Flashcard = flashcard;
            DeleteCommand = deleteCommand;
        }
    }
}
