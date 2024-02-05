using BepInEx.Logging;

namespace UltraColor;

public sealed class UltraColor : MonoSingleton<UltraColor>
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

        log.LogInfo("UltraColor initialized");
        this.initialized = true;

        return true;
    }
}