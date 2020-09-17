# CurrencyIntoWords
Task: convert number to text with dollar currency. Numbers from 0 to 999 999 999,99 with comma as a decimal separator.
## Client
Client side application is written in WPF. It has two TextBoxes, only the top one is editable. In this TextBox user has to put a number to convert. If the number is in the walid format and doesn't exceed the boundaries, the button *Convert* will become available. Pressing enter will have the same effect as pressing button *Convert*. The output of the conversion will be visible in the second TextBox on the bottom of the window.
## Server
Server part is written in ASP.NET. The most interesting are *NumberToWordsController* and it's method *Convert* which does the conversion. 
### Variables
#### Dictionary<int, string> Digits
Contains the digits like thousand and million and their text representations. **In case we will want to extend the application, it will be enough just to add billions to this dictionary, keeping the descending order. This way we can easily extend the convertion to work with every number fitting into the size of int datatype**
#### Dictionary<int, string> BasicNumbers
Only necessary numbers with their text representations. every other number can be generated based on this number.
### Methods
#### GetStringFrom2DigitsNumber
Converts two digits numbers to words. Used to convert cents and every other two-digits number.
#### GetStringFrom3DigitsNumber
Converts three digits number to words. Used to convert every three-digits number.
#### GetTextFromNumber 
Converts any number within a given range to words. Called recursively.
#### ConvertNumber
Main GET-Method to convert number to string.	