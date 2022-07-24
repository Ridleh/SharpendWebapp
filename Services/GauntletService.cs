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

    public class GauntletService
    {
        private readonly ICardsService cardsService;
        private readonly ILogger<CardsService> logger;
        private IEnumerable<CardData> staticCardsCache;
        private IEnumerable<CardData> cardsCache;
        private Random random; 

        public GauntletService(ICardsService cardsService, ILogger<CardsService> logger)
        {
            this.cardsService = cardsService;
            this.logger = logger;
            random = new();
        }

        public async Task GetAndSetStaticCards()
        {
            staticCardsCache = await cardsService.GetStaticCardData();
        }
        public async Task GetAndSetCardsFromFireStore()
        {
            
            cardsCache = await cardsService.GetCardsFromFireStore();
        }

        public IEnumerable<CardData> DrawCards()
        {
            return staticCardsCache.OrderBy(card => random.Next()).Take(3);
        }

        public IEnumerable<CardData> DrawCardsByRarity(string rarity)
        {
            return staticCardsCache.Where(card => card.rarity == rarity).OrderBy(card => random.Next()).Take(3);
        }

        public IEnumerable<CardData> DrawCardsByFaction(CardData.Faction faction)
        {
            return staticCardsCache.Where(card => card.faction == faction);
        }

        public IEnumerable<CardData> DrawCardsByFactionByRarity(CardData.Faction faction, string rarity)
        {
            return staticCardsCache.Where(card => card.faction == faction).Where(card => card.rarity == rarity);
        }
    }
}
