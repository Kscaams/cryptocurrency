using System;
using System.Collections.Generic;

public class Program
{
    // KSC: Liste over gyldige kryptovalutaer fra Wikipedia, tilgængelig for hele programmet
    public static List<string> validCurrencies = new List<string>
    {
        "Bitcoin", "Litecoin", "Namecoin", "Peercoin", "Dogecoin", "Gridcoin", "Primecoin",
        "Ripple", "Nxt", "Auroracoin", "Dash", "NEO", "MazaCoin", "Monero", "Titcoin",
        "Verge", "Stellar", "Vertcoin", "Ethereum", "Ethereum Classic", "Nano", "Tether",
        "Firo", "Zcash", "Bitcoin Cash", "EOS.IO", "Cardano", "Tron", "AmbaCoin",
        "Nervos Network", "Algorand", "Avalanche", "Shiba Inu", "Polkadot", "Solana",
        "DeSo", "SafeMoon"
    };

    /// <summary>
    /// KSC: Tilføjer en ny valuta til listen over gyldige kryptovalutaer.
    /// KSC: Hvis valutaen allerede findes, kaster funktionen en ArgumentException.
    /// </summary>
    /// <param name="currency">Navnet på den kryptovaluta, der skal tilføjes.</param>
    /// <exception cref="ArgumentException">Kastes hvis valutaen allerede findes i listen.</exception>
    public static void AddCurrency(string currency)
    {
        if (!validCurrencies.Contains(currency))
        {
            validCurrencies.Add(currency);
        }
        else
        {
            throw new ArgumentException($"Valutaen '{currency}' findes allerede.");
        }
    }

    /// <summary>
    /// KSC: Fjerner en valuta fra listen over gyldige kryptovalutaer.
    /// KSC: Hvis valutaen ikke findes, kaster funktionen en ArgumentException.
    /// </summary>
    /// <param name="currency">Navnet på den kryptovaluta, der skal fjernes.</param>
    /// <exception cref="ArgumentException">Kastes hvis valutaen ikke findes i listen.</exception>
    public static void RemoveCurrency(string currency)
    {
        if (validCurrencies.Contains(currency))
        {
            validCurrencies.Remove(currency);
        }
        else
        {
            throw new ArgumentException($"Valutaen '{currency}' findes ikke i listen.");
        }
    }


