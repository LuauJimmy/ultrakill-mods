using System.Reflection;
using System.Collections.Generic;

using BepInEx.Logging;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UltraColor;

public sealed class UltraColor : MonoSingleton<UltraColor> {
    private static ManualLogSource log = new("");
    internal static void SetLogger(ManualLogSource logger) => log = logger;

    private bool initialized = false;

    public void Update() {
        if (!this.TryInit()) {
            return;
        }
    }

    private bool TryInit() {
        if (this.initialized) {
            return true;
        }

        log.LogInfo("UltraColor initialized");
        this.initialized = true;

        return true;
    }
}
