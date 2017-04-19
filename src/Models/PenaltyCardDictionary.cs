using System.Collections.Generic;

namespace mmmsl.Models
{
    public static class PenaltyCardDictionary
    {
        private static IDictionary<string, PenaltyCard> penaltyCards = new Dictionary<string, PenaltyCard> {
            {
                "UB", new PenaltyCard {
                    MisconductCode = "UB",
                    Description = "Unsporting Behavior",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "DI", new PenaltyCard {
                    MisconductCode = "DI",
                    Description = "Dissent by word or action",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "PI", new PenaltyCard {
                    MisconductCode = "PI",
                    Description = "Persistent infringement of the rules",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "DR", new PenaltyCard {
                    MisconductCode = "DR",
                    Description = "Delays the restart of play",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "FD", new PenaltyCard {
                    MisconductCode = "FD",
                    Description = "Fails to respect required distance at restart of play (corner or free kick)",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "EN", new PenaltyCard {
                    MisconductCode = "EN",
                    Description = "Enters or reenters play without permission of the referee",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "LE", new PenaltyCard {
                    MisconductCode = "LE",
                    Description = "Deliberately leaves play without permissions of the referee",
                    Points = 1,
                    Severity = PenaltyCardSeverity.Yellow
                }
            },
            {
                "FP", new PenaltyCard {
                    MisconductCode = "FP",
                    Description = "Seriously foul play",
                    Points = 3,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "VC", new PenaltyCard {
                    MisconductCode = "VC",
                    Description = "Violent conduct",
                    Points = 3,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "SP", new PenaltyCard {
                    MisconductCode = "SP",
                    Description = "Spits at another player/official",
                    Points = 3,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "DH", new PenaltyCard {
                    MisconductCode = "DH",
                    Description = "Deliberate hand ball to stop a goal",
                    Points = 2,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "DF", new PenaltyCard {
                    MisconductCode = "DF",
                    Description = "Deliberate foul to stop a goal scoring opportunity",
                    Points = 2,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "OL", new PenaltyCard {
                    MisconductCode = "OL",
                    Description = "Offensive/insulting language or gestures",
                    Points = 2,
                    Severity = PenaltyCardSeverity.Red
                }
            },
            {
                "SC", new PenaltyCard {
                    MisconductCode = "SC",
                    Description = "2nd caution in the same match",
                    Points = 0,
                    Severity = PenaltyCardSeverity.Red
                }
            },
        };

        public static IDictionary<string, PenaltyCard> FindAll()
        {
            return penaltyCards;
        }

        public static PenaltyCard Find(string key)
        {
            return penaltyCards[key];
        }
    }
}
