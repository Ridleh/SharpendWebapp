using sharpend_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sharpend_webapp.Interfaces
{
    public interface ICardsService
    {
        Task<IEnumerable<CardData>> GetStaticCardData();
        Task<IEnumerable<CardData>> GetCardsFromFireStore();
        Task SaveCards(IEnumerable<CardData> cards);
    }
}
