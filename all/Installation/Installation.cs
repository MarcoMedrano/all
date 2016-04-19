using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All.Installation
{
    using Squirrel;

    class Installation
    {
        private readonly Actions actions = new Actions();
        internal void OnInstallationAndUpdate()
        {
            // Note, in most of these scenarios, the app exits after this method completes!
            SquirrelAwareApp.HandleEvents(
                onInitialInstall: v => this.actions.SetOsPath(),
                onAppUpdate: v => this.actions.SetOsPath(),
                onAppUninstall: v => { },
                onFirstRun: () => { });
        }

        internal void Update()
        {
            Console.WriteLine("Updating ...");
#if DEBUG
            using (var mgr = new UpdateManager(@"c:\lr.m\all\Releases\"))
            {
                Task<ReleaseEntry> updateApp = mgr.UpdateApp();

#else
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/MarcoMedrano/all"))
            {
                Task<ReleaseEntry> updateApp = mgr.Result.UpdateApp();
#endif
                var res = updateApp.Result;

                if (res != null)
                    Console.WriteLine("Updated to " + res.Version);
                else
                    Console.WriteLine("No new versions");
            }
        }
    }
}
