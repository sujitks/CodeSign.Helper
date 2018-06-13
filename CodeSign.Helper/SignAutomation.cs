using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace CodeSign.Helper
{
    /*
     * Start the signtool process by passing the args
     * search for he password dialog
     * Get the password text box and update the password and simulate enter
     * log the completion or error message
     */
    public class SignAutomation
    {

        public void Sign(SignOption options)
        {
            string signTool = Path.Combine(options.SignToolPath, "signtool.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo(signTool, $"sign /a /tr {options.TimeServerLocation} /td SHA256 {options.ApplicationFile}");

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;

            var process = Process.Start(startInfo);
           // process.WaitForExit();
            var element = GetTokenPasswordTextBox();

            if (SetPassword(element, options.CertificatePassword))
            {
                Console.WriteLine("File signed successfully");
            }

            
        }


        public AutomationElement GetTokenPasswordTextBox()
        {
            AutomationElement _certWindow = null;
            AutomationElement textBoxPassword = null;
            int ct = 0;
            do
            {
                _certWindow = AutomationElement.RootElement.FindFirst
    (TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty,
    "Token Logon"));

                ++ct;
                Thread.Sleep(100);
            }
            while (_certWindow == null && ct < 50);
            Console.WriteLine("Searching the token dialog!");

            if (_certWindow != null)
            {
                PropertyCondition documentControl = new PropertyCondition(
       AutomationElement.ControlTypeProperty,
       ControlType.Edit);
                var elements = _certWindow.FindAll(TreeScope.Descendants, documentControl);

                foreach (AutomationElement passwordText in elements)
                {
                    if (passwordText != null && passwordText.Current.IsEnabled)
                    {
                        //passwordText.SetFocus();
                        textBoxPassword = passwordText;
                        Console.WriteLine("Found the textbox");
                        break;

                    }
                }
            }
                return textBoxPassword;
         }


        public bool SetPassword(AutomationElement element, string password)
        {
            bool result = false;

            try
            {

                object valuePattern = null;


                if (!element.TryGetCurrentPattern(
            ValuePattern.Pattern, out valuePattern))
                {
                    
                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");     // Delete selection
                    SendKeys.SendWait(password);
                    SendKeys.SendWait("{ENTER}");
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                  

                    // Set focus for input functionality and begin.
                    element.SetFocus();
                    ((ValuePattern)valuePattern).SetValue(password);
                    SendKeys.SendWait("{ENTER}");
                }

                result = true;

            }
            catch (Exception)
            {
                result = false;
                throw;
            }

            return result;


        }
        
    }
}
