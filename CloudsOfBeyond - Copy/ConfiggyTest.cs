using BepInEx.Logging;
using Configgy;

namespace ConfiggyTest
{
    public sealed class ConfiggyTest : MonoSingleton<ConfiggyTest>
    {
        private static ManualLogSource log = new("");

        internal static void SetLogger(ManualLogSource logger) => log = logger;

        private bool initialized = false;

        public void Update()
        {
            if (!this.TryInit())
            {
                return;
            }
        }

        private bool TryInit()
        {
            if (this.initialized)
            {
                return true;
            }

            log.LogInfo("ConfiggyTest initialized");
            this.initialized = true;

            return true;
        }
    }
}