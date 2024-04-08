using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{


    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalPrice = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var performance in invoice.Performances)
            {
                Play play;
                int performancePrice;
                CalculatePlayPrice(plays, performance, out play, out performancePrice);
                // add volume credits
                volumeCredits += Math.Max(performance.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)performance.Audience / 5);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(performancePrice / 100), performance.Audience);
                totalPrice += performancePrice;
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalPrice / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);

            return result;
        }

        private static void CalculatePlayPrice(Dictionary<string, Play> plays, Performance performance, out Play play, out int playPrice)
        {
            play = plays[performance.PlayID];
            playPrice = 0;
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
        }
    }
}
