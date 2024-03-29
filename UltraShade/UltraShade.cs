using BepInEx.Logging;

namespace UltraShade
{
    public sealed class UltraShade : MonoSingleton<UltraShade>
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

            log.LogInfo("UltraShade initialized");
            this.initialized = true;

            return true;
        }
    }
}