namespace CalculatorProj.ViewModels
{
    using System.Globalization;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System;
    using CalculatorProj.Model;
    using CalculatorProj.Services;

    public class MainPageViewModel :  INotifyPropertyChanged
    {
        #region Fields
        private string _baseCurrency;
        private string targetCurrency;
        private string _resultString;
        private string targetCurrencyString;
        private OperationType _operation;
        private string _currentInput;
        private float _firstOperand;
        private float _secondOperand;
        private List<Currency> _currencies = new List<Currency>();
        private readonly IDataService _dataService;
        #endregion
        #region Props
        public ICommand NumButtonCommand { get; private set; }
        public ICommand EqualsButtonCommand { get; private set; }
        public ICommand ClearButtonCommand { get; private set; }
        public ICommand EraseButtonCommand { get; private set; }
        public ICommand DotButtonCommand { get; private set; }
        public ICommand OpButtonCommand { get; private set; }
       
        public string TargetCurrencyString
        {
            get
            {
                return targetCurrencyString;
            }
            set
            {
                if (targetCurrencyString != value)
                {
                    targetCurrencyString = value;
                    OnPropertyChanged("TargetCurrencyString");
                }
            }
        }
        public string ResultString
        {
            get
            {
                return _resultString;
            }
            set
            {
                if (_resultString != value)
                {
                    _resultString = value;
                    OnPropertyChanged("ResultString");
                    ConvertRates();
                }
            }
        }
        public string CurrentInput
        {
            get
            {
                return _currentInput;
            }
            set
            {

                if (_currentInput != value)
                {
                    _currentInput = value;
                    OnPropertyChanged("CurrentInput");
                }
            }
        }
        public OperationType Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                if (_operation != value)
                {
                    _operation = value;
                }
            }
        }
        public List<Currency> Currencies
        {
            get
            {
                return _currencies;
            }
            set
            {
                if (_currencies != value)
                {
                    _currencies = value;
                    OnPropertyChanged("Currencies");

                }
            }
        }
        #endregion
        #region Event
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Constructor
        public MainPageViewModel(IDataService dataService)
        {
            NumButtonCommand = new Command(NumButtonPressed);
            EqualsButtonCommand = new Command(EqualsButtonPressed);
            ClearButtonCommand = new Command(ClearButtonPressed);
            EraseButtonCommand = new Command(EraseButtonPressed);
            DotButtonCommand = new Command(DotButtonPressed);
            OpButtonCommand = new Command(OpButtonPressed);
            _baseCurrency = "";
            targetCurrency = ""; 
            ResultString = "0";
            targetCurrencyString = "0";
            CurrentInput = "0";
            _firstOperand = 0;
            _secondOperand = 0;
            Operation = OperationType.Initial;
            _dataService = dataService;
        }
        #endregion
        #region Convertions
        public async  void GetCurrencies()
        {

            Currencies = await _dataService.GetCurrencies();
            if (Currencies.Count==0)
            {
                Application.Current.MainPage.DisplayAlert("No connection", "It seems your internet connection stopped working. Cannot retrieve curencies. ", "OK");

            }
        }
        private async  void ConvertRates()
        {
            if (_baseCurrency != "" && targetCurrency != "" &&ResultString!="0" )
            {
                TargetCurrencyString= await _dataService.GetConvertion(_baseCurrency, targetCurrency, float.Parse(ResultString));
                if (TargetCurrencyString == "0")
                {
                    Application.Current.MainPage.DisplayAlert("No connection", "It seems your internet connection stopped working. Cannot retrieve curencies. ", "OK");
                }
                OnPropertyChanged(TargetCurrencyString);
            }
            if (ResultString == "0")
            {
                TargetCurrencyString = "0";
                OnPropertyChanged(TargetCurrencyString);
            }
        }
        public void UpdateBaseCurrency(string symbol)
        {
            _baseCurrency = symbol;
            ConvertRates();
        }
        public void UpdateTargerCurrency(string symbol)
        {
            targetCurrency = symbol;
            ConvertRates();
            
            
        }
        #endregion
        #region OperationButtonsCommands
        private void OpButtonPressed(object obj)
        {

            var operationButton = (OperationType)obj;
            switch (Operation)
            {
                case OperationType.Initial:
                    {
                        Operation = operationButton;
                        _firstOperand = float.Parse(CurrentInput, CultureInfo.InvariantCulture.NumberFormat);
                        CurrentInput += getSymbolByEnum(operationButton); 
                        OnPropertyChanged(CurrentInput);
                        return;
                    }

                case OperationType.NotSet:
                    {
                        Operation = operationButton;
                        _secondOperand = 0;
                        CurrentInput += getSymbolByEnum(operationButton);
                        OnPropertyChanged(CurrentInput);
                        return;
                    }
                case OperationType.Add:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = AddOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, operationButton);

                        }
                        return;

                    }
                case OperationType.Sub:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = SubOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, operationButton);

                        }
                        return;
                    }
                case OperationType.Multi:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = multiOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, operationButton);

                        }
                        return;
                        return;
                    }
                case OperationType.Div:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = DivOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, operationButton);

                        }
                        return;
                    }
                default:
                    {
                        break;
                    }
            }

        }

        private void EraseButtonPressed(object obj)
        {
            if (CurrentInput.Length < 2)
            {
                CurrentInput = "0";

            }
            else
            {
                if (CurrentInput[CurrentInput.Length-1]=='+'|| CurrentInput[CurrentInput.Length - 1] == '-' || CurrentInput[CurrentInput.Length - 1] == '*' || CurrentInput[CurrentInput.Length - 1] == '/' )
                {
                    Operation = OperationType.NotSet;

                }
                CurrentInput = CurrentInput.Remove(CurrentInput.Length - 1, 1);
            }
            OnPropertyChanged(CurrentInput);
        }
        private void ClearButtonPressed(object obj)
        {
            Operation = OperationType.Initial;
            _firstOperand = 0;
            _secondOperand = 0;
            ResultString = "0";
            CurrentInput = "0";
            OnPropertyChanged(ResultString);
            OnPropertyChanged(CurrentInput);
        }
        private void EqualsButtonPressed(object obj)
        {
            switch (Operation)
            {
                case OperationType.Initial:
                    {
                        ResultString = CurrentInput;
                        OnPropertyChanged(ResultString);
                        break;
                    }

                case OperationType.NotSet:
                    {
                        
                        ResultString = CurrentInput;
                        OnPropertyChanged(ResultString);
                        break;

                    }
                case OperationType.Add:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = AddOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, OperationType.NotSet);

                        }
                        return;

                    }
                case OperationType.Sub:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = SubOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, OperationType.NotSet);

                        }
                        return;
                    }
                case OperationType.Multi:
                    {

                        if (retrieveOperands(Operation))
                        {
                            string temp = multiOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, OperationType.NotSet);

                        }
                        return;
                    }
                case OperationType.Div:
                    {
                        if (retrieveOperands(Operation))
                        {
                            string temp = DivOperands(_firstOperand, _secondOperand);
                            updateValuesAfterOperation(temp, OperationType.NotSet);

                        }
                        return;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        #endregion
        #region Number or Dot Button Command
        private void DotButtonPressed(object obj)
        {
            if (Operation == OperationType.NotSet || Operation == OperationType.Initial)
            {
                string[] splitedStrings = CurrentInput.Split('.');
                if (splitedStrings.Length == 1)
                {
                    CurrentInput += '.';
                    OnPropertyChanged(CurrentInput);
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "You have allready pressed Dot", "OK");
                }
            }
            else
            {
                char currentOperator = getSymbolByEnum(Operation);
                string[] splitedStrings = CurrentInput.Split(currentOperator);
                {
                    if (splitedStrings[1] == "")
                    {
                        CurrentInput += "0.";
                        OnPropertyChanged(CurrentInput);

                    }
                    else
                    {

                        if (splitedStrings[1].Contains("."))
                        {
                            Application.Current.MainPage.DisplayAlert("Alert", "You have allready pressed Dot", "OK");
                        }
                        else
                        {
                            CurrentInput += ".";
                            OnPropertyChanged(CurrentInput);
                        }
                    }
                }

            }
            
        }
        private void NumButtonPressed(object digit)
        {
            AddDigitToCurrentInput(digit.ToString());
        }
        #endregion
        #region Helping Methods

        private bool retrieveOperands(OperationType operation)
        {
            string resolveFirstOperand = CurrentInput.Split(getSymbolByEnum(operation))[0];
            _firstOperand = float.Parse(resolveFirstOperand, CultureInfo.InvariantCulture.NumberFormat);
            string resolveSecondOperand = CurrentInput.Split(getSymbolByEnum(operation))[1];
            if (resolveSecondOperand == "")
            {
                return false;
            }
            _secondOperand = float.Parse(resolveSecondOperand, CultureInfo.InvariantCulture.NumberFormat);
            return true;

        }

        private void updateValuesAfterOperation(string resultString, OperationType operationButton )
        {
            ResultString = resultString;
            CurrentInput = (ResultString + getSymbolByEnum(operationButton)).TrimEnd('.');           
            OnPropertyChanged(ResultString);
            OnPropertyChanged(CurrentInput);
            Operation = operationButton;
            _secondOperand = 0;
            _firstOperand = float.Parse(ResultString, CultureInfo.InvariantCulture.NumberFormat);

        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private void AddDigitToCurrentInput(string number)
        {
            
            if (CurrentInput.Substring(0, 1) == "0" && CurrentInput.Length==1)
            {
                CurrentInput = CurrentInput.Remove(0, 1);
            }
            CurrentInput += number;
            OnPropertyChanged(CurrentInput);
        }
        private string AddOperands(float _firstOperand, float _secondOperand)
        {
            return (_firstOperand + _secondOperand).ToString();
        }
        private string SubOperands(float _firstOperand, float _secondOperand)
        {

            return (_firstOperand - _secondOperand).ToString();
        }
        private string multiOperands(float _firstOperand, float _secondOperand)
        {

            return (_firstOperand * _secondOperand).ToString();
        }
        private string DivOperands(float _firstOperand, float _secondOperand)
        {
            if (_secondOperand != 0)
            {
                return (_firstOperand / _secondOperand).ToString();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Can not Divide by Zero", "OK");

                return "0";

            }
        }
        private char getSymbolByEnum(OperationType opType)
        {
            switch (opType)
            {
                case OperationType.Add:
                    {
                        return '+';
                    }
                case OperationType.Sub:
                    {
                        return '-';                        
                    }
                case OperationType.Multi:
                    {
                        return '*';

                    }
                case OperationType.Div:
                    {
                        return '/';
                    }
            }
            return '.';

        }      
        #endregion


    }

    public enum OperationType
    {
        NotSet,
        Add,
        Sub ,
        Multi ,
        Div,
        Initial,

    }
}
