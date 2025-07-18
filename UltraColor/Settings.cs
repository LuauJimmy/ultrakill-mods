using BepInEx.Configuration;
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
    public static ColorField shotgunProjectileBoostStartColor;
    public static ColorField shotgunProjectileBoostEndColor;
    public static ColorField shotgunMuzzleFlashColor;
    public static ColorField shotgunMuzzleFlashPointLightColor;
    public static ColorField shotgunBulletColor;
    public static ColorField shotgunGrenadeSpriteColor;

    public static BoolField hammerEnabled;
    public static ColorField hammerSpriteInnerColor;
    public static ColorField hammerSpriteOuterColor;

    public static BoolField chainsawEnabled;
    public static ColorField chainsawTrailStartColor;
    public static ColorField chainsawTrailEndColor;
    public static ColorField chainsawSpriteColor;

    public static BoolField piercerRevolverEnabled;
    public static ColorField piercerRevolverChargeBeamStartColor;
    public static ColorField piercerRevolverChargeBeamEndColor;
    public static ColorField piercerRevolverBeamStartColor;
    public static ColorField piercerRevolverBeamEndColor;
    public static ColorField piercerRevolverMuzzleFlashColor;
    public static ColorField piercerRevolverChargeMuzzleFlashColor;
    public static ColorField piercerRevolverChargeEffectColor;

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
    public static ColorField revolverCoinFlashColor;
    public static ColorField marksmanMuzzleFlashColor;
    public static BoolField revolverCoinFlashEnabled;

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
    public static ColorField altPiercerRevolverChargeEffectColor;

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

    public static BoolField conductorEnabled;
    public static ColorField conductorStartColor;
    public static ColorField conductorEndColor;
    
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

    public static BoolField revolverCoinRicochetBeamEnabled;
    public static ColorField revolverCoinRicochetBeamStartColor;
    public static ColorField revolverCoinRicochetBeamEndColor;

    public static BoolField freezeRocketLauncherEnabled;
    public static ColorField freezeRocketLauncherTrailStartColor;
    public static ColorField freezeRocketLauncherTrailEndColor;

    public static BoolField cannonballRocketLauncherEnabled;
    public static ColorField cannonballRocketLauncherTrailStartColor;
    public static ColorField cannonballRocketLauncherTrailEndColor;
    public static ColorField cannonballTrailStartColor;
    public static ColorField cannonballTrailEndColor;

    public static ColorField knuckleBlasterShockwaveSpriteColor;
    public static ColorField knuckleBlasterExplosionColor;
    public static ColorField knuckleBlasterLightColor;
    public static BoolField knuckleBlasterEnabled;

    public static BoolField slideScrapeEnabled;
    public static ColorField slideScrapeSparksStartColor;
    public static ColorField slideScrapeSparksEndColor;
    public static BoolField dodgeEnabled;
    public static ColorField dodgeStartColor;
    public static ColorField dodgeEndColor;
    public static BoolField slamEnabled;
    public static ColorField slamStartColor;
    public static ColorField slamEndColor;

    public static BoolField blessingEnabled;
    public static ColorField blessingBeamColor;
    public static ColorField blessingColor;
    public static ColorField idolHaloColor;
    public static ColorField idolSpikesColor;

    public static BoolField enrageEnabled;
    public static ColorField enrageSpriteColor;
    public static ColorField enrageLightningColor;

    public static BoolField enemyProjEnabled;
    public static ColorField enemyProjColor;
    public static ColorField enemyProjInnerGlowColor;
    public static ColorField enemyProjTrailStartColor;
    public static ColorField enemyProjTrailEndColor;
    public static ColorField enemyProjOuterGlowColor;

    public static BoolField homingProjEnabled;
    public static ColorField homingProjColor;
    public static ColorField homingProjInnerGlowColor;
    public static ColorField homingProjTrailStartColor;
    public static ColorField homingProjTrailEndColor;
    public static ColorField homingProjOuterGlowColor;

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

        ConfigPanel effectsSubPanel = new ConfigPanel(config.rootPanel, "Misc Effects", "EffectsSubPanel");

        ConfigPanel explosionSubPanel = new ConfigPanel(config.rootPanel, "Explosion", "ExplosionSubPanel");

        ConfigPanel knuckleBlasterSubPanel = new ConfigPanel(config.rootPanel, "Knuckle Blaster", "KnuckleBlasterSubPanel");

        ConfigPanel enemiesSubPanel = new ConfigPanel(config.rootPanel, "Enemies", "EnemiesSubPanel");

        ConfigPanel enemyProjSubPanel = new ConfigPanel(enemiesSubPanel, "Enemy Projectiles", "EnemyProjSubPanel");

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
        shotgunProjectileEndColor = new ColorField(shotgunSubPanel, "Projectile Trail End Color", "ShotgunProjectileEndColor", new Color(0, 0, 0, 0));
        shotgunProjectileBoostStartColor = new ColorField(shotgunSubPanel, "Projectile Boost Trail Start Color", "ShotgunProjectileBoostStartColor", new Color(1f, 0.35f, 0f));
        shotgunProjectileBoostEndColor = new ColorField(shotgunSubPanel, "Projectile Boost Trail End Color", "ShotgunProjectileBoostEndColor", new Color(0f, 0f, 0f));
        shotgunBulletColor = new ColorField(shotgunSubPanel, "Bullet Color", "ShotgunProjectileMeshColor", new Color(1f, 0.35f, 0f));
        shotgunGrenadeSpriteColor = new ColorField(shotgunSubPanel, "Grenade Sprite Color", "ShotgunGrenadeSpriteColor", new Color(1f, 0.35f, 0f));

        // Hammer
        hammerEnabled = new BoolField(shotgunSubPanel, "Hammer Enabled", "hammerEnabled", false);
        hammerSpriteInnerColor = new ColorField(shotgunSubPanel, "Hammer impact inner color", "HammerImpactInnerColor", new Color(1, 0, 0));
        hammerSpriteOuterColor = new ColorField(shotgunSubPanel, "Hammer impact outer color", "HammerImpactOuterColor", new Color(1, 0, 0));

        //Chainsaw
        chainsawEnabled = new BoolField(shotgunSubPanel, "Chainsaw Enabled", "chainsawEnabled", false);
        chainsawTrailStartColor = new ColorField(shotgunSubPanel, "Chainsaw Trail Start Color", "ChainsawTrailInnerColor", new Color(1, 0, 0));
        chainsawTrailEndColor = new ColorField(shotgunSubPanel, "Chainsaw Trail End Color", "ChainsawTrailEndColor", new Color(1, 0, 0));
        chainsawSpriteColor = new ColorField(shotgunSubPanel, "Chainsaw Sprite Color", "ChainsawSpriteColor", new Color(1, 0, 0));

        // Piercer
        piercerRevolverEnabled = new BoolField(piercerRevolverSubPanel, "Enabled", "piercerEnabled", false);
        piercerRevolverMuzzleFlashColor = new ColorField(piercerRevolverSubPanel, "Muzzle Flash Sprite Color", "PiercerRevolverMuzzleFlashColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeMuzzleFlashColor = new ColorField(piercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "PiercerRevolverChargeMuzzleFlashColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeEffectColor = new ColorField(piercerRevolverSubPanel, "Charge Effect Color", "PiercerRevolverChargeEffectColor", new Color(0.1f, 0.25f, 0.85f, 1));
        piercerRevolverBeamStartColor = new ColorField(piercerRevolverSubPanel, "Normal Beam Start Color", "PiercerRevolverBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverBeamEndColor = new ColorField(piercerRevolverSubPanel, "Normal Beam End Color", "PiercerRevolverBeamEndColor", new Color(1, 0.8078F, 0, 1));
        piercerRevolverChargeBeamStartColor = new ColorField(piercerRevolverSubPanel, "Charge Beam Start Color", "PiercerRevolverChargeBeamStartColor", new Color(1, 1, 1, 1));
        piercerRevolverChargeBeamEndColor = new ColorField(piercerRevolverSubPanel, "Charge Beam End Color", "PiercerRevolverChargeBeamEndColor", new Color(0, 0.8353F, 1, 1));

        // Alt Piercer
        altPiercerRevolverEnabled = new BoolField(altPiercerRevolverSubPanel, "Enabled", "altPiercerEnabled", false);
        altPiercerRevolverMuzzleFlashColor = new ColorField(altPiercerRevolverSubPanel, "Muzzle Flash Sprite Color", "AltPiercerRevolverMuzzleFlashColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeMuzzleFlashColor = new ColorField(altPiercerRevolverSubPanel, "Charge Muzzle Flash Sprite Color", "AltPiercerRevolverChargeMuzzleFlashColor", new Color(1, 1, 1, 1));
        altPiercerRevolverChargeEffectColor = new ColorField(altPiercerRevolverSubPanel, "Charge Effect Color", "AltPiercerRevolverChargeEffectColor", new Color(0.1f, 0.25f, 0.85f, 1));
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
        revolverCoinFlashEnabled = new BoolField(coinSubPanel, "Flash Enabled", "CoinFlashEnabled", false);
        revolverCoinFlashColor = new ColorField(coinSubPanel, "Coin Flash Color", "CoinFlashColor", new Color(1, 0.6f, 0, 1));

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
        altOverheatNailgunTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail Start Color", "AltOverheatNailgunNailTrailStartColor", new Color(0.502F, 0.502F, 0.502F, 0.4902F));
        altOverheatNailgunTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Nail Trail End Color", "AltOverheatNailgunNailTrailEndColor", new Color(0, 0, 0, 0));
        altOverheatNailgunMuzzleFlashColor = new ColorField(altOverheatNailgunSubPanel, "Muzzle Flash Sprite Color", "AltOverheatNailgunMuzzleFlashColor", new Color(1, 0.7725f, 0, 1));
        altOverheatNailgunHeatedNailTrailStartColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail Start Color", "AltOverHeatNailgunHeatedNailTrailStartColor", new Color(1, 0.5922F, 0, 0.4902F));
        altOverheatNailgunHeatedNailTrailEndColor = new ColorField(altOverheatNailgunSubPanel, "Heated Nail Trail End Color", "AltOverHeatNailgunHeatedNailTrailEndColor", new Color(0, 0, 0, 0));

        blueRailcannonEnabled = new BoolField(blueRailcannonSubPanel, "Enabled", "blueRailcannonEnabled", false);
        blueRailcannonMuzzleFlashColor = new ColorField(blueRailcannonSubPanel, "Muzzle Flash Color", "BlueRailcannonMuzzleFlashColor", new Color(0f, 0.8f, 0.1f));
        blueRailcannonStartColor = new ColorField(blueRailcannonSubPanel, "Beam Start Color", "BlueRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        blueRailcannonEndColor = new ColorField(blueRailcannonSubPanel, "Beam End Color", "BlueRailcannonBeamEndColor", new Color(0, 0.8353F, 1, 1));

        redRailcannonEnabled = new BoolField(redRailcannonSubPanel, "Enabled", "redRailcannonEnabled", false);
        redRailcannonMuzzleFlashColor = new ColorField(redRailcannonSubPanel, "Muzzle Flash Color", "RedRailcannonMuzzleFlashColor", new Color(1, 1, 1,1));
        redRailcannonStartColor = new ColorField(redRailcannonSubPanel, "Beam Start Color", "RedRailcannonBeamStartColor", new Color(1, 1, 1, 1));
        redRailcannonEndColor = new ColorField(redRailcannonSubPanel, "Beam End Color", "RedRailcannonBeamEndColor", new Color(1, 0.6314F, 0, 1));
        //redRailcannonGlowStartColor = new ColorField(redRailcannonSubPanel, "Red Railcannon Beam Glow Start Color", "RedRailcannonBeamGlowStartColor", )

        greenRailcannonEnabled = new BoolField(greenRailcannonSubPanel, "Enabled", "greenRailcannonEnabled", false);
        greenRailcannonTrailStartColor = new ColorField(greenRailcannonSubPanel, "Trail Start Color", "GreenRailcannonTrailStartColor", new Color(0.2667F, 1, 0.2706F, 1));
        greenRailcannonTrailEndColor = new ColorField(greenRailcannonSubPanel, "Trail End Color", "GreenRailcannonTrailEndColor", new Color(0, 0, 0, 0));

        freezeRocketLauncherEnabled = new BoolField(freezeRocketSubPanel, "Enabled", "freezeRocketLauncherEnabled", false);
        freezeRocketLauncherTrailStartColor = new ColorField(freezeRocketSubPanel, "Rocket Trail Start Color", "FreezeRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        freezeRocketLauncherTrailEndColor = new ColorField(freezeRocketSubPanel, "Rocket Trail End Color", "FreezeRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));

        cannonballRocketLauncherEnabled = new BoolField(cannonballRocketSubPanel, "Enabled", "cannonballRocketLauncherEnabled", false);
        cannonballRocketLauncherTrailStartColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail Start Color", "CannonballRocketLauncherTrailStartColor", new Color(1, 0.502F, 0, 0.3922F));
        cannonballRocketLauncherTrailEndColor = new ColorField(cannonballRocketSubPanel, "Rocket Trail End Color", "CannonballRocketLauncherTrailEndColor", new Color(1, 0, 0, 0));
        cannonballTrailStartColor = new ColorField(cannonballRocketSubPanel, "Cannonball Trail Start Color", "CannonballTrailStartColor", new Color(0.4902f, 0.4902f, 0.4902f, 1));
        cannonballTrailEndColor = new ColorField(cannonballRocketSubPanel, "Cannonball Trail End Color", "CannonballTrailEndColor", new Color(0, 0, 0, 0));

        // Movement Stuff
        slideScrapeEnabled = new BoolField(effectsSubPanel, "Slide Scrape Enabled", "SlideScrapeEnabled", false);
        slideScrapeSparksStartColor = new ColorField(effectsSubPanel, "Slide Scrape Sparks Start Color", "SlideScrapeSparksStartColor", new Color(0.3f, 0.3f, 0.3f, 1));
        slideScrapeSparksEndColor = new ColorField(effectsSubPanel, "Slide Scrape Sparks End Color", "SlideScrapeSparksEndColor", new Color(0.3f, 0.3f, 0.3f, 1));

        dodgeEnabled = new BoolField(effectsSubPanel, "Dodge Enabled", "DodgeEnabled", false);
        dodgeStartColor = new ColorField(effectsSubPanel, "Dodge Start Color", "DodgeStartColor", new Color(0.3f, 0.3f, 0.3f, 1));
        dodgeEndColor = new ColorField(effectsSubPanel, "Dodge End Color", "DodgeEndColor", new Color(0.3f, 0.3f, 0.3f, 1));

        slamEnabled = new BoolField(effectsSubPanel, "Slam Enabled", "SlamEnabled", false);
        slamStartColor = new ColorField(effectsSubPanel, "Slam Start Color", "SlamStartColor", new Color(0.3f, 0.3f, 0.3f, 1));
        slamEndColor = new ColorField(effectsSubPanel, "Slam End Color", "SlamSparksEndColor", new Color(0.3f, 0.3f, 0.3f, 1));

        knuckleBlasterEnabled = new BoolField(knuckleBlasterSubPanel, "Knuckleblaster Enabled", "knuckleBlasterEnabled", false);
        knuckleBlasterShockwaveSpriteColor = new ColorField(knuckleBlasterSubPanel, "Shockwave Color", "knuckleBlasterShockwaveSpriteColor", new Color(0.63f, 0.63f, 0.63f, 1));
        knuckleBlasterExplosionColor = new ColorField(knuckleBlasterSubPanel, "Explosion Color", "knuckleBlasterExplosionColor", new Color(1, 1, 1, 1));
        knuckleBlasterLightColor = new ColorField(knuckleBlasterSubPanel, "Light Color", "knuckleBlasterLightColor", new Color(1, 0.5754f, 0, 1));

        conductorEnabled = new BoolField(effectsSubPanel, "Conductor Enabled", "ConductorEnabled", false);
        conductorStartColor = new ColorField(effectsSubPanel, "Conductor Color 1", "ConductorStartColor", new Color(1f, 0.8f, 0.8f, 0.8f));
        conductorEndColor = new ColorField(effectsSubPanel, "Conductor Color 2", "ConductorEndColor", new Color(1, 1, 1, 1));

        // Enemy Settings
        // Blessing
        blessingEnabled = new BoolField(enemiesSubPanel, "Enabled Recolor Enemy Blessing", "blessingEnabled", false);
        blessingColor = new ColorField(enemiesSubPanel, "Enemy Blessing Color", "blessingColor", new Color(0.3f, 0.5f, 0.8f));
        blessingBeamColor = new ColorField(enemiesSubPanel, "Blessing Beam Color", "blessingBeamColor", new Color(0.3f, 0.5f, 0.8f));
        idolHaloColor = new ColorField(enemiesSubPanel, "Idol Halo Color", "idolHaloColor", new Color(0.3f, 0.5f, 0.8f));
        idolSpikesColor = new ColorField(enemiesSubPanel, "Idol Spikes Color", "idolSpikesColor", new Color(0.3f, 0.5f, 0.8f));

        // Enemy Settings
        // Enrage
        enrageEnabled = new BoolField(enemiesSubPanel, "Enrage Enabled", "enrageEnabled", false);
        enrageSpriteColor = new ColorField(enemiesSubPanel, "Enrage Circle Color", "EnrageSpriteColor", new Color(1, 0, 0));
        enrageLightningColor = new ColorField(enemiesSubPanel, "Enrage Lightning Color", "EnrageLightningColor", new Color(1, 0, 0));

        // Enemy Settings
        // "Normal" Enemy Projectile

        enemyProjEnabled = new BoolField(enemyProjSubPanel, "Enable Enemy Projectile", "enemyProjEnabled", false);
        enemyProjColor = new ColorField(enemyProjSubPanel, "Enemy Projectile Color", "enemyProjColor", new Color(1, 1, 1));
        enemyProjInnerGlowColor = new ColorField(enemyProjSubPanel, "Enemy Projectile Inner Glow Color", "enemyProjInnerGlowColor", new Color(1, 1, 1));
        enemyProjTrailStartColor = new ColorField(enemyProjSubPanel, "Enemy Projectile Trail Start Color", "enemyProjTrailStartColor", new Color(1, 1, 1));
        enemyProjTrailEndColor = new ColorField(enemyProjSubPanel, "Enemy Projectile Trail End Color", "enemyProjTrailEndColor", new Color(1, 1, 1));
        enemyProjOuterGlowColor = new ColorField(enemyProjSubPanel, "Enemy Projectile Outer Glow Color", "enemyProjOuterGlowColor", new Color(1, 1, 1));

        homingProjEnabled = new BoolField(enemyProjSubPanel, "Enable homing Projectile", "homingProjEnabled", false);
        homingProjColor = new ColorField(enemyProjSubPanel, "Homing Projectile Color", "homingProjColor", new Color(1, 1, 1));
        homingProjInnerGlowColor = new ColorField(enemyProjSubPanel, "Homing Projectile Inner Glow Color", "homingProjInnerGlowColor", new Color(1, 1, 1));
        homingProjTrailStartColor = new ColorField(enemyProjSubPanel, "Homing Projectile Trail Start Color", "homingProjTrailStartColor", new Color(1, 1, 1));
        homingProjTrailEndColor = new ColorField(enemyProjSubPanel, "Homing Projectile Trail End Color", "homingProjTrailEndColor", new Color(1, 1, 1));
        homingProjOuterGlowColor = new ColorField(enemyProjSubPanel, "Homing Projectile Outer Glow Color", "homingProjOuterGlowColor", new Color(1, 1, 1));


    }
}