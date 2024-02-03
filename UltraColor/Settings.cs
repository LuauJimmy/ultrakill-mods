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

public static class Settings {

    private static PluginConfigurator? config;

    public static BoolField shotgunEnabled;
    public static ColorField shotgunProjectileStartColor;
    public static ColorField shotgunProjectileEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> shotgunMuzzleFlashColor;
    public static ColorField shotgunMuzzleFlashPointLightColor;
    public static EnumField<ColorHelper.BulletColor> shotgunBulletColor;
    public static EnumField<ColorHelper.MuzzleFlash> shotgunGrenadeSpriteColor;

    public static BoolField piercerRevolverEnabled;
    public static ColorField piercerRevolverChargeBeamStartColor;
    public static ColorField piercerRevolverChargeBeamEndColor;
    public static ColorField piercerRevolverBeamStartColor;
    public static ColorField piercerRevolverBeamEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> piercerRevolverMuzzleFlashColor;
    public static EnumField<ColorHelper.MuzzleFlash> piercerRevolverChargeMuzzleFlashColor;
    public static EnumField<ColorHelper.BulletColor> piercerRevolverChargeEffectColor;

    public static BoolField sharpShooterEnabled;
    public static ColorField sharpShooterChargeBeamStartColor;
    public static ColorField sharpShooterChargeBeamEndColor;
    public static ColorField sharpShooterBeamStartColor;
    public static ColorField sharpShooterBeamEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> sharpShooterMuzzleFlashColor;

    public static BoolField coinEnabled;
    public static BoolField marksmanEnabled;
    public static ColorField marksmanBeamStartColor;
    public static ColorField marksmanBeamEndColor;
    public static ColorField revolverCoinTrailStartColor;
    public static ColorField revolverCoinTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> marksmanMuzzleFlashColor;

    public static BoolField altMarksmanEnabled;
    public static ColorField altMarksmanBeamStartColor;
    public static ColorField altMarksmanBeamEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> altMarksmanMuzzleFlashColor;

    public static BoolField altSharpShooterEnabled;
    public static ColorField altSharpShooterChargeBeamStartColor;
    public static ColorField altSharpShooterChargeBeamEndColor;
    public static ColorField altSharpShooterBeamStartColor;
    public static ColorField altSharpShooterBeamEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> altSharpShooterMuzzleFlashColor;

    public static BoolField altPiercerRevolverEnabled;
    public static ColorField altPiercerRevolverChargeBeamStartColor;
    public static ColorField altPiercerRevolverChargeBeamEndColor;
    public static ColorField altPiercerRevolverBeamStartColor;
    public static ColorField altPiercerRevolverBeamEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> altPiercerRevolverMuzzleFlashColor;
    public static EnumField<ColorHelper.MuzzleFlash> altPiercerRevolverChargeMuzzleFlashColor;
    public static EnumField<ColorHelper.BulletColor> altPiercerRevolverChargeEffectColor;

    public static BoolField magnetNailgunEnabled;
    public static ColorField magnetNailgunTrailStartColor;
    public static ColorField magnetNailgunTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> magnetNailgunMuzzleFlashColor;

    public static BoolField overheatNailgunEnabled;
    public static ColorField overheatNailgunTrailStartColor;
    public static ColorField overheatNailgunTrailEndColor;
    public static ColorField overheatNailgunHeatedNailTrailStartColor;
    public static ColorField overheatNailgunHeatedNailTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> overheatNailgunMuzzleFlashColor;

    public static BoolField altMagnetNailgunEnabled;
    public static ColorField altMagnetNailgunTrailStartColor;
    public static ColorField altMagnetNailgunTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> altMagnetNailgunMuzzleFlashColor;

    public static BoolField altOverheatNailgunEnabled;
    public static ColorField altOverheatNailgunTrailStartColor;
    public static ColorField altOverheatNailgunTrailEndColor;
    public static ColorField altOverheatNailgunHeatedNailTrailStartColor;
    public static ColorField altOverheatNailgunHeatedNailTrailEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> altOverheatNailgunMuzzleFlashColor;

    public static BoolField explosionsEnabled;
    public static EnumField<ColorHelper.ExplosionColor> smallExplosionColor;
    public static EnumField<ColorHelper.ExplosionColor> maliciousExplosionColor;
    public static EnumField<ColorHelper.ExplosionColor> nukeExplosionColor;

    public static BoolField blueRailcannonEnabled;
    public static ColorField blueRailcannonStartColor;
    public static ColorField blueRailcannonEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> blueRailcannonMuzzleFlashColor;

