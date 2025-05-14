// This javascript file contains methods that support client printing for the Web Label Print sample application for the 
// Seagull.BarTender.Print SDK. If this file is used, the BarTenderPrintClient.js file must be included on the web page before
// this file.


// Writes an error message regarding the inability to install the BarTenderPrintClient that is based on which
// browser the client is using.
function OutputPrintClientError(messagesControlId, messagePanelControlId)
{
   if (navigator.appName == "Microsoft Internet Explorer")// It can get here if a user on IE does not click 'yes' on the ActiveX install or if the install is blocked.
   {
      OutputAlert("Internet printing is disabled because the BarTender Print Client ActiveX control is not installed.<br/>To enable Internet printing, allow the BarTender Print Client ActiveX control to install. In Internet Explorer 7, look for a yellow bar at the top of the screen to enable the control.<br/>", messagesControlId, messagePanelControlId);
   }
   else // Since activex will not run on firefox or other non IE-browsers without special plugin, they will usually cause this error
   {
      OutputAlert("Internet printing is disabled because the BarTender Print Client ActiveX control is not installed.<br/>Internet printing is only supported on Internet Explorer.<br/>", messagesControlId, messagePanelControlId);
   }
}

// Writes input error message to the message panel.
function OutputAlert(message, messagesControlId, messagePanelControlId)
{
   var messagesControl = document.getElementById(messagesControlId);
   if (messagesControl.innerHTML != "")
      messagesControl.innerHTML += "<hr/>";
   messagesControl.innerHTML += message;

   var messagesPanel = document.getElementById(messagePanelControlId);
   messagesPanel.style.display = "block";
}

// Gets the client printers from the client and puts printers with drivers that match
// drivers on the server into the printer list
function AddClientPrintersToList(serverPrinterString, serverDriverString, selectedPrinter, printerNameFromLabelFormat, listPrinterControlId, messagesControlId, messagePanelControlId, btnPrintControlId, btnPreviewControlId)
{
   var clientPrinterObjects;
   try
   {
      clientPrinterObjects = GetMatchingClientPrinters(GetClientPrinters(), serverPrinterString, serverDriverString);
   }
   catch(exception)
   {
      if(exception.name == "ClientPrintException")
         OutputPrintClientError(messagesControlId,messagePanelControlId);
   }
   
   var lstPrinters = document.getElementById(listPrinterControlId);
   
   if(clientPrinterObjects == undefined || clientPrinterObjects.length == 0)
   {
      lstPrinters.options[0] = new Option("Matching Printers Not Found");
      lstPrinters.selectedItem = 0;
      lstPrinters.disabled = true;
        
      if(btnPrintControlId != null)
      {
         var btnPrint = document.getElementById(btnPrintControlId);
         btnPrint.disabled = true;
      }
        
      if(btnPreviewControlId != null)
      {
         var btnPreview = document.getElementById(btnPreviewControlId);
         btnPreview.disabled = true;
      }

      OutputAlert("Matching printers were not found. <br /><br />Printer drivers must be installed on both on the web server and the client computer.<br/>", messagesControlId, messagePanelControlId);
   }
   else
   {
      var printerWasSelected = false;
      var defaultPrinterIndex = 0;
      
      for(var i=0; i<clientPrinterObjects.length; i++)
      {
         var clientPrinter = clientPrinterObjects[i];
         var printerValue = clientPrinter.PrinterName + ",client," + clientPrinter.MatchingServerPrinterName;
         lstPrinters.options[i]=new Option(clientPrinter.PrinterName+" [On Client]",printerValue);
             
         // Select the printer if it is either:
         // A. The last selected printer or
         // B. A compatible printer to the one assigned to the format and the last printer wasn't set
         if((selectedPrinter == clientPrinter.PrinterName) || (!printerWasSelected && (printerNameFromLabelFormat == clientPrinter.MatchingServerPrinterName)))
         {
            printerWasSelected = true;
            lstPrinters.options[i].selected = true;
         }
             
         if(clientPrinter.IsDefault)
         {
            defaultPrinterIndex = i;
         }
      }
      
      // If a printer wasn't selected above, try to select the default printer. If the default
      // printer doesn't exist or doesn't have a compatible printer, the first printer on the list will be selected.
      if(clientPrinterObjects.length > 0 && !printerWasSelected)
      {
         lstPrinters.options[defaultPrinterIndex].selected = true;
      }      
   }
}

// Gets the row style for alternating table rows
function GetAlternatingRowStyle(rowNum)
{
   if(rowNum % 2 == 0)
   {
      return "TableEvenRow";
   }
   else
   {
      return "TableOddRow";
   } 
}

