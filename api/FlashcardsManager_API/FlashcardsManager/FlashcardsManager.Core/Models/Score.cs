using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsManager.Core.Models
{
    public class Score
    {
        public int SumPoints { get; set; }

        public Score(int sumPoints)
        {
            SumPoints = sumPoints;
        }
    }
}
