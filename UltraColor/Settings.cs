using BepInEx.Configuration;
using EffectChanger.Enum;
using PluginConfig.API;
using PluginConfig.API.Decorators;
using PluginConfig.API.Fields;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UltraColor;

#nullable disable

public static class Settings
{
    private static PluginConfigurator? config;

    public static BoolField shotgunEnabled;
    public static ColorField shotgunProjectileStartColor;
    public static ColorField shotgunProjectileEndColor;
    public static ColorField shotgunMuzzleFlashColor;
    public static ColorField shotgunMuzzleFlashPointLightColor;
    public static EnumField<ColorHelper.BulletColor> shotgunBulletColor;
    public static EnumField<ColorHelper.MuzzleFlash> shotgunGrenadeSpriteColor;

    public static BoolField piercerRevolverEnabled;
    public static ColorField piercerRevolverChargeBeamStartColor;
    public static ColorField piercerRevolverChargeBeamEndColor;
    public static ColorField piercerRevolverBeamStartColor;
    public static ColorField piercerRevolverBeamEndColor;
    public static ColorField piercerRevolverMuzzleFlashColor;
    public static ColorField piercerRevolverChargeMuzzleFlashColor;
    public static EnumField<ColorHelper.BulletColor> piercerRevolverChargeEffectColor;

    public static BoolField sharpShooterEnabled;
    public static ColorField sharpShooterChargeBeamStartColor;
    public static ColorField sharpShooterChargeBeamEndColor;
    public static ColorField sharpShooterBeamStartColor;
    public static ColorField sharpShooterBeamEndColor;
    public static ColorField sharpShooterMuzzleFlashColor;

    public static BoolField coinEnabled;
    public static BoolField marksmanEnabled;
    public static ColorField marksmanBeamStartColor;
    public static ColorField marksmanBeamEndColor;
    public static ColorField revolverCoinTrailStartColor;
    public static ColorField revolverCoinTrailEndColor;
    public static ColorField marksmanMuzzleFlashColor;

    public static BoolField altMarksmanEnabled;
    public static ColorField altMarksmanBeamStartColor;
    public static ColorField altMarksmanBeamEndColor;
    public static ColorField altMarksmanMuzzleFlashColor;

    public static BoolField altSharpShooterEnabled;
    public static ColorField altSharpShooterChargeBeamStartColor;
    public static ColorField altSharpShooterChargeBeamEndColor;
    public static ColorField altSharpShooterBeamStartColor;
    public static ColorField altSharpShooterBeamEndColor;
    public static ColorField altSharpShooterMuzzleFlashColor;

    public static BoolField altPiercerRevolverEnabled;
    public static ColorField altPiercerRevolverChargeBeamStartColor;
    public static ColorField altPiercerRevolverChargeBeamEndColor;
    public static ColorField altPiercerRevolverBeamStartColor;
    public static ColorField altPiercerRevolverBeamEndColor;
    public static ColorField altPiercerRevolverMuzzleFlashColor;
    public static ColorField altPiercerRevolverChargeMuzzleFlashColor;
    public static EnumField<ColorHelper.BulletColor> altPiercerRevolverChargeEffectColor;

    public static BoolField magnetNailgunEnabled;
    public static ColorField magnetNailgunTrailStartColor;
    public static ColorField magnetNailgunTrailEndColor;
    public static ColorField magnetNailgunMuzzleFlashColor;

    public static BoolField overheatNailgunEnabled;
    public static ColorField overheatNailgunTrailStartColor;
    public static ColorField overheatNailgunTrailEndColor;
    public static ColorField overheatNailgunHeatedNailTrailStartColor;
    public static ColorField overheatNailgunHeatedNailTrailEndColor;
    public static ColorField overheatNailgunMuzzleFlashColor;

    public static BoolField altMagnetNailgunEnabled;
    public static ColorField altMagnetNailgunTrailStartColor;
    public static ColorField altMagnetNailgunTrailEndColor;
    public static ColorField altMagnetNailgunMuzzleFlashColor;

