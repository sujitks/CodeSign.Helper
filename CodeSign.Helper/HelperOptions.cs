using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSign.Helper
{
    public class HelperOptions
    {


        [VerbOption(SignOption.Verb, HelpText = "Digitally sign an application")]
        public SignOption Sign { get; set; }


        public string GetUsage()
        {


            var help = HelpText.AutoBuild(this);
            if (this.Sign != null) help.AddOptions(this.Sign);
            
            return help.ToString();


        }
    }

    public class SignOption
    {

        public const string Verb = "sign";

        [Option("signtoolpath", Required = true, HelpText = "Location of signtool.exe on the machine")]
        public string SignToolPath { get; set; }

        [Option("timeserver", Required = true, HelpText = "URL of the timeserver i.e. http://rfc3161timestamp.globalsign.com/advanced")]
        public string TimeServerLocation { get; set; }

        [Option("password", Required = true, HelpText = "Password of certificate token")]
        public string CertificatePassword { get; set; }

        [Option("applicationfile", Required = true, HelpText = "Name and full path of application file")]
        public string ApplicationFile { get; set; }





    }


}
