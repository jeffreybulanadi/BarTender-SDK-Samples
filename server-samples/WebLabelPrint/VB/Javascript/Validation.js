// Validation functions for use with BarTender prompts that use validation.

// Converts the event into a char code
function GetCharcode(keyEvent)
{
   return (keyEvent.which) ? keyEvent.which : event.keyCode;
}

// Changes the case of the string to upper or lower depending on alphacase
// alphaCase UPPER = upper case
// alphaCase LOWER = lower case
// alphaCase ALL   = no change
function ChangeCase(string, alphaCase)
{
   if (alphaCase == "UPPER")
      return string.toUpperCase();
   if (alphaCase == "LOWER")
      return string.toLowerCase();
   return string;
}

// Returns true if the char is a control character.
function IsControlChar(charCode)
{
   return (charCode < 32);
}

// Returns true for control keys and 0-9
function IsNumeric(charCode)
{
   return (IsControlChar(charCode) || (charCode >= 48 && charCode <= 57));
}

// Strips out all non numeric chars from a control
function IsNumericOnChange(control, alphaCase)
{
   var finalValue = "";
   var controlValue = control.value;
   for (i = 0; i < controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsNumeric(c))
         finalValue+=String.fromCharCode(c);
   }

   control.value = finalValue;
}

// Strips out all non numeric chars from a control and caps at a max value
function IsNumericOnChangeMax(control, maxValue)
{
   IsNumericOnChange(control, "");
   if(control.value > maxValue)
      control.value = maxValue;
   else if(control.value<1)
      control.value = 1;
}

// Returns true for control keys and alpha chars
function IsAlpha(charCode)
{
   var character = String.fromCharCode(charCode);

   // Non alpha chars are the same in upper and lowercase
   return (IsControlChar(charCode) || (character.toUpperCase() != character.toLowerCase()));
}

//strips out all non alpha chars from a control and changes the case
function IsAlphaOnChange(control, alphaCase)
{
   var finalValue="";
   var controlValue = control.value;
   for (i = 0; i < controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsAlpha(c))
         finalValue += String.fromCharCode(c);
   }

   finalValue = ChangeCase(finalValue, alphaCase);
   control.value = finalValue;
}

// Returns true for control keys and 0-9, a-z and A-Z
function IsAlphaNumeric(charCode)
{
   return (IsNumeric(charCode)||IsAlpha(charCode));
}

// Strips out all non alphanumeric chars from a control and changes the case
function IsAlphaNumericOnChange(control, alphaCase)
{
   var finalValue="";
   var controlValue = control.value;
   for (i = 0; i < controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsAlphaNumeric(c))
      finalValue+=String.fromCharCode(c);
   }
	 
   finalValue = ChangeCase(finalValue, alphaCase);
   control.value = finalValue;
}

// Returns true for control keys or 0-9, a-f and A-F
function IsHex(charCode)
{
   return (IsNumeric(charCode) || (charCode >= 97 && charCode <= 102) || (charCode>=65 && charCode<=70));
}

//strips out all non hex chars from a control and changes the case
function IsHexOnChange(control, alphaCase)
{
   var finalValue="";
   var controlValue = control.value;
   for (i = 0; i<controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsHex(c))
         finalValue+=String.fromCharCode(c);
   }
	 
   finalValue = ChangeCase(finalValue, alphaCase);
   control.value = finalValue;
}

// Returns true if it is a currency character
function IsCurrency(charCode)
{
   return IsCustom(charCode,"1234567890$.,\u00A2\u00A3\u00A4\u00A5\u0192\u060B\u09F2\u09F3\u0AF1\u0BF9\u0E3F\u17DB\u2133\uFDFC\u20A0\u20A1\u20A2\u20A3\u20A4\u20A5\u20A6\u20A7\u20A8\u20A9\u20AA\u20AB\u20AC\u20AD\u20AE\u20AF\u20B0\u20B1\u20B2\u20B3\u20B4\u20B5\uFF04\uFFE5\uFFE6\uFFE0\uFFE1");
}

// Strips out all non currency chars from a control and changes the case
function IsCurrencyOnChange(control, alphaCase)
{
   var finalValue="";
   var controlValue = control.value;
   for(i=0; i<controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsCurrency(c))
         finalValue+=String.fromCharCode(c);
   }
	 
   finalValue = ChangeCase(finalValue, alphaCase);
   control.value = finalValue;
}

// Returns true if the charcode is in the charlist
function IsCustom(charCode, charList)
{
   if(IsControlChar(charCode))
      return true;

   var character = String.fromCharCode(charCode);
   var regexPattern = "["+charList+"]";
   var letterRegex = new RegExp(regexPattern,"");
   return letterRegex.test(character);
}

//strips out all non specified custom chars from a control and changes the case
function IsCustomOnChange(control, charList, alphaCase)
{
   var finalValue="";
   var controlValue = control.value;
   for(i=0; i<controlValue.length; i++)
   {
      var c = controlValue.charCodeAt(i);
      if(IsCustom(c,charList))
         finalValue+=String.fromCharCode(c);
   }
	 
   finalValue = ChangeCase(finalValue, alphaCase);
   control.value = finalValue;
}

// All chars are accepted so just changes the case
function IsAllOnChange(control, alphaCase)
{
   control.value = ChangeCase(control.value, alphaCase);
}