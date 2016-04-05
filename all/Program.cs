﻿using System;
using System.Linq;

namespace All
{
    using System.Diagnostics;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            string command = string.Empty;
            args.ToList().ForEach(arg => command += arg + " ");

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
                    //TODO investigate tricks with echo o command line to make visible separations between execution on each folder
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
    }
}