﻿using BepInEx.Logging;

namespace UKHitMarker
{
    public sealed class UKHitMarker : MonoSingleton<UKHitMarker>
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

            log.LogInfo("UKHitMarker initialized");
            this.initialized = true;

            return true;
        }
    }
}