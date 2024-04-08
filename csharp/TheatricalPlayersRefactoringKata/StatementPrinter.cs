using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var (totalPrice, volumeCredits) = CalculateTotalPrice(invoice, plays);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            string result = string.Format("Statement for {0}\n", invoice.Customer);

            foreach (var performance in invoice.Performances)
            {
                var (play, performancePrice) = CalculatePlayPrice(plays, performance);

                // Print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(performancePrice / 100), performance.Audience);
            }

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalPrice / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);

            return result;
        }

        private static (int, int) CalculateTotalPrice(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalPrice = 0;
            int volumeCredits = 0;

            foreach (var performance in invoice.Performances)
            {
                var (play, performancePrice) = CalculatePlayPrice(plays, performance);

                // Add volume credits
                volumeCredits += Math.Max(performance.Audience - 30, 0);
                // Add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)performance.Audience / 5);

                totalPrice += performancePrice;
            }

            return (totalPrice, volumeCredits);
        }

        private static (Play, int) CalculatePlayPrice(Dictionary<string, Play> plays, Performance performance)
        {
            var play = plays[performance.PlayID];
            int playPrice = 0;
            switch (play.Type)
            {
                case "tragedy":
                    playPrice = 40000;
                    if (performance.Audience > 30)
                    {
                        playPrice += 1000 * (performance.Audience - 30);
                    }
                    break;
                case "comedy":
                    playPrice = 30000;
                    if (performance.Audience > 20)
                    {
                        playPrice += 10000 + 500 * (performance.Audience - 20);
                    }
                    playPrice += 300 * performance.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + play.Type);
            }

            return (play, playPrice);
        }
    }
}