    public static BoolField altOverheatNailgunEnabled;
    public static ColorField altOverheatNailgunTrailStartColor;
    public static ColorField altOverheatNailgunTrailEndColor;
    public static ColorField altOverheatNailgunHeatedNailTrailStartColor;
    public static ColorField altOverheatNailgunHeatedNailTrailEndColor;
    public static ColorField altOverheatNailgunMuzzleFlashColor;

    public static BoolField smallExplosionEnabled;
    public static ColorField smallExplosionColor;
    public static BoolField maliciousExplosionEnabled;
    public static ColorField maliciousExplosionColor;
    public static BoolField nukeExplosionEnabled;
    public static ColorField nukeExplosionColor;

    public static BoolField blueRailcannonEnabled;
    public static ColorField blueRailcannonStartColor;
    public static ColorField blueRailcannonEndColor;
    public static ColorField blueRailcannonMuzzleFlashColor;

    public static BoolField redRailcannonEnabled;
    public static ColorField redRailcannonStartColor;
    public static ColorField redRailcannonEndColor;
    public static ColorField redRailcannonMuzzleFlashColor;

    public static BoolField greenRailcannonEnabled;
    public static ColorField greenRailcannonTrailStartColor;
    public static ColorField greenRailcannonTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> greenRailcannonMuzzleFlashColor;

    public static BoolField revolverCoinRicochetBeamEnabled;
    public static ColorField revolverCoinRicochetBeamStartColor;
    public static ColorField revolverCoinRicochetBeamEndColor;

    public static BoolField freezeRocketLauncherEnabled;
    public static ColorField freezeRocketLauncherTrailStartColor;
    public static ColorField freezeRocketLauncherTrailEndColor;

    public static BoolField cannonballRocketLauncherEnabled;
    public static ColorField cannonballRocketLauncherTrailStartColor;
    public static ColorField cannonballRocketLauncherTrailEndColor;

