using Newtonsoft.Json;
using System;

namespace sharpend_webapp.Models
{
    public class CardData
    {
        public int? id { get; set; }
        [JsonProperty("card_id")] 
        public string cardID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        [JsonProperty("sprite_url")] 
        public string spriteUrl { get; set; }
        public int cost { get; set; }
        public int? attack { get; set; }
        public int? health { get; set; }
        public bool general { get; set; }
        public string tribe { get; set; }
        public string rarity { get; set; }
        public string tag { get; set; }
        [JsonProperty("card_effect_id")] 
        public int? cardEffectID { get; set; }
        [JsonProperty("card_effect")] 
        public string cardEffect { get; set; }

        public string cardType
        {
            get
            {
                if (attack == null)
                {
                    if (spriteUrl != null)
                    {
                        if (spriteUrl.Contains("icons/artifact"))
                        {
                            return "Artifact";
                        }
                        return "Spell";
                    }
                    else
                    {
                        return "Spell";
                    }
                }
                return "Unit";
            }
        }
        public Faction faction { get; set; }

        public enum Faction
        {
            Neutral,
            Lyonar,
            Songhai,
            Vetruvian,
            Abyssian,
            Magmar,
            Vanar
        }
    }
}
