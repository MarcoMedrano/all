using System;

namespace All.Installation
{
    using Squirrel;

    class InstallerGate
    {
        private readonly Actions actions = new Actions();

        internal void OnInstallationAndUpdate()
        {
            // Note, in most of these scenarios, the app exits after this method completes!
            SquirrelAwareApp.HandleEvents(
            onInitialInstall: v => this.actions.SetOsPath(),
            onAppUpdate: v => this.actions.SetOsPath(),
            onAppUninstall: v => this.actions.RemoveOsPath(),
            onFirstRun: () => { });
        }

        internal void Update()
        {
            Console.WriteLine("Updating ...");
#if DEBUG
            using (var mgr = new UpdateManager(@"c:\lr.m\all\Releases\"))
            {
#else
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/MarcoMedrano/all").Result)
            {
#endif
                var res = mgr.UpdateApp().Result;

                if (res != null)
                {
                    Console.WriteLine("Updated to " + res.Version);
                    Console.WriteLine("Please open a new commandline to use new version.");
                }
                else
                    Console.WriteLine("No new version");

            }
        }
    }
}
