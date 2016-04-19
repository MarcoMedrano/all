namespace All.Installation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    class Actions
    {
        public Actions()
        {
            CurrentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
        }

        public static DirectoryInfo CurrentDirectory { get; private set; }

        internal void SetOsPath()
        {
            var pathsList = GetOsPathAsList();
            RemoveCurrentPath(pathsList);
            pathsList.Add(CurrentDirectory.FullName);

            SetOsPathFromPathList(pathsList);
        }

        public void RemoveOsPath()
        {
            var pathsList = GetOsPathAsList();
            RemoveCurrentPath(pathsList);
            
            SetOsPathFromPathList(pathsList);
        }

        private static void RemoveCurrentPath(List<string> pathsList)
        {
            string parentDirectory = CurrentDirectory.Parent.FullName;

            var oldPath =
                pathsList.FirstOrDefault(path => path.StartsWith(parentDirectory, StringComparison.InvariantCultureIgnoreCase));

            if (!string.IsNullOrEmpty(oldPath))
            {
                pathsList.Remove(oldPath);
            }
        }

        private static List<string> GetOsPathAsList()
        {
            var paths = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.User);

            return paths.Split(';').ToList();
        }

        private static void SetOsPathFromPathList(List<string> pathsList)
        {
            var paths = string.Empty;
            pathsList.ForEach(p => paths += p + ";");
            paths = paths.Remove(paths.Length - 1, 1);

            Environment.SetEnvironmentVariable("PATH", paths, EnvironmentVariableTarget.User);
        }
    }
}
