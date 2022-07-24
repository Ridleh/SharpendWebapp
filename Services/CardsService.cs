using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using sharpend_webapp.Interfaces;
using sharpend_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace sharpend_webapp.Services
{
    public class CardsService
    {
        private readonly ILogger<CardsService> logger;

        private static readonly string CardsURl = "https://firebasestorage.googleapis.com/v0/b/duelyst-dev-57aa1.appspot.com/o/public%2Fdata%2Fcards.json?alt=media";
        private static IEnumerable<CardData> cardsCache;

        public Dictionary<string, int> firstDrawProb = new Dictionary<string, int> {
                {"basic", 0},{"common", 0},{"rare", 0},{"epic", 96},{"legendary", 4},
            };
        public Dictionary<string, int> secondDrawProb = new Dictionary<string, int> {
                {"basic", 0},{"common", 0},{"rare", 90},{"epic", 10},{"legendary", 4},
            };
        public Dictionary<string, int> thirdDrawProb = new Dictionary<string, int> {
                {"basic", 0},{"common", 95},{"rare", 3},{"epic", 2},{"legendary", 0},
            };
        public Dictionary<string, int> NDrawProb = new Dictionary<string, int> {
                {"basic", 0},{"common", 95},{"rare", 5},{"epic", 0},{"legendary", 0},
            };

        private static Random random = new Random();
        public CardsService(ILogger<CardsService> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<CardData> GetCards()
        {
            return cardsCache;
        }

        public void SaveCards(IEnumerable<CardData> cards)
        {
            cardsCache = cards;
        }

        public WeightedRandomService<CardData> SetUpWeightedRandoms(WeightedRandomService<CardData> weightedRandoms, IEnumerable<CardData> cards, Dictionary<string,int> probabilities)
        {
            foreach(var card in cards)
            {
                switch (card.rarity)
                {
                    case "basic":
                        weightedRandoms.AddEntry(card, probabilities["basic"]);
                        break;
                    case "common":
                        weightedRandoms.AddEntry(card, probabilities["common"]);
                        break;
                    case "rare":
                        weightedRandoms.AddEntry(card, probabilities["rare"]);
                        break;
                    case "epic":
                        weightedRandoms.AddEntry(card, probabilities["epic"]);
                        break;
                    case "legendary":
                        weightedRandoms.AddEntry(card, probabilities["legendary"]);
                        break;
                    default:
                        Console.WriteLine($"No Rarity found for card {card.name}");
                        break;
                }
            }
            return weightedRandoms;
        }

        public IEnumerable<CardData> DrawCardsForLootBox(int count)
        {
            List<CardData> cards = new();
            for(int i = 0; i < count; i++)
            {
                switch (i)
                {
                    case 0:
                        var weightedCards = SetUpWeightedRandoms(new WeightedRandomService<CardData>(), cardsCache, firstDrawProb);
                        cards.Add(weightedCards.GetRandom());
                        break;
                    case 1:
                        var weightedCards1 = SetUpWeightedRandoms(new WeightedRandomService<CardData>(), cardsCache, firstDrawProb);
                        cards.Add(weightedCards1.GetRandom());
                        break;
                    case 2:
                        var weightedCards2 = SetUpWeightedRandoms(new WeightedRandomService<CardData>(), cardsCache, secondDrawProb);
                        cards.Add(weightedCards2.GetRandom());
                        break;
                    case 3:
                        var weightedCards3 = SetUpWeightedRandoms(new WeightedRandomService<CardData>(), cardsCache, thirdDrawProb);
                        cards.Add(weightedCards3.GetRandom());
                        break;
                    default:
                        var weightedCardsN = SetUpWeightedRandoms(new WeightedRandomService<CardData>(), cardsCache, NDrawProb);
                        cards.Add(weightedCardsN.GetRandom());
                        break;
                }
            }
            return cards;
        }

        public IEnumerable<CardData> DrawCardsForGauntlet(IEnumerable<CardData.Faction> factions, IEnumerable<string> rarities, bool includeNuetral)
        {
            var baseList = includeNuetral ? cardsCache : cardsCache.Where(card => card.faction != CardData.Faction.Neutral);
            return baseList.Where(card => factions.Contains(card.faction))
                .Where(card => rarities.Contains(card.rarity))
                .OrderBy(card => random.Next())
                .Take(3);
        }
    }
}
