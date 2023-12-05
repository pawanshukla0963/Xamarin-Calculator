namespace CalculatorProj.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DataResponceRates
    {
        public string Success { get; set; }
        public IDictionary<string, string> Symbols { get; set; }
    }
}
