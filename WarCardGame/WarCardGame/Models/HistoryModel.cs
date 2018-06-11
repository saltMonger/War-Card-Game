using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarCardGame.Models
{
    public class HistoryModel
    {
        public CardModel PlayerCard { get; set; }
        public CardModel ComputerCard { get; set; }
    }
}
