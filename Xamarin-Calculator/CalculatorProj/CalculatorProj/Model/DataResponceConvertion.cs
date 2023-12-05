namespace CalculatorProj.Model
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class DataResponceConvertion
    {
        public string Success { get; set; }
        public int Timestamp { get; set; }
        public string Basecurrency { get; set; }
        public string Date { get; set; }
        public IDictionary<string, string> Rates { get; set; }

    }
}