// Displays client printers in a table
function DisplayClientPrinters(serverPrinterCSV, serverDriverCSV, tblClientPrintersControlId, tblAvailableClientsControlId, panelClientPrintingControlId, messagesControlId, messagePanelControlId, noPrintersControlId, noMatchingPrintersControlId)
{
   var clientPrinterObjects;
   var matchingClientPrinterObjects;

   try
   {
      clientPrinterObjects = GetClientPrinters();
      matchingClientPrinterObjects = GetMatchingClientPrinters(clientPrinterObjects, serverPrinterCSV, serverDriverCSV);
   }
   catch(exception)
   {
      if(navigator.appName == "Microsoft Internet Explorer")// It can get here if a user on IE does not click 'yes' on the ActiveX install or if the install is blocked.
      {
         OutputAlert("In order to view printers installed on the client computer, please allow the BarTender Print Client ActiveX control to be installed.<br/>",messagesControlId, messagePanelControlId);
      }
      else // Since activex will not run on firefox or other non IE-browsers without special plugin, they will usually cause this error
      {
         OutputAlert("Viewing printers installed on the client computer is only supported using Internet Explorer.<br/>",messagesControlId, messagePanelControlId);
      }
      var panelClientPrinting = document.getElementById(panelClientPrintingControlId);
      panelClientPrinting.style.display = "none";
        
      return;
   }
   
   var tblPrinters = document.getElementById(tblClientPrintersControlId);
   var tblAvailablePrinters = document.getElementById(tblAvailableClientsControlId);
   
   if(clientPrinterObjects == undefined || clientPrinterObjects.length == 0)
   {
      // Display error if no client printers.
      tblPrinters.style.display = 'none';
      var noPrintersAlert = document.getElementById(noPrintersControlId);
      noPrintersAlert.style.display = "block";
   }
   else
   {
      // Write all client printers into client printer table
      for(var i=0; i<clientPrinterObjects.length; i++)
      {
         // Insert a new row for the printer
         var row = tblPrinters.insertRow(i+1);
         var printerNameCell = row.insertCell(0);
         var printerDriverCell = row.insertCell(1);
         var printerDefaultCell = row.insertCell(2);
           
         row.className = GetAlternatingRowStyle(i);
         
         var clientPrinter = clientPrinterObjects[i];
           
         printerNameCell.innerHTML=clientPrinter.PrinterName;
         printerDriverCell.innerHTML=clientPrinter.ModelName;
           
         if(clientPrinter.IsDefault)
         {
            printerDefaultCell.innerHTML="True";
         }
         else
         {
            printerDefaultCell.innerHTML="False";
         }
      }
   }
   
   if(matchingClientPrinterObjects == undefined || matchingClientPrinterObjects.length == 0)
   {
      // Display error if no matching server printers to client printers.
      tblAvailablePrinters.style.display = 'none';
      var noPrintersAlert = document.getElementById(noMatchingPrintersControlId);
      noPrintersAlert.style.display = "block";
   }
   else
   {
      // Write all matching client printers into matching client printer table
      for(var i=0; i<matchingClientPrinterObjects.length; i++)
      {
         row = tblAvailablePrinters.insertRow(i+1);
         var availablePrinterNameCell = row.insertCell(0);
         var availablePrinterMatchCell = row.insertCell(1);
            
         row.className = GetAlternatingRowStyle(i);
         
         var clientPrinter = matchingClientPrinterObjects[i];

         availablePrinterNameCell.innerHTML=clientPrinter.PrinterName;
         availablePrinterMatchCell.innerHTML=clientPrinter.MatchingServerPrinterName;
      }
   }
}

// Sets the license for the selected printer if it is a client printer
function SetPrintLicense(printer, printLicenseControlId, messagesControlId, messagePanelControlId)
{
   try
   {
      var printerInfo = printer.split(",");
      var printerName = printerInfo[0];
      var printerType = printerInfo[1];

      if(printerType=="client")
      {
         document.getElementById(printLicenseControlId).value = GetPrintLicense(printerName);
      }
   }
   catch(exception)
   {
      if(exception.name == "ClientPrintException")
      {
         if(exception.ErrorCode == ClientPrintException.ErrorCode.BarTenderPrintClientNotInstalled)
            OutputPrintClientError(messagesControlId,messagePanelControlId);
         else if(exception.ErrorCode == ClientPrintException.ErrorCode.PrintLicenseCreationFailed)
            OutputAlert("Unable to print. <br /><br />Reason: Failed to create print license.<br/>",messagesControlId,messagePanelControlId);
      }
      return;
   }     
}
    
    
// Prints the client print code to the selected printer 
function SampleClientPrint(selectedPrinter, printCodeControlId, messagesControlId, messagePanelControlId)
{
   var hiddenClientPrintCode = document.getElementById(printCodeControlId);
   
   if(hiddenClientPrintCode.value=="")
   {
      OutputAlert("Unable to print. Reason: No printer code was created.<br/>",messagesControlId,messagePanelControlId);
   }
   else
   {
      try 
      {
         ClientPrint(selectedPrinter, "Web Label Print", hiddenClientPrintCode.value);
         OutputAlert("The print job was spooled to " + selectedPrinter + " on the client computer.<br/>",messagesControlId,messagePanelControlId);
      }
      catch (exception)
      {
         if(exception.name == "ClientPrintException")
         {
            if(exception.ErrorCode == ClientPrintException.ErrorCode.SpoolingFailed)
               OutputAlert("Spooling failed while printing to " + selectedPrinter + "<br/><br/>Error Message: " + exception.Message + "<br/>",messagesControlId,messagePanelControlId);
            else if(exception.ErrorCode == ClientPrintException.ErrorCode.BarTenderPrintClientNotInstalled)
               OutputPrintClientError(messagesControlId,messagePanelControlId);
         }
      }
   }
}