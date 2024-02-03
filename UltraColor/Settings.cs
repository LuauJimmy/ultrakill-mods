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

        ConfigPanel revolverSubPanel = new ConfigPanel(config.rootPanel, "Revolver", "RevolverSubPanel");
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
        shotgunMuzzleFlashPointLightColor = new ColorField(shotgunSubPanel, "Muzzle Flash Point Light Color", "MuzzleFlashPointLightColor", new Color(1, 0.7725f, 0, 1));
        shotgunProjectileStartColor = new ColorField(shotgunSubPanel, "Shotgun Projectile Start Color", "ShotgunProjectileStartColor", new Color(1, 0.7725F, 0, 1));
        shotgunProjectileEndColor = new ColorField(shotgunSubPanel, "Shotgun Projectile End Color", "ShotgunProjectileEndColor", new Color(1, 0.7725F, 0, 0));
        shotgunBulletColor = new EnumField<ColorHelper.BulletColor>(shotgunSubPanel, "Bullet Color", "ShotgunProjectileMeshColor", ColorHelper.BulletColor.Default);
        shotgunGrenadeSpriteColor = new EnumField<ColorHelper.MuzzleFlash>(shotgunSubPanel, "Grenade Sprite Color", "ShotgunGrenadeSpriteColor", ColorHelper.MuzzleFlash.Default);

        // Piercer 
        piercerRevolverEnabled = new BoolField(piercerRevolverSubPanel, "Enabled", "piercerEnabled", false);
        piercerRevolverChargeBeamStartColor = new ColorField(piercerRevolverSubPanel, "Piercer Charge Beam Start Color", "PiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeBeamEndColor = new ColorField(piercerRevolverSubPanel, "Piercer Charge Beam End Color", "PiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));
        piercerRevolverBeamStartColor = new ColorField(piercerRevolverSubPanel, "Piercer Beam Start Color", "PiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverBeamEndColor = new ColorField(piercerRevolverSubPanel, "Piercer Beam End Color", "PiercerRevolverBeamEndColor", new Color(1, 0.8078F, 0, 1));
        piercerRevolverMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(piercerRevolverSubPanel, "Piercer Muzzle Flash Color", "PiercerRevolverMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        piercerRevolverChargeMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(piercerRevolverSubPanel, "Piercer Charge Shot Muzzle Flash Color", "PiercerRevolverChargeMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        piercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(piercerRevolverSubPanel, "Piercer Charge Effect Color", "PiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);

        // Alt Piercer
        altPiercerRevolverEnabled = new BoolField(altPiercerRevolverSubPanel, "Enabled", "altPiercerEnabled", false);
        altPiercerRevolverChargeBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Alt Piercer Charge Beam Start Color", "AltPiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Alt Piercer Charge Beam End Color", "AltPiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));
        altPiercerRevolverBeamStartColor = new ColorField(altPiercerRevolverSubPanel, "Alt Piercer Beam Start Color", "AltPiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        altPiercerRevolverBeamEndColor = new ColorField(altPiercerRevolverSubPanel, "Alt Piercer Beam End Color", "AltPiercerRevolverBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altPiercerRevolverMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altPiercerRevolverSubPanel, "Alt Piercer Muzzle Flash Color", "AltPiercerRevolverMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altPiercerRevolverChargeMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altPiercerRevolverSubPanel, "Alt Piercer Charge Shot Muzzle Flash Color", "AltPiercerRevolverChargeMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);
        altPiercerRevolverChargeEffectColor = new EnumField<ColorHelper.BulletColor>(altPiercerRevolverSubPanel, "Alt Piercer Charge Effect Color", "AltPiercerRevolverChargeEffectColor", ColorHelper.BulletColor.Default);

        // Marksman
        marksmanBeamStartColor = new ColorField(marksmanSubPanel, "Marksman Beam Start Color", "MarksmanBeamStartColor", new Color(1, 1, 1, 1));
        marksmanBeamEndColor = new ColorField(marksmanSubPanel, "Marksman Beam End Color", "MarksmanBeamEndColor", new Color(1, 0.8078F, 0, 1));
        marksmanMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(marksmanSubPanel, "Marksman Muzzle Flash Color", "MarksmanMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        // Alt Marksman
        altMarksmanEnabled = new BoolField(altMarksmanSubPanel, "Enabled", "altMarksmanEnabled", false);
        altMarksmanBeamStartColor = new ColorField(altMarksmanSubPanel, "Alt Marksman Beam Start Color", "AltMarksmanBeamStartColor", new Color(1, 1, 1, 1));
        altMarksmanBeamEndColor = new ColorField(altMarksmanSubPanel, "Alt Marksman Beam End Color", "AltMarksmanBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altMarksmanMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altMarksmanSubPanel, "Alt MarksmanMuzzle Flash Color", "AltMarksmanMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        // Sharpshooter
        sharpShooterEnabled = new BoolField(sharpShooterSubPanel, "Enabled", "sharpShooterEnabled", false);
        sharpShooterChargeBeamStartColor = new ColorField(sharpShooterSubPanel, "Sharpshooter Charge Beam Start Color", "SharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterChargeBeamEndColor = new ColorField(sharpShooterSubPanel, "Sharpshooter Charge Beam End Color", "SharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));
        sharpShooterBeamStartColor = new ColorField(sharpShooterSubPanel, "Sharpshooter Beam Start Color", "SharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        sharpShooterBeamEndColor = new ColorField(sharpShooterSubPanel, "Sharpshooter Beam End Color", "SharpShooterBeamEndColor", new Color(1, 0.8078F, 0, 1));
        sharpShooterMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(sharpShooterSubPanel, "Sharpshooter Muzzle Flash Color", "SharpShooterMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        //ALt Sharpshooter
        altSharpShooterEnabled = new BoolField(altSharpShooterSubPanel, "Enabled", "altSharpshooterEnabled", false);
        altSharpShooterChargeBeamStartColor = new ColorField(altSharpShooterSubPanel, "Alt Sharpshooter Charge Beam Start Color", "AltSharpShooterChargeBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterChargeBeamEndColor = new ColorField(altSharpShooterSubPanel, "Alt Sharpshooter Charge Beam End Color", "AltSharpShooterChargeBeamEndColor", new Color(1, 0.2353F, 0.2353F, 1));
        altSharpShooterBeamStartColor = new ColorField(altSharpShooterSubPanel, "Alt Sharpshooter Beam Start Color", "AltSharpShooterBeamStartColor", new Color(1, 1, 1, 1));
        altSharpShooterBeamEndColor = new ColorField(altSharpShooterSubPanel, "Alt Sharpshooter Beam End Color", "AltSharpShooterBeamEndColor", new Color(1, 0.7255F, 0, 1));
        altSharpShooterMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altSharpShooterSubPanel, "Alt Sharpshooter Muzzle Flash Color", "AltSharpShooterMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);


        // Coin Trail
        marksmanEnabled = new BoolField(marksmanSubPanel, "Enabled", "marksmanEnabled", false);
        revolverCoinTrailStartColor = new ColorField(marksmanSubPanel, "Coin Trail Start Color", "CoinTrailStartColor", new Color(1, 1, 1, 0));
        revolverCoinTrailEndColor = new ColorField(marksmanSubPanel, "Coin Trail End Color", "CoinTrailEndColor", new Color(1, 1, 1, 0));
        // Coin Ricochet beam after being shot by normal Marksman
        revolverCoinRicochetBeamStartColor = new ColorField(marksmanSubPanel, "Revolver Coin Ricochet Beam Start Color", "RevolverCoinRicochetBeamStartColor", new Color(1, 0.8078F, 0, 1));
        revolverCoinRicochetBeamEndColor = new ColorField(marksmanSubPanel, "Revolver Coin Ricochet Beam End Color", "RevolverCoinRicochetBeamEndColor", new Color(1, 0.8078F, 0, 1));

        // Magnet nail
        magnetNailgunEnabled = new BoolField(magnetNailgunSubPanel, "Enabled", "magnetNailgunEnabled", false);
        magnetNailgunTrailStartColor = new ColorField(magnetNailgunSubPanel, "Magnet Nailgun Nail Trail Start Color", "MagnetNailgunNailTrailStartColor", new Color(0.251F, 0.9059F, 1, 0.4902F));
        magnetNailgunTrailEndColor = new ColorField(magnetNailgunSubPanel, "Magnet Nailgun Nail Trail End Color", "MagnetNailgunNailTrailEndColor", new Color(0.251F, 0.9059F, 1, 0));
        magnetNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(magnetNailgunSubPanel, "Magnet Nailgun Muzzle Flash Color", "MagnetNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        altMagnetNailgunEnabled = new BoolField(altMagnetNailgunSubPanel, "Enabled", "altMagnetNailgunEnabled", false);
        altMagnetNailgunTrailStartColor = new ColorField(altMagnetNailgunSubPanel, "Alt Magnet Nailgun Nail Trail Start Color", "AltMagnetNailgunNailTrailStartColor", new Color(0, 0.8745F, 1, 0.4902F));
        altMagnetNailgunTrailEndColor = new ColorField(altMagnetNailgunSubPanel, "Alt Magnet Nailgun Nail Trail End Color", "AltMagnetNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        altMagnetNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altMagnetNailgunSubPanel, "Alt Magnet Nailgun Muzzle Flash Color", "AltMagnetNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        overheatNailgunEnabled = new BoolField(overheatNailgunSubPanel, "Enabled", "overheatNailgunEnabled", false);
        overheatNailgunTrailStartColor = new ColorField(overheatNailgunSubPanel, "Overheat Nailgun Nail Trail Start Color", "OverheatNailgunNailTrailStartColor", new Color(1, 1, 1, 0.4902F));
        overheatNailgunTrailEndColor = new ColorField(overheatNailgunSubPanel, "Overheat Nailgun Nail Trail End Color", "OverheatNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        overheatNailgunHeatedNailTrailStartColor = new ColorField(overheatNailgunSubPanel, "Overheat Nailgun Heated Nail Trail Start Color", "OverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        overheatNailgunHeatedNailTrailEndColor = new ColorField(overheatNailgunSubPanel, "Overheat Nailgun Heated Nail Trail End Color", "OverHeatNailgunHeatedNailTrailEndColor", new Color(1, 1, 1, 0));

        overheatNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(overheatNailgunSubPanel, "Overheat Nailgun Muzzle Flash Color", "OverheatNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        altOverheatNailgunEnabled = new BoolField(altOverheatNailgunSubPanel, "Enabled", "altOverheatNailgunEnabled", false);
        altOverheatNailgunTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Alt Overheat Nailgun Nail Trail Start Color", "AltOverheatNailgunNailTrailStartColor", new Color(0.502F, 0.502F, 0.502F, 0.4902F));
        altOverheatNailgunTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Alt Overheat Nailgun Nail Trail End Color", "AltOverheatNailgunNailTrailEndColor", new Color(1, 1, 1, 0));
        altOverheatNailgunHeatedNailTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Alt Overheat Nailgun Heated Nail Trail Start Color", "AltOverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        altOverheatNailgunHeatedNailTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Alt Overheat Nailgun Heated Nail Trail End Color", "AltOverHeatNailgunHeatedNailTrailEndColor", new Color(1, 1, 1, 0));
        altOverheatNailgunMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(altOverheatNailgunSubPanel, "ALt Overheat Nailgun Muzzle Flash Color", "AltOverheatNailgunMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        blueRailcannonEnabled = new BoolField(blueRailcannonSubPanel, "Enabled", "blueRailcannonEnabled", false);
        blueRailcannonStartColor = new ColorField(blueRailcannonSubPanel, "Blue Railcannon Beam Start Color", "BlueRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        blueRailcannonEndColor = new ColorField(blueRailcannonSubPanel, "Blue Railcannon Beam End Color", "BlueRailcannonBeamEndColor", new Color(0, 0.8353F, 1, 1));
        blueRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(blueRailcannonSubPanel, "Blue Railcannon Muzzle Flash Color", "BlueRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        redRailcannonEnabled = new BoolField(redRailcannonSubPanel, "Enabled", "redRailcannonEnabled", false);
        redRailcannonStartColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam Start Color", "RedRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        redRailcannonEndColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam End   Color", "RedRailcannonBeamEndColor", new Color(1, 0.6314F, 0, 1));
        //redRailcannonGlowStartColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam Glow Start Color", "RedRailcannonBeamGlowStartColor", )
        redRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(redRailcannonSubPanel, "Red Railcannon Muzzle Flash Color", "RedRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        greenRailcannonEnabled = new BoolField(greenRailcannonSubPanel, "Enabled", "greenRailcannonEnabled", false);
        greenRailcannonTrailColor = new ColorField(greenRailcannonSubPanel, "Green Railcannon Trail Color", "GreenRailcannonTrailColor", new Color(0, 1, 1, 1));
        greenRailcannonMuzzleFlashColor = new EnumField<ColorHelper.MuzzleFlash>(greenRailcannonSubPanel, "Green Railcannon Muzzle Flash Color", "GreenRailcannonMuzzleFlashColor", ColorHelper.MuzzleFlash.Default);

        freezeRocketLauncherEnabled = new BoolField(freezeRocketSubPanel, "Enabled", "freezeRocketLauncherEnabled", false);
        freezeRocketLauncherTrailStartColor = new ColorField(freezeRocketSubPanel, "FreezeFrame Rocket Trail Start Color", "FreezeRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        freezeRocketLauncherTrailEndColor = new ColorField(freezeRocketSubPanel, "FreezeFrame Rocket Trail End Color", "FreezeRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));

        cannonballRocketLauncherEnabled = new BoolField(cannonballRocketSubPanel, "Enabled", "cannonballRocketLauncherEnabled", false);
        cannonballRocketLauncherTrailStartColor = new ColorField(cannonballRocketSubPanel, "S.R.S. Cannon Rocket Trail Start Color", "CannonballRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        cannonballRocketLauncherTrailEndColor = new ColorField(cannonballRocketSubPanel, "S.R.S. Cannon Rocket Trail End Color", "CannonballRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));
    }
}
