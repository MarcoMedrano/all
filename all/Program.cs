using System;
using System.Linq;

namespace All
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Squirrel;

    class Program
    {
        static void Main(string[] args)
        {
            HandleInstallation();
            string command = string.Empty;
            args.ToList().ForEach(arg => command += arg + " ");
            command = command.ToLowerInvariant().Trim();

            HandleOwnCommands(command);

            var baseDir = new DirectoryInfo(Environment.CurrentDirectory);

            var p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.Start();

            int countDirectoriesSuccess = 0;
            int countDirectoriesFail = 0;

            foreach (var currentDir in baseDir.GetDirectories())
            {
                try
                {
                    //TODO investigate tricks with echo o cmd to make visible separations between execution on each folder
                    //p.StandardInput.WriteLine(@"echo \n----------------------");
                    p.StandardInput.WriteLine(@"cd " + currentDir);
                    p.StandardInput.WriteLine(command);
                    p.StandardInput.WriteLine(@"cd ..");

                    //Might be another option one process per command.
                    //Process p = new Process();
                    //p.StartInfo.FileName = "cmd.exe";
                    //p.StartInfo.Arguments = "/c " + command;
                    //p.StartInfo.WorkingDirectory = currentDir.FullName;
                    //p.StartInfo.UseShellExecute = false;
                    //p.Start();
                    countDirectoriesSuccess++;
                }
                catch (Exception e)
                {
                    countDirectoriesFail++;
                    Console.WriteLine(e);
                }
            }

            p.Close();

            //What the hell it does not change the color and why prints at the begining instead of the end?
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Executed on {countDirectoriesSuccess} directoies successuful and failed on {countDirectoriesFail} directories.");
            Console.ForegroundColor = oldColor;
        }

        private static void HandleInstallation()
        {
            using (var mgr = new UpdateManager(@"c:\lr.m\all\Releases\"))
            {
                // Note, in most of these scenarios, the app exits after this method
                // completes!
                SquirrelAwareApp.HandleEvents(
                  onInitialInstall: v => SetOsPath(),
                  onAppUpdate: v => SetOsPath(),
                  onAppUninstall: v => { },
                  onFirstRun: () => { });
            }
        }

        private static void SetOsPath()
        {
            //TODO make proper code which does not replace the whole PATH lol :S
            //Environment.SetEnvironmentVariable("PATH", Assembly.GetExecutingAssembly().Location, EnvironmentVariableTarget.User);
        }

        private static void HandleOwnCommands(string command)
        {
            CommandLineOptions options = new CommandLineOptions();

            if (Regex.IsMatch(command, "^(-|--|/)(u$|update$)"))
            {
                Console.WriteLine("Updating ...");
                using (var mgr = new UpdateManager(@"c:\lr.m\all\Releases\"))
                {
                    Task<ReleaseEntry> updateApp = mgr.UpdateApp();
                    var res = updateApp.Result;
                    if (res != null)
                        Console.WriteLine("Updated to " + res.Version);
                    else
                        Console.WriteLine("No new versions");
                }

                Environment.Exit(0);
            }

            if (Regex.IsMatch(command, @"^(-|--|/)(h$|help$|\?)") || string.IsNullOrWhiteSpace(command))
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(0);
            }
        }
    }
}
