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
   messagesPanel.style.display = "";
}

// Gets the matching server printer for a given client printer.
function GetMatchingServerPrinter(clientModel, serverPrinters, serverDrivers)
{
   var compatPrinter = null;
   for (var j = 0; j < serverDrivers.length; j++)
   {
      if (serverDrivers[j]==clientModel)
      {
         compatPrinter = serverPrinters[j];
         break;
      }
   }
    
   return compatPrinter;
}

// Gets the client printers from the client and puts printers with drivers that match
// drivers on the server into the printer list
function GetClientPrinters(serverPrinterString, serverDriverString, selectedPrinter, printerNameFromLabelFormat, lstPrinterControlId, messagesControlId, messagePanelControlId, btnPrintControlId, btnPreviewControlId)
{
   var lstPrinters = document.getElementById(lstPrinterControlId);
    
   var  serverDrivers = serverDriverString.split(",");
   var  serverPrinters = serverPrinterString.split(",");

   // Get the list of client printers
   var objPrinters;
   var clientPrinterString;
   var error = false;
   var numMatches = 0;
    
   try
   {
      objPrinters = new ActiveXObject("BarTenderPrintClient.Printers");
      clientPrinterString = objPrinters.GetPrinterAndModelNames();
   }
   catch(exception)
   {
      OutputPrintClientError(messagesControlId,messagePanelControlId);
      error = true;
   }
   if(!error)
   {
      var clientPrinters = clientPrinterString.split("\n");
      var printerWasSelected = false;
      var defaultPrinterName = objPrinters.GetDefaultPrinterName();
      var defaultPrinterIndex = 0;
      // Find printers with matching drivers and write them into the printer list
      for(var i = 0; i < clientPrinters.length; i++)
      {
         // The string is empty for some reason
         if (clientPrinters[i].length == 0) break; 
        
         var printerInfo = clientPrinters[i].split(",");
         var name = printerInfo[0];
         var model = printerInfo[1];
            
         var compatPrinter = GetMatchingServerPrinter(model,serverPrinters,serverDrivers);
            
         if(compatPrinter)
         {
            var printerValue = name+",client,"+compatPrinter;
            lstPrinters.options[numMatches]=new Option(name+" [On Client]",printerValue);
                
            // Select the printer if it is either:
            // A. The last selected printer or
            // B. A compatible printer to the one assigned to the format and the last printer wasn't set
            if((selectedPrinter == name) || (!printerWasSelected && (printerNameFromLabelFormat == compatPrinter)))
            {
               printerWasSelected = true;
               lstPrinters.options[numMatches].selected = true;
            }
                
            if(name == defaultPrinterName)
            {
               defaultPrinterIndex = numMatches;
            }
                
            numMatches++;            
         }
      }
        
      // If a printer wasn't selected above, try to select the default printer. If the default
      // printer doesn't exist or doesn't have a compatible printer, the first printer on the list will be selected.
      if(numMatches > 0 && !printerWasSelected)
      {
         lstPrinters.options[defaultPrinterIndex].selected = true;
      }
        
   }
   // No matching printers were found.
   if(numMatches==0)
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
}

