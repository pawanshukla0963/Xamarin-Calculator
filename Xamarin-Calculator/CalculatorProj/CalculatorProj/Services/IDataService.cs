namespace CalculatorProj.Services
{
    using CalculatorProj.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataService
    {
        Task<List<Currency>> GetCurrencies();
        Task<string> GetConvertion(string from, string to, float ammount);
    }
}
