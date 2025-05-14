// Exception that that details the error that occurred.
function ClientPrintException(message, errorCode)
{
   this.Message = message;
   this.ErrorCode = errorCode;
   
   this.name = "ClientPrintException";   
}

// Enumeration that designates error codes for use with the ClientPrintException
ClientPrintException.ErrorCode = 
{
   BarTenderPrintClientNotInstalled : 0,
   PrintLicenseCreationFailed : 1,
   SpoolingFailed: 2
}

// Helper function that gets the matching server printer for a given client printer.
function GetMatchingServerPrinter(clientModelName, serverPrinterNames, serverModelNames)
{
   var compatiblePrinterName = null;
   for (var i = 0; i < serverModelNames.length; i++)
   {
      if (serverModelNames[i]==clientModelName)
      {
         compatiblePrinterName = serverPrinterNames[i];
         break;
      }
   }
    
   return compatiblePrinterName;
}

// Represents a client printer
function ClientPrinter(printerName, modelName, isDefault, matchingServerPrinterName)
{
   this.PrinterName = printerName;
   this.ModelName = modelName;
   this.MatchingServerPrinterName = matchingServerPrinterName;
   this.IsDefault = isDefault;
}

// Gets the client printers. 
// Throws: ClientPrintException if BarTenderPrintClient is not installed.
// Returns: Array of ClientPrinter objects that describe the printers installed on the client. 
// The MatchingServerPrinterName value of the ClientPrinter object is undefined.
function GetClientPrinters()
{
   // Get the list of client printers
   var objPrinters;
   var clientPrinterCSV;
    
   try
   {
      objPrinters = new ActiveXObject("BarTenderPrintClient.Printers");
      if(!objPrinters)
         throw(0);
      clientPrinterCSV = objPrinters.GetPrinterAndModelNames();
   }
   catch(exception)
   {
      throw new ClientPrintException("BarTenderPrintClient is not installed.", ClientPrintException.ErrorCode.BarTenderPrintClientNotInstalled);
   }

   var clientPrinters = clientPrinterCSV.split("\n");
   var defaultPrinterName = objPrinters.GetDefaultPrinterName();
   
   var clientPrinterObjects = [];

   for(var i = 0; i < clientPrinters.length; i++)
   {
      // The string is empty for some reason
      if (clientPrinters[i].length == 0) break; 
     
      var printerInfo = clientPrinters[i].split(",");
      var name = printerInfo[0];
      var model = printerInfo[1];
         
      var isDefault = (defaultPrinterName == name ? true : false);
      var clientPrinter = new ClientPrinter(name, model , isDefault, undefined);
                   
      clientPrinterObjects[i] = clientPrinter; 
   }
   
   return clientPrinterObjects;
}

// Gets the client printer objects that match drivers on the server. 
// Throws: ClientPrintException if BarTenderPrintClient is not installed.
// Returns: Array of ClientPrinter objects that describe the client printers with drivers that match server printers.
function GetMatchingClientPrinters(clientPrinterObjects, serverPrinterCSV, serverDriverCSV)
{
   var matchingClientPrinters = [];

   if(clientPrinterObjects != undefined)
   {
      var  serverDrivers = serverDriverCSV.split(",");
      var  serverPrinters = serverPrinterCSV.split(",");

      var numMatches = 0;

      // Find printers with matching drivers and add them to the printer array.
      for(var i = 0; i < clientPrinterObjects.length; i++)
      {
         var clientPrinter = clientPrinterObjects[i];
         var compatiblePrinter = GetMatchingServerPrinter(clientPrinter.ModelName, serverPrinters, serverDrivers);
            
         if(compatiblePrinter)
         {
            clientPrinter.MatchingServerPrinterName = compatiblePrinter;
                         
            matchingClientPrinters[numMatches] = clientPrinter; 
            numMatches++;
         }
      }
   }
   return matchingClientPrinters;
}

// Gets the license for the selected printer
// Returns: The print license for the printer
// Throws: ClientPrintException on error
// Error Codes:
// ErrorCode.BarTenderPrintClientNotInstalled if there is a problem with the BarTenderPrintClient.
// ErrorCode.PrintLicenseCreationFailed if unable to create a printing license.
function GetPrintLicense(printerName)
{
   var printLicense = "";
   var objPrinter;
   try
   {
      var objPrinter = new ActiveXObject("BarTenderPrintClient.Printer");
      if(!objPrinter)
         throw(0);
   }
   catch(exception)
   {
      throw new ClientPrintException("BarTenderPrintClient is not installed.",ClientPrintException.ErrorCode.BarTenderPrintClientNotInstalled);
   }

   printLicense = objPrinter.CreatePrintToFileLicenseScriptSafe(printerName);
   var error = objPrinter.GetLastErrorMessage();
   if (error != "")
   {
      throw new ClientPrintException(error,ClientPrintException.ErrorCode.PrintLicenseCreationFailed);
   }

   return printLicense; 
}
    
    
// Prints the client print code to the selected printer 
// Throws: ClientPrintException on error
// Error Codes:
// ErrorCode.BarTenderPrintClientNotInstalled if there is a problem with the BarTenderPrintClient.
// ErrorCode.SpoolingFailed if unable to spool to the printer.
function ClientPrint(printerName, printJobName, printCode)
{
   var objPrinter;
   try 
   {
      objPrinter = new ActiveXObject("BarTenderPrintClient.Printer");
      if(!objPrinter)
         throw(0);
   }
   catch (e)
   {
      throw new ClientPrintException("BarTenderPrintClient is not installed.",ClientPrintException.ErrorCode.BarTenderPrintClientNotInstalled);
   }
            
   var success = objPrinter.SendPrintCode(printerName, printJobName, printCode);
   if(success == false)
   {
      var error = objPrinter.GetLastErrorMessage();
      throw new ClientPrintException(error,ClientPrintException.ErrorCode.SpoolingFailed);
   }
}