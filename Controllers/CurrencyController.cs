using Microsoft.AspNetCore.Mvc;
using Expense_Tracker_WebApp.Data;
using Expense_Tracker_WebApp.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_WebApp.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string ApiUrl = "https://api.exchangerate-api.com/v4/latest/USD"; // Base API URL

        public CurrencyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currencies = await _context.Currencies.ToListAsync();
            ViewData["AvailableCurrencies"] = await GetAvailableCurrencies();
            return View(currencies);
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Length != 3)
            {
                TempData["Error"] = "Invalid currency code.";
                return RedirectToAction(nameof(Index));
            }

            await AddCurrencyAsync(currencyCode);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRates()
        {
            await UpdateAllExchangeRatesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task AddCurrencyAsync(string currencyCode)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<dynamic>(response);
                    var rates = data.rates;

                    if (rates[currencyCode] == null)
                    {
                        TempData["Error"] = "Currency code not found in the API.";
                        return;
                    }

                    var exchangeRateToPKR = Convert.ToDecimal(rates.PKR) / Convert.ToDecimal(rates[currencyCode]);

                    var currency = _context.Currencies.FirstOrDefault(c => c.Code == currencyCode);
                    if (currency == null)
                    {
                        // Add new currency
                        currency = new Currency
                        {
                            Code = currencyCode,
                            ExchangeRate = exchangeRateToPKR,
                            LastUpdated = DateTime.Now
                        };
                        _context.Currencies.Add(currency);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        TempData["Error"] = "Currency already exists.";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding currency: {ex.Message}");
                    TempData["Error"] = "Error adding currency.";
                }
            }
        }

        private async Task UpdateAllExchangeRatesAsync()
        {
            var currencies = await _context.Currencies.ToListAsync();
            foreach (var currency in currencies)
            {
                await UpdateExchangeRateAsync(currency.Code);
            }
        }

        private async Task UpdateExchangeRateAsync(string currencyCode)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(ApiUrl);
                    var data = JsonConvert.DeserializeObject<dynamic>(response);
                    var rates = data.rates;

                    var exchangeRateToPKR = Convert.ToDecimal(rates.PKR) / Convert.ToDecimal(rates[currencyCode]);

                    var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.Code == currencyCode);
                    if (currency != null)
                    {
                        currency.ExchangeRate = exchangeRateToPKR;
                        currency.LastUpdated = DateTime.Now;
                        _context.Update(currency);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating exchange rate for {currencyCode}: {ex.Message}");
                }
            }
        }

        private async Task<IEnumerable<string>> GetAvailableCurrencies()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(ApiUrl);
                var data = JsonConvert.DeserializeObject<dynamic>(response);
                var rates = data.rates;

                return rates.Keys;
            }
        }
    }
}