    public static void Init(ConfigFile cfg)
    {
        config = PluginConfigurator.Create("UltraColor", "luaujimmy.UltraColor");
        string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string iconPath = Path.Combine(pluginPath, "Assets\\icon.png");
        config.SetIconWithURL("file://" + iconPath);

        ConfigHeader headerTitle = new ConfigHeader(config.rootPanel, "UltraColor", 30);
        ConfigHeader headerInfo = new ConfigHeader(config.rootPanel, "--Restart Level for Changes to Take Effect--", 20);

        ConfigPanel revolverSubPanel = new ConfigPanel(config.rootPanel, "Revolver / Coin", "RevolverSubPanel");
        ConfigPanel coinSubPanel = new ConfigPanel(revolverSubPanel, "Coin", "CoinSubPanel");
        ConfigPanel piercerRevolverSubPanel = new ConfigPanel(revolverSubPanel, "Piercer", "PiercerRevolverSubPanel");
        ConfigPanel altPiercerRevolverSubPanel = new ConfigPanel(revolverSubPanel, "Alt Piercer", "AltPiercerRevolverSubPanel");
        ConfigPanel marksmanSubPanel = new ConfigPanel(revolverSubPanel, "Marksman", "MarksmanRevolverSubPanel");
        ConfigPanel altMarksmanSubPanel = new ConfigPanel(revolverSubPanel, "Alt Marksman", "AltMarksmanRevolverSubPanel");
        ConfigPanel sharpShooterSubPanel = new ConfigPanel(revolverSubPanel, "Sharpshooter", "SharpShooterSubPanel");
        ConfigPanel altSharpShooterSubPanel = new ConfigPanel(revolverSubPanel, "Alt Sharpshooter", "AltSharpShooterSubPanel");

        ConfigPanel shotgunSubPanel = new ConfigPanel(config.rootPanel, "Shotgun", "ShotgunSubPanel");

        ConfigPanel nailgunSubPanel = new ConfigPanel(config.rootPanel, "Nailgun", "NailgunSubPanel");
        ConfigPanel magnetNailgunSubPanel = new ConfigPanel(nailgunSubPanel, "Magnet Nailgun", "MagnetNailgunSubPanel");
        ConfigPanel altMagnetNailgunSubPanel = new ConfigPanel(nailgunSubPanel, "Alt Magnet Nailgun", "NailgunSubPanel");
        ConfigPanel overheatNailgunSubPanel = new ConfigPanel(nailgunSubPanel, "Overheat Nailgun", "Overheat Nailgun");
        ConfigPanel altOverheatNailgunSubPanel = new ConfigPanel(nailgunSubPanel, "Alt Overheat Nailgun", "");

        ConfigPanel railcannonSubPanel = new ConfigPanel(config.rootPanel, "Railcannon", "RailcannonSubPanel");
        ConfigPanel blueRailcannonSubPanel = new ConfigPanel(railcannonSubPanel, "Blue Railcannon", "BlueRailcannonSubPanel");
        ConfigPanel redRailcannonSubPanel = new ConfigPanel(railcannonSubPanel, "Red Railcannon", "RedRailcannonSubPanel");
        ConfigPanel greenRailcannonSubPanel = new ConfigPanel(railcannonSubPanel, "Green Railcannon", "GreenRailcannonSubPanel");

        ConfigPanel rocketSubPanel = new ConfigPanel(config.rootPanel, "Rocket Launcher", "RocketLauncherSubPanel");
        ConfigPanel freezeRocketSubPanel = new ConfigPanel(rocketSubPanel, "FreezeFrame Rocket Launcher", "FreezeRocketSubPanel");
        ConfigPanel cannonballRocketSubPanel = new ConfigPanel(rocketSubPanel, "S.R.S Cannon Rocket Launcher", "CannonballRocketSubPanel");

        ConfigPanel explosionSubPanel = new ConfigPanel(config.rootPanel, "Explosion", "ExplosionSubPanel");

        // Explosions
        smallExplosionEnabled = new BoolField(explosionSubPanel, "Enabled", "SmallExplosionEnabled", false);
        // Core, friendly fire, projectile boost, probably interruption
        smallExplosionColor = new ColorField(explosionSubPanel, "Small Explosion Color", "SmallExplosionColor", new Color(1, 0.5f, 0, 0.5f));

        // Red railcannon, airshot rockets,
        
        maliciousExplosionEnabled = new BoolField(explosionSubPanel, "Enabled", "MaliciousExplosionEnabled", false);
        maliciousExplosionColor = new ColorField(explosionSubPanel, "Malicious Explosion Color", "MaliciousExplosionColor", new Color(1, 0.5f, 0, 0.5f));

        // Core Nuke

        nukeExplosionEnabled = new BoolField(explosionSubPanel, "Enabled", "NukeExplosionEnabled", false);
        nukeExplosionColor = new ColorField(explosionSubPanel, "Nuke Explosion Color", "NukeExplosionColor", new Color(1, 0.5f, 0, 0.5f));

        // Shotgun
        shotgunEnabled = new BoolField(shotgunSubPanel, "Enabled", "shotgunEnabled", false);
        shotgunMuzzleFlashColor = new ColorField(shotgunSubPanel, "Muzzle Flash Sprite Color", "ShotgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        shotgunMuzzleFlashPointLightColor = new ColorField(shotgunSubPanel, "Muzzle Flash Environment Light Color", "MuzzleFlashPointLightColor", new Color(1, 0.7725f, 0, 1));
        shotgunProjectileStartColor = new ColorField(shotgunSubPanel, "Projectile Trail Start Color", "ShotgunProjectileStartColor", new Color(1, 0.7725F, 0, 1));
        shotgunProjectileEndColor = new ColorField(shotgunSubPanel, "Projectile Trail End Color", "ShotgunProjectileEndColor", new Color(1, 0.7725F, 0, 0));
        shotgunBulletColor = new EnumField<ColorHelper.BulletColor>(shotgunSubPanel, "Bullet Color", "ShotgunProjectileMeshColor", ColorHelper.BulletColor.Default);
        shotgunGrenadeSpriteColor = new EnumField<ColorHelper.MuzzleFlash>(shotgunSubPanel, "Grenade Sprite Color", "ShotgunGrenadeSpriteColor", ColorHelper.MuzzleFlash.Default);

        // Piercer
        piercerRevolverEnabled = new BoolField(piercerRevolverSubPanel, "Enabled", "piercerEnabled", false);
        piercerRevolverMuzzleFlashColor = new ColorField(piercerRevolverSubPanel, "Muzzle Flash Sprite Color", "PiercerRevolverMuzzleFlashColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeMuzzleFlashColor = new ColorField(piercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "PiercerRevolverChargeMuzzleFlashColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(piercerRevolverSubPanel, "Charge Effect Color", "PiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);
        piercerRevolverBeamStartColor = new ColorField(piercerRevolverSubPanel, "Normal Beam Start Color", "PiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverBeamEndColor = new ColorField(piercerRevolverSubPanel, "Normal Beam End Color", "PiercerRevolverBeamEndColor", new Color(1, 0.8078F, 0, 1));
        piercerRevolverChargeBeamStartColor = new ColorField(piercerRevolverSubPanel, "Charge Beam Start Color", "PiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeBeamEndColor = new ColorField(piercerRevolverSubPanel, "Charge Beam End Color", "PiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));

        // Alt Piercer
        altPiercerRevolverEnabled = new BoolField(altPiercerRevolverSubPanel, "Enabled", "altPiercerEnabled", false);
        altPiercerRevolverMuzzleFlashColor = new ColorField(altPiercerRevolverSubPanel, "Muzzle Flash Sprite Color", "AltPiercerRevolverMuzzleFlashColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeMuzzleFlashColor = new ColorField(altPiercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "AltPiercerRevolverChargeMuzzleFlashColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(altPiercerRevolverSubPanel, "Charge Effect Color", "AltPiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);
        altPiercerRevolverBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Normal Beam Start Color", "AltPiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Normal Beam End Color", "AltPiercerRevolverBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altPiercerRevolverChargeBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Charge Beam Start Color", "AltPiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Charge Beam End Color", "AltPiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));

        // Marksman
        marksmanEnabled = new BoolField(marksmanSubPanel, "Enabled", "marksmanEnabled", false);

        //Coin
        coinEnabled = new BoolField(coinSubPanel, "Enabled", "coinEnabled", false);
        // Coin Trail
        revolverCoinTrailStartColor = new ColorField(coinSubPanel, "Coin Trail Start Color", "CoinTrailStartColor", new Color(1, 1, 1, 0));
        revolverCoinTrailEndColor = new ColorField(coinSubPanel, "Coin Trail End Color", "CoinTrailEndColor", new Color(0, 0, 0, 0));

        marksmanMuzzleFlashColor = new ColorField(marksmanSubPanel, "Marksman Muzzle Flash Color", "MarksmanMuzzleFlashColor", new Color(1, 1, 1, 1));
        marksmanBeamStartColor = new ColorField(marksmanSubPanel, "Normal Beam Start Color", "MarksmanBeamStartColor", new Color(1, 1, 1, 1));
        marksmanBeamEndColor = new ColorField(marksmanSubPanel, "Normal Beam End Color", "MarksmanBeamEndColor", new Color(1, 0.8078F, 0, 1));
        // Coin Ricochet beam after being shot by normal Marksman
        revolverCoinRicochetBeamStartColor = new ColorField(marksmanSubPanel, "Ricochet Beam Start Color", "RevolverCoinRicochetBeamStartColor", new Color(1, 0.8078F, 0, 1));
        revolverCoinRicochetBeamEndColor = new ColorField(marksmanSubPanel, "Ricochet Beam End Color", "RevolverCoinRicochetBeamEndColor", new Color(1, 0.8078F, 0, 1));

        // Alt Marksman
        altMarksmanEnabled = new BoolField(altMarksmanSubPanel, "Enabled", "altMarksmanEnabled", false);
        altMarksmanMuzzleFlashColor = new ColorField(altMarksmanSubPanel, "Muzzle Flash Sprite Color", "AltMarksmanMuzzleFlashColor", new Color(1, 1, 1, 1));
        altMarksmanBeamStartColor = new ColorField(altMarksmanSubPanel, "Beam Start Color", "AltMarksmanBeamStartColor", new Color(1, 1, 1, 1));
        altMarksmanBeamEndColor = new ColorField(altMarksmanSubPanel, "Beam End Color", "AltMarksmanBeamEndColor", new Color(1, 0.7255F, 0, 1));

        // Sharpshooter
        sharpShooterEnabled = new BoolField(sharpShooterSubPanel, "Enabled", "sharpShooterEnabled", false);
        sharpShooterMuzzleFlashColor = new ColorField(sharpShooterSubPanel, "Muzzle Flash Sprite Color", "SharpShooterMuzzleFlashColor", new Color(1, 1, 1, 1));
        sharpShooterChargeBeamStartColor = new ColorField(sharpShooterSubPanel, "Charge Beam Start Color", "SharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterChargeBeamEndColor = new ColorField(sharpShooterSubPanel, "Charge Beam End Color", "SharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));
        sharpShooterBeamStartColor = new ColorField(sharpShooterSubPanel, "Beam Start Color", "SharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterBeamEndColor = new ColorField(sharpShooterSubPanel, "Beam End Color", "SharpShooterBeamEndColor", new Color(1, 0.8078F, 0, 1));

        //ALt Sharpshooter
        altSharpShooterEnabled = new BoolField(altSharpShooterSubPanel, "Enabled", "altSharpshooterEnabled", false);
        altSharpShooterMuzzleFlashColor = new ColorField(altSharpShooterSubPanel, "Muzzle Flash Color", "AltSharpShooterMuzzleFlashColor", new Color(1, 1, 1, 1));
        altSharpShooterBeamStartColor = new ColorField(altSharpShooterSubPanel, "Beam Start Color", "AltSharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterBeamEndColor = new ColorField(altSharpShooterSubPanel, "Beam End Color", "AltSharpShooterBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altSharpShooterChargeBeamStartColor = new ColorField(altSharpShooterSubPanel, "Charge Beam Start Color", "AltSharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterChargeBeamEndColor = new ColorField(altSharpShooterSubPanel, "Charge Beam End Color", "AltSharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));

        // Magnet nail
        magnetNailgunEnabled = new BoolField(magnetNailgunSubPanel, "Enabled", "magnetNailgunEnabled", false);
        magnetNailgunMuzzleFlashColor = new ColorField(magnetNailgunSubPanel, "Muzzle Flash Color", "MagnetNailgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        magnetNailgunTrailStartColor = new ColorField(magnetNailgunSubPanel, "Nail Trail Start Color", "MagnetNailgunNailTrailStartColor", new Color(0.251F, 0.9059F, 1, 0.4902F));
        magnetNailgunTrailEndColor = new ColorField(magnetNailgunSubPanel, "Nail Trail End Color", "MagnetNailgunNailTrailEndColor", new Color(0.251F, 0.9059F, 1, 0));

        altMagnetNailgunEnabled = new BoolField(altMagnetNailgunSubPanel, "Enabled", "altMagnetNailgunEnabled", false);
        altMagnetNailgunMuzzleFlashColor = new ColorField(altMagnetNailgunSubPanel, "Muzzle Flash Color", "AltMagnetNailgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        altMagnetNailgunTrailStartColor = new ColorField(altMagnetNailgunSubPanel, "Nail Trail Start Color", "AltMagnetNailgunNailTrailStartColor", new Color(0, 0.8745F, 1, 0.4902F));
        altMagnetNailgunTrailEndColor = new ColorField(altMagnetNailgunSubPanel, "Nail Trail End Color", "AltMagnetNailgunNailTrailEndColor", new Color(0, 0, 0, 0));

        overheatNailgunEnabled = new BoolField(overheatNailgunSubPanel, "Enabled", "overheatNailgunEnabled", false);
        overheatNailgunMuzzleFlashColor = new ColorField(overheatNailgunSubPanel, "Muzzle Flash Color", "OverheatNailgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        overheatNailgunTrailStartColor = new ColorField(overheatNailgunSubPanel, "Nail Trail Start Color", "OverheatNailgunNailTrailStartColor", new Color(1, 1, 1, 0.4902F));
        overheatNailgunTrailEndColor = new ColorField(overheatNailgunSubPanel, "Nail Trail End Color", "OverheatNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        overheatNailgunHeatedNailTrailStartColor = new ColorField(overheatNailgunSubPanel, "Heated Nail Trail Start Color", "OverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        overheatNailgunHeatedNailTrailEndColor = new ColorField(overheatNailgunSubPanel, "Heated Nail Trail End Color", "OverHeatNailgunHeatedNailTrailEndColor", new Color(0, 0, 0, 0));

        altOverheatNailgunEnabled = new BoolField(altOverheatNailgunSubPanel, "Enabled", "altOverheatNailgunEnabled", false);
        altOverheatNailgunMuzzleFlashColor = new ColorField(altOverheatNailgunSubPanel, "Muzzle Flash Sprite Color", "AltOverheatNailgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        altOverheatNailgunTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail Start Color", "AltOverheatNailgunNailTrailStartColor", new Color(0.502F, 0.502F, 0.502F, 0.4902F));
        altOverheatNailgunTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail End Color", "AltOverheatNailgunNailTrailEndColor", new Color(0, 0, 0, 0));
        altOverheatNailgunHeatedNailTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail Start Color", "AltOverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        altOverheatNailgunHeatedNailTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail End Color", "AltOverHeatNailgunHeatedNailTrailEndColor", new Color(0, 0, 0, 0));

        blueRailcannonEnabled = new BoolField(blueRailcannonSubPanel, "Enabled", "blueRailcannonEnabled", false);
        blueRailcannonMuzzleFlashColor = new ColorField(blueRailcannonSubPanel, "Muzzle Flash Color", "BlueRailcannonMuzzleFlashColor", new Color(0f, 0.8f, 0.1f));
        blueRailcannonStartColor = new ColorField(blueRailcannonSubPanel, "Beam Start Color", "BlueRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        blueRailcannonEndColor = new ColorField(blueRailcannonSubPanel, "Beam End Color", "BlueRailcannonBeamEndColor", new Color(0, 0.8353F, 1, 1));

        redRailcannonEnabled = new BoolField(redRailcannonSubPanel, "Enabled", "redRailcannonEnabled", false);
        redRailcannonMuzzleFlashColor = new ColorField(redRailcannonSubPanel, "Muzzle Flash Color", "RedRailcannonMuzzleFlashColor", new Color(1, 0.6314F, 0, 1));
        redRailcannonStartColor = new ColorField(redRailcannonSubPanel, "Beam Start Color", "RedRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        redRailcannonEndColor = new ColorField(redRailcannonSubPanel, "Beam End Color", "RedRailcannonBeamEndColor", new Color(1, 0.6314F, 0, 1));
        //redRailcannonGlowStartColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam Glow Start Color", "RedRailcannonBeamGlowStartColor", )

        greenRailcannonEnabled = new BoolField(greenRailcannonSubPanel, "Enabled", "greenRailcannonEnabled", false);
        greenRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(greenRailcannonSubPanel, "Muzzle Flash Color", "GreenRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        greenRailcannonTrailStartColor = new ColorField(greenRailcannonSubPanel, "Trail Start Color", "GreenRailcannonTrailStartColor", new Color(0.2667F, 1, 0.2706F, 1));
        greenRailcannonTrailEndColor = new ColorField(greenRailcannonSubPanel, "Trail End Color", "GreenRailcannonTrailEndColor", new Color(0, 0, 0, 0));

        freezeRocketLauncherEnabled = new BoolField(freezeRocketSubPanel, "Enabled", "freezeRocketLauncherEnabled", false);
        freezeRocketLauncherTrailStartColor = new ColorField(freezeRocketSubPanel, "Rocket Trail Start Color", "FreezeRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        freezeRocketLauncherTrailEndColor = new ColorField(freezeRocketSubPanel, "Rocket Trail End Color", "FreezeRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));

        cannonballRocketLauncherEnabled = new BoolField(cannonballRocketSubPanel, "Enabled", "cannonballRocketLauncherEnabled", false);
        cannonballRocketLauncherTrailStartColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail Start Color", "CannonballRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        cannonballRocketLauncherTrailEndColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail End Color", "CannonballRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));
    }
}