namespace CalculatorProj.Model
{
    public class Currency
    {
        public string Symbol { get; private set; }
        public string Description { get; private set; }

        public Currency(string symbol, string description)
        {
            this.Symbol = symbol;
            this.Description = description;
        }
    }
}
