using BepInEx.Logging;

namespace CloudsOfBeyond
{
    public sealed class CloudsOfBeyond : MonoSingleton<CloudsOfBeyond>
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

            log.LogInfo("CloudsOfBeyond initialized");
            this.initialized = true;

            return true;
        }
    }
}