    public static void Main(string[] args)
    {
        // Print listen over gyldige kryptovalutaer
        Console.Write("Liste over gyldige kryptovalutaer: ");
        for (int i = 0; i < validCurrencies.Count; i++)
        {
            if (i == validCurrencies.Count - 1)
            {
                // Sidste valuta uden komma efter
                Console.Write(validCurrencies[i]);
            }
            else
            {
                // Valuta med komma og mellemrum
                Console.Write(validCurrencies[i] + ", ");
            }
        }
        Console.WriteLine();

        Converter converter = new Converter();

        converter.SetPricePerUnit("Bitcoin", 50000.00);
        converter.SetPricePerUnit("Ethereum", 2462.51);
        converter.SetPricePerUnit("Bitcoin", 61752.70);

        try
        {
            double result = converter.Convert("Bitcoin", "Ethereum", 1);
            result = Math.Round(result, 2);
            Console.WriteLine();
            //Console.WriteLine($"Test convertering af 1 Bitcoin({converter.GetPricePerUnit("Bitcoin")} USD) til Ethereum({converter.GetPricePerUnit("Ethereum")} USD)");
            Console.WriteLine();
            Console.WriteLine("1 Bitcoin er så mange Etherum værd: " + result);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public class Converter
{
    // KSC: Dictionary til at holde kryptovaluta navne og deres priser i dollars
    private Dictionary<string, double> priceMapping = new Dictionary<string, double>();

    /// <summary>
    /// KSC: Fjerner alle valutaer og deres priser fra priceMapping listen
    /// </summary>
    public void ClearAllPrices()
    {
        priceMapping.Clear();
    }

    /// <summary>
    /// KSC: Fjerner en enkelt valuta fra priceMapping.
    /// KSC: Kaster en ArgumentException, hvis valutaen ikke findes.
    /// </summary>
    /// <param name="currencyName">Navnet på den valuta, der skal fjernes.</param>
    public void RemoveCurrency(string currencyName)
    {
        if (!priceMapping.ContainsKey(currencyName))
        {
            throw new ArgumentException($"Valuta '{currencyName}' findes ikke og kan ikke fjernes.");
        }

        priceMapping.Remove(currencyName);
    }


    /// <summary>
    /// Angiver prisen for en enhed af en forud defineret kryptovaluta. Prisen angives i dollars.
    /// Hvis der tidligere er angivet en værdi for samme kryptovaluta, 
    /// bliver den gamle værdi overskrevet af den nye værdi.
    /// KSC Se listen "validCurrencies" i Program-klassen for en liste over gyldige kryptovalutaer.
    /// KSC Det er et case sensitiv opslag fra listen.
    /// KSC Prisen skal være positiv og større end nul.
    /// </summary>
    /// <param name="currencyName">Navnet på den kryptovaluta der angives</param>
    /// <param name="price">Prisen på en enhed af valutaen målt i dollars. Prisen kan ikke være negativ</param>
    public void SetPricePerUnit(string currencyName, double price)
    {
        // KSC: Tjek om valuta navnet er gyldigt jf. liste fra Program-klassen
        if (!Program.validCurrencies.Contains(currencyName))
        {
            throw new ArgumentException("Ugyldigt kryptovalutanavn.");
        }

        // KSC: Tjek om prisen er positiv større end nul
        if (price <= 0)
        {
            throw new ArgumentException("Prisen skal være positiv større end nul.");
        }

        // KSC: Hvis valutaen allerede findes, opdater prisen. Ellers tilføj den.
        if (priceMapping.ContainsKey(currencyName))
        {
            priceMapping[currencyName] = price;
        }
        else
        {
            priceMapping.Add(currencyName, price);
        }
    }


    /// <summary>
    /// KSC: Henter prisen for en kryptovaluta, hvis valutaen allerede er sat.
    /// KSC: Hvis valutaen ikke findes, kaster funktionen en ArgumentException.
    /// KSC: Case sensitiv opslag og ingen trim etc.
    /// </summary>
    /// <param name="currencyName">Navnet på den kryptovaluta, der ønskes prisen for.</param>
    /// <returns>Prisen på den angivne kryptovaluta.</returns>
    /// <exception cref="ArgumentException">Kastes hvis valutaen ikke er fundet i priceMapping.</exception>
    public double GetPricePerUnit(string currencyName)
    {
        // Tjek om valutaen findes i priceMapping
        if (!priceMapping.ContainsKey(currencyName))
        {
            throw new ArgumentException($"Valuta '{currencyName}' er ikke fundet.");
        }

        // Returner prisen
        return priceMapping[currencyName];
    }

    /// <summary>
    /// Konverterer fra en kryptovaluta til en anden. 
    /// Hvis en af de angivne valutaer ikke findes, kaster funktionen en ArgumentException
    /// </summary>
    /// <param name="fromCurrencyName">Navnet på den valuta, der konverteres fra</param>
    /// <param name="toCurrencyName">Navnet på den valuta, der konverteres til</param>
    /// <param name="amount">Beløbet angivet i valutaen angivet i fromCurrencyName</param>
    /// <returns>Værdien af beløbet i toCurrencyName</returns>
    public double Convert(string fromCurrencyName, string toCurrencyName, double amount)
    {
        // KSC: Tjek om begge valutaer findes i priceMapping
        if (!priceMapping.ContainsKey(fromCurrencyName))
        {
            throw new ArgumentException($"Valuta '{fromCurrencyName}' er ikke fundet.");
        }
        if (!priceMapping.ContainsKey(toCurrencyName))
        {
            throw new ArgumentException($"Valuta '{toCurrencyName}' er ikke fundet.");
        }

        // Udregn konverteringen baseret på priserne
        double fromPrice = priceMapping[fromCurrencyName];
        double toPrice = priceMapping[toCurrencyName];

        // Returner konverteret beløb 
        return (amount * fromPrice) / toPrice;
    }



}

