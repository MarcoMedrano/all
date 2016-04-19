using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All
{
    using CommandLine;
    using CommandLine.Text;

    //Sad it does not support put all unbound commands in somewhere, 
    class CommandLineOptions
    {
        [Option('u', "update", Required = false, HelpText = "Updates 'all' to last version.")]
        public bool Update { get; set; }

        //[Option('v', "verbose", HelpText = "Prints all messages to standard output.")]
        //public bool Verbose { get; set; }
 
        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this);
        }
    }
}