    public static BoolField redRailcannonEnabled;
    public static ColorField redRailcannonStartColor;
    public static ColorField redRailcannonEndColor;
    public static EnumField<ColorHelper.MuzzleFlash> redRailcannonMuzzleFlashColor;

    public static BoolField greenRailcannonEnabled;
    public static ColorField greenRailcannonTrailColor;
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

    public static void Init(ConfigFile cfg) {
        config = PluginConfigurator.Create("UltraColor", "luaujimmy.UltraColor");
        string pluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string iconPath = Path.Combine(pluginPath, "icon.png");
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

        // Core, friendly fire, projectile boost, probably interruption
        smallExplosionColor = new EnumField<ColorHelper.ExplosionColor>(explosionSubPanel, "Small Explosion Color", "SmallExplosionColor", ColorHelper.ExplosionColor.Default);
        
        // Red railcannon, airshot rockets, 
        maliciousExplosionColor = new EnumField<ColorHelper.ExplosionColor>(explosionSubPanel, "Malicious Explosion Color", "MaliciousExplosionColor", ColorHelper.ExplosionColor.Default);
        nukeExplosionColor = new EnumField<ColorHelper.ExplosionColor>(explosionSubPanel, "Nuke Explosion Color", "NukeExplosionColor", ColorHelper.ExplosionColor.Default);

        // Shotgun
        shotgunEnabled = new BoolField(shotgunSubPanel, "Enabled", "shotgunEnabled", false);
        shotgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(shotgunSubPanel, "Muzzle Flash Sprite Color", "ShotgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        shotgunMuzzleFlashPointLightColor = new ColorField(shotgunSubPanel, "Muzzle Flash Environment Light Color", "MuzzleFlashPointLightColor", new Color(1, 0.7725f, 0, 1));
        shotgunProjectileStartColor = new ColorField(shotgunSubPanel, "Projectile Trail Start Color", "ShotgunProjectileStartColor", new Color(1, 0.7725F, 0, 1));
        shotgunProjectileEndColor = new ColorField(shotgunSubPanel, "Projectile Trail End Color", "ShotgunProjectileEndColor", new Color(1, 0.7725F, 0, 0));
        shotgunBulletColor = new EnumField<ColorHelper.BulletColor>(shotgunSubPanel, "Bullet Color", "ShotgunProjectileMeshColor", ColorHelper.BulletColor.Default);
        shotgunGrenadeSpriteColor = new EnumField<ColorHelper.MuzzleFlash>(shotgunSubPanel, "Grenade Sprite Color", "ShotgunGrenadeSpriteColor", ColorHelper.MuzzleFlash.Default);

        // Piercer 
        piercerRevolverEnabled = new BoolField(piercerRevolverSubPanel, "Enabled", "piercerEnabled", false);
        piercerRevolverMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(piercerRevolverSubPanel, "Muzzle Flash Sprite Color", "PiercerRevolverMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        piercerRevolverChargeMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(piercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "PiercerRevolverChargeMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        piercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(piercerRevolverSubPanel, "Charge Effect Color", "PiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);
        piercerRevolverBeamStartColor = new ColorField(piercerRevolverSubPanel, "Normal Beam Start Color", "PiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverBeamEndColor = new ColorField(piercerRevolverSubPanel, "Normal Beam End Color", "PiercerRevolverBeamEndColor", new Color(1, 0.8078F, 0, 1));
        piercerRevolverChargeBeamStartColor = new ColorField(piercerRevolverSubPanel, "Charge Beam Start Color", "PiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeBeamEndColor = new ColorField(piercerRevolverSubPanel, "Charge Beam End Color", "PiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));

        // Alt Piercer
        altPiercerRevolverEnabled = new BoolField(altPiercerRevolverSubPanel, "Enabled", "altPiercerEnabled", false);
        altPiercerRevolverMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altPiercerRevolverSubPanel, "Muzzle Flash Sprite Color", "AltPiercerRevolverMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altPiercerRevolverChargeMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altPiercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "AltPiercerRevolverChargeMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altPiercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(altPiercerRevolverSubPanel, "Charge Effect Color", "AltPiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);
        altPiercerRevolverBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Normal Beam Start Color", "AltPiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Normal Beam End Color", "AltPiercerRevolverBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altPiercerRevolverChargeBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Charge Beam Start Color", "AltPiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Charge Beam End Color", "AltPiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));

        // Marksman
        marksmanEnabled = new BoolField(marksmanSubPanel, "Enabled", "marksmanEnabled", false);
        

        //Coin
        coinEnabled  = new BoolField(coinSubPanel, "Enabled", "coinEnabled", false);
        // Coin Trail
        revolverCoinTrailStartColor = new ColorField(coinSubPanel, "Coin Trail Start Color", "CoinTrailStartColor", new Color(1, 1, 1, 0));
        revolverCoinTrailEndColor = new ColorField(coinSubPanel, "Coin Trail End Color", "CoinTrailEndColor", new Color(1, 1, 1, 0));

        marksmanMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(marksmanSubPanel, "Marksman Muzzle Flash Color", "MarksmanMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        marksmanBeamStartColor = new ColorField(marksmanSubPanel, "Normal Beam Start Color", "MarksmanBeamStartColor", new Color(1, 1, 1, 1));
        marksmanBeamEndColor = new ColorField(marksmanSubPanel, "Normal Beam End Color", "MarksmanBeamEndColor", new Color(1, 0.8078F, 0, 1));
        // Coin Ricochet beam after being shot by normal Marksman
        revolverCoinRicochetBeamStartColor = new ColorField(marksmanSubPanel, "Ricochet Beam Start Color", "RevolverCoinRicochetBeamStartColor", new Color(1, 0.8078F, 0, 1));
        revolverCoinRicochetBeamEndColor = new ColorField(marksmanSubPanel, "Ricochet Beam End Color", "RevolverCoinRicochetBeamEndColor", new Color(1, 0.8078F, 0, 1));


        // Alt Marksman
        altMarksmanEnabled = new BoolField(altMarksmanSubPanel, "Enabled", "altMarksmanEnabled", false);
        altMarksmanMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altMarksmanSubPanel, "Muzzle Flash Sprite Color", "AltMarksmanMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altMarksmanBeamStartColor = new ColorField(altMarksmanSubPanel, "Beam Start Color", "AltMarksmanBeamStartColor", new Color(1, 1, 1, 1));
        altMarksmanBeamEndColor = new ColorField(altMarksmanSubPanel, "Beam End Color", "AltMarksmanBeamEndColor", new Color(1, 0.7255F, 0, 1));

        // Sharpshooter
        sharpShooterEnabled = new BoolField(sharpShooterSubPanel, "Enabled", "sharpShooterEnabled", false);
        sharpShooterMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(sharpShooterSubPanel, "Muzzle Flash Sprite Color", "SharpShooterMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        sharpShooterChargeBeamStartColor = new ColorField(sharpShooterSubPanel, "Charge Beam Start Color", "SharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterChargeBeamEndColor = new ColorField(sharpShooterSubPanel, "Charge Beam End Color", "SharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));
        sharpShooterBeamStartColor = new ColorField(sharpShooterSubPanel, "Beam Start Color", "SharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterBeamEndColor = new ColorField(sharpShooterSubPanel, "Beam End Color", "SharpShooterBeamEndColor", new Color(1, 0.8078F, 0, 1));

        //ALt Sharpshooter
        altSharpShooterEnabled = new BoolField(altSharpShooterSubPanel, "Enabled", "altSharpshooterEnabled", false);
        altSharpShooterMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altSharpShooterSubPanel, "Muzzle Flash Color", "AltSharpShooterMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altSharpShooterBeamStartColor = new ColorField(altSharpShooterSubPanel, "Beam Start Color", "AltSharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterBeamEndColor = new ColorField(altSharpShooterSubPanel, "Beam End Color", "AltSharpShooterBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altSharpShooterChargeBeamStartColor = new ColorField(altSharpShooterSubPanel, "Charge Beam Start Color", "AltSharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterChargeBeamEndColor = new ColorField(altSharpShooterSubPanel, "Charge Beam End Color", "AltSharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));

        // Magnet nail
        magnetNailgunEnabled = new BoolField(magnetNailgunSubPanel, "Enabled", "magnetNailgunEnabled", false);
        magnetNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(magnetNailgunSubPanel, "Muzzle Flash Color", "MagnetNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        magnetNailgunTrailStartColor = new ColorField(magnetNailgunSubPanel, "Nail Trail Start Color", "MagnetNailgunNailTrailStartColor", new Color(0.251F, 0.9059F, 1, 0.4902F));
        magnetNailgunTrailEndColor = new ColorField(magnetNailgunSubPanel, "Nail Trail End Color", "MagnetNailgunNailTrailEndColor", new Color(0.251F, 0.9059F, 1, 0));

        altMagnetNailgunEnabled = new BoolField(altMagnetNailgunSubPanel, "Enabled", "altMagnetNailgunEnabled", false);
        altMagnetNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altMagnetNailgunSubPanel, "Muzzle Flash Color", "AltMagnetNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altMagnetNailgunTrailStartColor = new ColorField(altMagnetNailgunSubPanel, "Nail Trail Start Color", "AltMagnetNailgunNailTrailStartColor", new Color(0, 0.8745F, 1, 0.4902F));
        altMagnetNailgunTrailEndColor = new ColorField(altMagnetNailgunSubPanel, "Nail Trail End Color", "AltMagnetNailgunNailTrailEndColor", new Color(1, 1, 1, 0));

        overheatNailgunEnabled = new BoolField(overheatNailgunSubPanel, "Enabled", "overheatNailgunEnabled", false);
        overheatNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(overheatNailgunSubPanel, "Muzzle Flash Color", "OverheatNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        overheatNailgunTrailStartColor = new ColorField(overheatNailgunSubPanel, "Nail Trail Start Color", "OverheatNailgunNailTrailStartColor", new Color(1, 1, 1, 0.4902F));
        overheatNailgunTrailEndColor = new ColorField(overheatNailgunSubPanel, "Nail Trail End Color", "OverheatNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        overheatNailgunHeatedNailTrailStartColor = new ColorField(overheatNailgunSubPanel, "Heated Nail Trail Start Color", "OverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        overheatNailgunHeatedNailTrailEndColor = new ColorField(overheatNailgunSubPanel, "Heated Nail Trail End Color", "OverHeatNailgunHeatedNailTrailEndColor", new Color(1, 1, 1, 0));


        altOverheatNailgunEnabled = new BoolField(altOverheatNailgunSubPanel, "Enabled", "altOverheatNailgunEnabled", false);
        altOverheatNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altOverheatNailgunSubPanel, "Muzzle Flash Sprite Color", "AltOverheatNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altOverheatNailgunTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail Start Color", "AltOverheatNailgunNailTrailStartColor", new Color(0.502F, 0.502F, 0.502F, 0.4902F));
        altOverheatNailgunTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail End Color", "AltOverheatNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        altOverheatNailgunHeatedNailTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail Start Color", "AltOverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        altOverheatNailgunHeatedNailTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail End Color", "AltOverHeatNailgunHeatedNailTrailEndColor", new Color(1, 1, 1, 0));

        blueRailcannonEnabled = new BoolField(blueRailcannonSubPanel, "Enabled", "blueRailcannonEnabled", false);
        blueRailcannonStartColor = new ColorField(blueRailcannonSubPanel, "Beam Start Color", "BlueRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        blueRailcannonEndColor = new ColorField(blueRailcannonSubPanel, "Beam End Color", "BlueRailcannonBeamEndColor", new Color(0, 0.8353F, 1, 1));
        blueRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(blueRailcannonSubPanel, "Muzzle Flash Color", "BlueRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        redRailcannonEnabled = new BoolField(redRailcannonSubPanel, "Enabled", "redRailcannonEnabled", false);
        redRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(redRailcannonSubPanel, "Muzzle Flash Color", "RedRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        redRailcannonStartColor = new ColorField(redRailcannonSubPanel, "Beam Start Color", "RedRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        redRailcannonEndColor = new ColorField(redRailcannonSubPanel, "Beam End Color", "RedRailcannonBeamEndColor", new Color(1, 0.6314F, 0, 1));
        //redRailcannonGlowStartColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam Glow Start Color", "RedRailcannonBeamGlowStartColor", )

        greenRailcannonEnabled = new BoolField(greenRailcannonSubPanel, "Enabled", "greenRailcannonEnabled", false);
        greenRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(greenRailcannonSubPanel, "Muzzle Flash Color", "GreenRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        greenRailcannonTrailColor = new ColorField(greenRailcannonSubPanel, "Trail Color", "GreenRailcannonTrailColor", new Color(0, 1, 1, 1));

        freezeRocketLauncherEnabled = new BoolField(freezeRocketSubPanel, "Enabled", "freezeRocketLauncherEnabled", false);
        freezeRocketLauncherTrailStartColor = new ColorField(freezeRocketSubPanel, "Rocket Trail Start Color", "FreezeRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        freezeRocketLauncherTrailEndColor = new ColorField(freezeRocketSubPanel, "Rocket Trail End Color", "FreezeRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));

        cannonballRocketLauncherEnabled = new BoolField(cannonballRocketSubPanel, "Enabled", "cannonballRocketLauncherEnabled", false);
        cannonballRocketLauncherTrailStartColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail Start Color", "CannonballRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        cannonballRocketLauncherTrailEndColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail End Color", "CannonballRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));
    }
}
