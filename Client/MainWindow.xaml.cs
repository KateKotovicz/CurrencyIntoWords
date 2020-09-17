using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CurrencyToWordsConvertor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        #region Private members

        private string _number;
        private string _convertedNumber;

        #endregion

        #region Public Properties

        public string Number 
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged("Number");
                OnPropertyChanged("IsNumberValid");
            } 
        }

        public string ConvertedNumber
        {
            get => _convertedNumber;
            set
            {
                _convertedNumber = value;
                OnPropertyChanged("ConvertedNumber");
            }
        }

        public bool IsNumberValid => !string.IsNullOrEmpty(Number) && Regex.IsMatch(Number, @"^\d{1,9}(,\d{1,2})?$");

        #endregion

        #region Methods

        public string ConvertNumber(string number)
        {
            try
            {
                WebRequest request = WebRequest.Create(
                  "https://localhost:44331/NumberToWords/Convert/"+ Uri.EscapeUriString(number)); 
                 WebResponse response = request.GetResponse();
                string responseFromServer = string.Empty;
                using (Stream dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    responseFromServer = reader.ReadToEnd();
                }
                response.Close();
                return responseFromServer.Replace("\"","");
            }
            catch (Exception e)
            {
                MessageBox.Show("The application can't connect to server.","Connection Error");
                return string.Empty;
            }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();            
        }

        #region Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {           
            ConvertedNumber = ConvertNumber(Number);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsNumberValid)
            {
                ConvertedNumber = ConvertNumber(Number);
            }
        }

        #endregion
    }
}
