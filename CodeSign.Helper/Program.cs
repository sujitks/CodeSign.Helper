using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSign.Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            HelperOptions options = new HelperOptions();
            if (args.Length > 0)
            {   
                var result = Parser.Default.ParseArguments(args, options, OnVerbCommand);
                if (!result)
                {
                    WriteUsagesAndThrowError(options);

                }

            }
            else
            {
                WriteUsagesAndThrowError(options);
            }
        }

        private static void WriteUsagesAndThrowError(HelperOptions options)
        {
            Console.Write(options.GetUsage());
            //throw new InvalidOperationException("Please see Usages of the tool and pass the appropriate parameters");   
        }

        private static void OnVerbCommand(string verbArgument, object verbOptions)
        {

            SignOption options = verbOptions as SignOption;
            if (options != null)//parsing successful
            {
                Console.Write(verbArgument);
                if (verbArgument.ToLower().Equals(SignOption.Verb))
                {
                    //logic to sign the app
                    SignAutomation automation = new SignAutomation();
                    automation.Sign(options);
                }
                else
                {
                    WriteUsagesAndThrowError(new HelperOptions());
                }

            }
            else {
                Console.WriteLine($"Verbargument {verbArgument} and options are {verbOptions}");
                WriteUsagesAndThrowError(new HelperOptions());
            }
        }
    }
}
