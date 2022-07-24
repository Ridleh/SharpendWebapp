using Microsoft.Extensions.Logging;
using sharpend_webapp.Interfaces;
using sharpend_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace sharpend_webapp.Services
{
    public class DroprateService
    {
        private readonly ICardsService cardsService;
        private readonly ILogger<CardsService> logger;
        private static IEnumerable<CardData> staticCardsCache;
        private static IEnumerable<CardData> cardsCache;
        public DroprateService(ICardsService cardsService, ILogger<CardsService> logger)
        {
            this.cardsService = cardsService;
            this.logger = logger;
        }

        public async Task<IEnumerable<CardData>> GetStaticCardData()
        {
            try
            {
                if (staticCardsCache != null)
                {
                    return staticCardsCache;
                }
                var cards = await cardsService.GetStaticCardData();
                staticCardsCache = cards;
                return cards;
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public async Task<IEnumerable<CardData>> GetCardsFromFireStore()
        {
            if (cardsCache != null)
            {
                return cardsCache;
            }
            var cards = await cardsService.GetStaticCardData();
            cardsCache = cards;
            return cards;
        }
    }
}
