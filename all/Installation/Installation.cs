using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace All.Installation
{
    using System.Runtime.InteropServices;

    using Squirrel;

    class Installation
    {
        private readonly Actions actions = new Actions();
        internal void OnInstallationAndUpdate()
        {
            //TODO is missing remove path when uninstall
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
#else
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/MarcoMedrano/all", prerelease: true).Result)
            {
#endif
                var res = mgr.UpdateApp().Result;

                if (res != null)
                {
                    Console.WriteLine("Updated to " + res.Version);
                    Console.WriteLine("Please open a new commandline to use new version.");
                }
                else
                    Console.WriteLine("No new versions");

            }
        }
    }
}
