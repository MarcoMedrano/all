namespace All
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    class Actions
    {
        internal void SetOsPath()
        {
            var paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);
            var currentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
            string parentDirectory = currentDirectory.Parent.FullName;

            var pathsList = paths.Split(';').ToList();
            var oldPath = pathsList.FirstOrDefault(path => path.StartsWith(parentDirectory, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(oldPath))
            {
                pathsList.Remove(oldPath);
            }

            pathsList.Add(currentDirectory.FullName);
            paths = string.Empty;
            pathsList.ForEach(p => paths += p + ";");

            Environment.SetEnvironmentVariable("PATH", paths, EnvironmentVariableTarget.User);
        }
    }
}