// Displays client printers in a table
function DisplayClientPrinters(serverPrinterString, serverDriverString, tblClientPrintersControlId, tblAvailableClientsControlId, panelClientPrintingControlId, messagesControlId, messagePanelControlId)
{
   var tblPrinters = document.getElementById(tblClientPrintersControlId);
   var tblAvailablePrinters = document.getElementById(tblAvailableClientsControlId);
    
   var  serverDrivers = serverDriverString.split(",");
   var  serverPrinters = serverPrinterString.split(",");

   // Get the list of client printers
   var objPrinters;
   var clientPrinterString;
    
   try
   {
      objPrinters = new ActiveXObject("BarTenderPrintClient.Printers");
      clientPrinterString = objPrinters.GetPrinterAndModelNames();
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

   var clientPrinters = clientPrinterString.split("\n");
   var numMatches=0;
    
   // Get the default printer name
   var defaultPrinter = objPrinters.GetDefaultPrinterName();
    
   // Write all printers into the table
   for(var i=0; i<clientPrinters.length &&clientPrinters[i].length>0; i++)
   {
      var printerInfo = clientPrinters[i].split(",");
      var name = printerInfo[0];
      var model = printerInfo[1];
      var compatPrinter = GetMatchingServerPrinter(model,serverPrinters,serverDrivers);
        
      // Insert a new row for the printer
      var row = tblPrinters.insertRow(i+1);
      var printerNameCell = row.insertCell(0);
      var printerDriverCell = row.insertCell(1);
      var printerDefaultCell = row.insertCell(2);
        
      // Alternate coloring for the rows
      if(i % 2 == 0)
      {
         row.className = "TableEvenRow";
      }
      else
      {
         row.className = "TableOddRow";
      } 
        
      printerNameCell.innerHTML=name;
      printerDriverCell.innerHTML=model;
        
      if(defaultPrinter == name)
      {
         printerDefaultCell.innerHTML="True";
      }
      else
      {
         printerDefaultCell.innerHTML="False";
      }
        
      if(compatPrinter)
      {
         row = tblAvailablePrinters.insertRow(numMatches+1);
         var availablePrinterNameCell = row.insertCell(0);
         var availablePrinterMatchCell = row.insertCell(1);
            
         // Alternate coloring for the rows
         if(i % 2 == 0)
         {
            row.className = "TableEvenRow";
         }
         else
         {
            row.className = "TableOddRow";
         } 
            
         availablePrinterNameCell.innerHTML=name;
         availablePrinterMatchCell.innerHTML=compatPrinter;
            
         numMatches++;
      }
   }
    
   // Display error if no client printers.
   if(i==0)
   {
      tblPrinters.style.display = 'none';
      var alert = document.createElement("div");
      alert.innerHTML = "There are no printers installed on the client computer.";
      alert.className = "Alert";

      tblPrinters.parentNode.replaceChild(alert,tblPrinters);
   }
    
   // Display error if no matching server printers to client printers.
   if(numMatches==0)
   {
      tblAvailablePrinters.style.display = 'none';
      var alert = document.createElement("div");
      alert.innerHTML = "There are no printers installed on the client computer with drivers that match printers installed on the web server computer.";
      alert.className = "Alert";

      tblAvailablePrinters.parentNode.replaceChild(alert,tblAvailablePrinters);
   }
}

// Gets the license for the selected printer if it is a client printer
function GetPrintLicense(printer, printLicenseControlId, messagesControlId, messagePanelControlId)
{
   try
   {
      var objPrinters = new ActiveXObject("BarTenderPrintClient.Printer");
      var printerInfo = printer.split(",");
      var sPrinterName = printerInfo[0];
      var sPrinterType = printerInfo[1];
      var printingLicense = "";
      if(sPrinterType=="client")
      {
         printingLicense = objPrinters.CreatePrintToFileLicenseScriptSafe(sPrinterName);
         var error = objPrinters.GetLastErrorMessage();
         if(error=="")
            document.getElementById(printLicenseControlId).value = printingLicense;
         else
         {
            OutputAlert("Unable to print. <br /><br />Reason: Failed to create print license.<br/>",messagesControlId,messagePanelControlId);
         }
      }
   }
   catch(exception)
   {
      OutputPrintClientError(messagesControlId,messagePanelControlId);
      return;
   }     
}
    
    
// Prints the client print code to the selected printer 
function ClientPrint(sSelectedPrinter, printCodeControlId, messagesControlId, messagePanelControlId)
{
    
   var prnObj;
   try 
   {
      prnObj = new ActiveXObject("BarTenderPrintClient.Printer");
         if(!prnObj)
            throw(0);
   }
   catch (e)
   {
      OutputPrintClientError(messagesControlId,messagePanelControlId);
      return;
   }
            
   var hiddenClientPrintCode = document.getElementById(printCodeControlId)
    
   if(hiddenClientPrintCode.value=="")
   {
      OutputAlert("Unable to print. Reason: No printer code was created.<br/>",messagesControlId,messagePanelControlId);
   }
   else
   {
      var returnVal = new Boolean(prnObj.SendPrintCode(sSelectedPrinter,"Web Label Print",hiddenClientPrintCode.value));
      if(returnVal==false)
      {
         var error = prnObj.GetLastErrorMessage();
         OutputAlert("Spooling failed while printing to " + sSelectedPrinter + "<br/><br/>Error Message: " + error + ".<br/>",messagesControlId,messagePanelControlId);
      }
      else
      {
         OutputAlert("The print job was spooled to " + sSelectedPrinter + " on the client computer.<br/>",messagesControlId,messagePanelControlId);
      }
   }
    
   var messagesPanel = document.getElementById(messagePanelControlId);
   messagesPanel.style.display = "";
}