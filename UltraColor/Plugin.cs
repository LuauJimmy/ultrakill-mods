using BepInEx;
using EffectChanger;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EffectChanger.Weapons;
using EffectChanger.Player;
using UnityEngine.Assertions;
using System.Runtime.CompilerServices;
using EffectChanger.Enemies;
using UnityEngine.UIElements.Experimental;
namespace UltraColor;
using PluginInfo = EffectChanger.PluginInfo;



[BepInDependency("com.eternalUnion.pluginConfigurator", BepInDependency.DependencyFlags.HardDependency)]
[BepInPlugin("aglooper", "aglooper", "0.0.4")]

public sealed class Plugin : BaseUnityPlugin
{
    public sealed class AssetDir : SortedDictionary<string, object>;

    private static string AssetPath;

    public static string? workingDir;
    public static string? ultraColorCatalogPath;
    public static Sprite? chargeBlankSprite;
    public static Texture2D? blankExplosionTexture;
    public static Sprite? blankMuzzleFlashSprite;
    public static Sprite? muzzleFlashInnerBase;
    public static Sprite? chargeBlank;
    public static Sprite? blankMuzzleFlashShotgunSprite;
    public static Sprite? shotgunInnerComponent;
    public static Sprite? defaultMuzzleFlashSprite;
    public static Texture2D? chargeBlankTexture;
    public static Texture2D? basicWhiteTexture;
    public static Texture2D? whiteSparkTexture;
    public static Texture2D? enrageEffectTextureBlank;
    public static Gradient? ElectricLineGradient;
    private static Color _revolverMuzzleFlashColor;
    private static bool debugMode;

    private static Vector3 rocketRotation;
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    public static T Fetch<T>(string key)
    {
        return Addressables.LoadAssetAsync<T>((object)key).WaitForCompletion();
    }

    public void Awake()
    {
        AssetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        Debug.Log("Pathname: " + AssetPath);
        debugMode = false;
        defaultMuzzleFlashSprite = Utils.CreateSpriteFromImage($"{AssetPath}\\muzzleflash.png");
        blankExplosionTexture = Utils.CreateTextureFromImage($"{AssetPath}\\explosion_blank.png");
        blankMuzzleFlashSprite = Utils.CreateSpriteFromImage($"{AssetPath}\\muzzleflashblank2.png");
        blankMuzzleFlashShotgunSprite = Utils.CreateSpriteFromImage($"{AssetPath}\\muzzleflashshotgunblank.png");
        muzzleFlashInnerBase = Utils.CreateSpriteFromImage($"{AssetPath}\\muzzleflash-innerbase.png");
        chargeBlankSprite = Utils.CreateSpriteFromImage($"{AssetPath}\\chargeblank.png", FilterMode.Point);
        shotgunInnerComponent = Utils.CreateSpriteFromImage($"{AssetPath}\\muzzleflashshotguninnercomponent.png");
        chargeBlankTexture = Utils.CreateTextureFromImage($"{AssetPath}\\chargeblank.png", FilterMode.Point);
        basicWhiteTexture = Utils.CreateTextureFromImage($"{AssetPath}\\basicwhite.png");
        whiteSparkTexture = Utils.CreateTextureFromImage($"{AssetPath}\\spark.png");
        enrageEffectTextureBlank = Utils.CreateTextureFromImage($"{AssetPath}\\RageEffect.png", FilterMode.Point);

        Settings.Init(this.Config);

        System.Type[] enabledPatches = [
            this.GetType(),
            typeof(EnemyProjectile),
            typeof(_Shotgun),
            typeof(_Revolver),
            typeof(_Nailgun),
            typeof(_RocketLauncher),
            typeof(_Railcannon),
            typeof(_Movement),
            typeof(_Idol),
            //(typeof(_EnrageEffect))
        ];

        foreach ( var type in enabledPatches )
        {
            Harmony.CreateAndPatchAll(type);
        }

        if (debugMode) { Harmony.CreateAndPatchAll(typeof(DebugPatches)); }

    }

    public void Start()
    {
        UltraColor.SetLogger(this.Logger);

    }

    public void Update()
    {
        _ = UltraColor.Instance;
    }
    

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(Explosion), nameof(Explosion.Start))]
    //private static bool RecolorKBExplosion(Explosion __instance)
    //{
    //    if (!Settings.knuckleBlasterEnabled.value) return true;
    //    Color expColor = Settings.knuckleBlasterExplosionColor.value;
    //    Color lightColor = Settings.knuckleBlasterLightColor.value;
    //    Color shockwaveColor = Settings.knuckleBlasterShockwaveSpriteColor.value;
    //    if (__instance.transform.parent.name == "Explosion Wave(Clone)")
    //    {
    //        var trans = __instance.transform.parent;
    //        trans.GetComponentInChildren<Light>().color = lightColor;
    //        trans.GetComponentInChildren<SpriteRenderer>().color = shockwaveColor;
    //        var mr = __instance.GetComponentInChildren<MeshRenderer>();
    //        var newMat = new Material(mr.material)
    //        {
    //            //mainTexture = blankExplosionTexture,
    //            //shaderKeywords = [/*"_FADING_ON",*/ "_EMISSION"],
    //            color = expColor,
    //        };
    //        mr.material = newMat;
    //    }
    //    return true;
    //}

    [HarmonyPrefix]
    [HarmonyPatch(typeof(ExplosionController), "Start")]
    private static bool RecolorExplosion(ExplosionController __instance)
    {
        //if (__instance.gameObject.name == "Explosion(Clone)")
        //{
        //    //if (global::UltraColor.Config.smallExplosionColor.value == ColorHelper.ExplosionColor.Default) return;

        //    //var newExplosionMat = ColorHelper.LoadExplosionMaterial(global::UltraColor.Config.smallExplosionColor.value);
        //    //var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
        //    //explosionRenderers[0].material = newExplosionMat;

        //    var mr = __instance.GetComponentsInChildren<MeshRenderer>();

        //    var newMat = new Material(mr[0].material);

        //    newMat.mainTexture = explosionTexture;

        //    mr[0].material = newMat;

        //    var explosionRenderers = __instance.gameObject.GetComponentsInChildren<MeshRenderer>();
        //    explosionRenderers[0].material = newMat;
        //}
        if (__instance.gameObject.name == "Explosion Malicious Railcannon(Clone)")
        {
            if (!Settings.maliciousExplosionEnabled.value) return true;

            var mr = __instance.GetComponentsInChildren<MeshRenderer>();
            var newMat = new Material(mr[0].material)
            {
                mainTexture = blankExplosionTexture,
                shaderKeywords = ["_FADING_ON", "_EMISSION"],
                color = Settings.maliciousExplosionColor.value
            };
            mr[0].material = newMat;
            //__instance.transform.Find("Sphere_8").gameObject.AddComponent<RendererFader>();
        }
        else if (__instance.gameObject.name == "Explosion Super(Clone)")
        {
            if (!Settings.nukeExplosionEnabled.value) return true;

            var mr = __instance.GetComponentsInChildren<MeshRenderer>();
            var newMat = new Material(mr[0].material)
            {
                mainTexture = blankExplosionTexture,
                shaderKeywords = ["_FADING_ON", "_EMISSION"],
                color = Settings.nukeExplosionColor.value
            };
            mr[0].material = newMat;
            //__instance.transform.Find("Sphere_8").gameObject.AddComponent<RendererFader>();
        }

        return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(CameraController), "Update")]
    private static bool UpdateCC(CameraController __instance)
    {
        __instance.CheckAspectRatio();
		if (Input.GetKeyDown(KeyCode.F1) && Debug.isDebugBuild)
		{
			if (Cursor.lockState != CursorLockMode.Locked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (__instance.cameraShaking > 0f)
		{
			if ((bool)MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused)
			{
				__instance.transform.localPosition = __instance.defaultPos;
			}
			else
			{
				Vector3 vector = __instance.transform.parent.position + __instance.defaultPos;
				Vector3 vector2 = vector;
				if (__instance.cameraShaking > 1f)
				{
					vector2 += __instance.transform.right * UnityEngine.Random.Range(-1, 2);
					vector2 += __instance.transform.up * UnityEngine.Random.Range(-1, 2);
				}
				else
				{
					vector2 += __instance.transform.right * (__instance.cameraShaking * UnityEngine.Random.Range(-1f, 1f));
					vector2 += __instance.transform.up * (__instance.cameraShaking * UnityEngine.Random.Range(-1f, 1f));
				}
				if (Physics.Raycast(vector, vector2 - vector, out var hitInfo, Vector3.Distance(vector2, vector) + 0.4f, __instance.environmentMask))
				{
					__instance.transform.position = hitInfo.point - (vector2 - vector).normalized * 0.5f;
				}
				else
				{
					__instance.transform.position = vector2;
				}
				__instance.cameraShaking -= Time.unscaledDeltaTime * 3f;
			}
		}
		if (__instance.platformerCamera)
		{
			return false;
		}
		if (__instance.player == null)
		{
			__instance.player = __instance.pm.gameObject;
		}
		__instance.scroll = Input.GetAxis("Mouse ScrollWheel");
		bool flag = __instance.activated;
		if (false)//MonoSingleton<InputManager>.Instance.LastButtonDevice is Gamepad && gamepadFreezeCount > 0)
		{
			flag = false;
		}
		if (GameStateManager.Instance.CameraLocked)
		{
			flag = false;
		}
		if (flag)
		{
			float num = 1f;
			Vector2 vector3 = MonoSingleton<InputManager>.Instance.InputSource.Look.ReadValue<Vector2>();
			if (__instance.zooming)
			{
				num = __instance.cam.fieldOfView / __instance.defaultFov;
			}
			if (!__instance.reverseY)
			{
				__instance.rotationX += vector3.y * (__instance.opm.mouseSensitivity / 10f) * num;
			}
			else
			{
				__instance.rotationX -= vector3.y * (__instance.opm.mouseSensitivity / 10f) * num;
			}
			if (!__instance.reverseX)
			{
				__instance.rotationY += vector3.x * (__instance.opm.mouseSensitivity / 10f) * num;
			}
			else
			{
				__instance.rotationY -= vector3.x * (__instance.opm.mouseSensitivity / 10f) * num;
			}
		}
		if (__instance.rotationY > 180f)
		{
			__instance.rotationY -= 360f;
		}
		else if (__instance.rotationY < -180f)
		{
			__instance.rotationY += 360f;
		}
		__instance.rotationX = Mathf.Clamp(__instance.rotationX, __instance.minimumX, __instance.maximumX);
		if (__instance.zooming)
		{
			__instance.cam.fieldOfView = Mathf.MoveTowards(__instance.cam.fieldOfView, __instance.zoomTarget, Time.deltaTime * 300f);
		}
		else if (__instance.pm.boost)
		{
			if (__instance.dodgeDirection == 0)
			{
				__instance.cam.fieldOfView = __instance.defaultFov - __instance.defaultFov / 20f;
			}
			else if (__instance.dodgeDirection == 1)
			{
				__instance.cam.fieldOfView = __instance.defaultFov + __instance.defaultFov / 10f;
			}
		}
		else
		{
			__instance.cam.fieldOfView = Mathf.MoveTowards(__instance.cam.fieldOfView, __instance.defaultFov, Time.deltaTime * 300f);
		}
		if ((bool)__instance.hudCamera)
		{
			if (__instance.zooming)
			{
				__instance.hudCamera.fieldOfView = Mathf.MoveTowards(__instance.hudCamera.fieldOfView, __instance.zoomTarget, Time.deltaTime * 300f);
			}
			else if (__instance.hudCamera.fieldOfView != 110f)
			{
				__instance.hudCamera.fieldOfView = Mathf.MoveTowards(__instance.hudCamera.fieldOfView, 110f, Time.deltaTime * 300f);
			}
		}
		if (flag)
		{
			__instance.player.transform.localEulerAngles = new Vector3(0f, __instance.rotationY, 0f);
		}
		float num2 = 0f;
		float num3 = __instance.movementHor * -1f;
		float num4 = __instance.transform.localEulerAngles.z;
		if (num4 > 180f)
		{
			num4 -= 360f;
		}
		num2 = ((!__instance.tilt) ? Mathf.MoveTowards(num4, 0f, Time.deltaTime * 25f * (Mathf.Abs(num4) + 0.01f)) : (__instance.pm.boost ? Mathf.MoveTowards(num4, num3 * 5f, Time.deltaTime * 100f * (Mathf.Abs(num4 - num3 * 5f) + 0.01f)) : Mathf.MoveTowards(num4, num3, Time.deltaTime * 25f * (Mathf.Abs(num4 - num3) + 0.01f))));
		if (flag)
		{
			__instance.transform.localEulerAngles = new Vector3(0f - __instance.rotationX, 0f, num2);
		}
		if (__instance.defaultPos != __instance.defaultTarget)
		{
			__instance.defaultPos = Vector3.MoveTowards(__instance.defaultPos, __instance.defaultTarget, ((__instance.defaultTarget - __instance.defaultPos).magnitude + 0.5f) * Time.deltaTime * 10f);
		}
		if (!__instance.pm.activated || !(__instance.cameraShaking <= 0f))
		{
			return false;
		}
		if (__instance.pm.walking && __instance.pm.standing && __instance.defaultPos == __instance.defaultTarget)
		{
			__instance.transform.localPosition = new Vector3(Mathf.MoveTowards(__instance.transform.localPosition.x, __instance.targetPos.x, Time.deltaTime * 0.5f), Mathf.MoveTowards(__instance.transform.localPosition.y, __instance.targetPos.y, Time.deltaTime * 0.5f * (Mathf.Min(__instance.pm.rb.velocity.magnitude, 15f) / 15f)), Mathf.MoveTowards(__instance.transform.localPosition.z, __instance.targetPos.z, Time.deltaTime * 0.5f));
			if (__instance.transform.localPosition == __instance.targetPos && __instance.targetPos != __instance.defaultPos)
			{
				__instance.targetPos = __instance.defaultPos;
			}
			else if (__instance.transform.localPosition == __instance.targetPos && __instance.targetPos == __instance.defaultPos)
			{
				__instance.targetPos = new Vector3(__instance.defaultPos.x, __instance.defaultPos.y - 0.1f, __instance.defaultPos.z);
			}
		}
		else
		{

            __instance.transform.localScale = Vector3.one * 0.5f;
            __instance.transform.localPosition = __instance.defaultPos;
			__instance.targetPos = new Vector3(__instance.defaultPos.x, __instance.defaultPos.y - 0.1f, __instance.defaultPos.z);
		}

		return false;
    }

    ////default explosion update
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Explosion), "Update")]
    private static bool UpdateExp(Explosion __instance)
    {
        if (__instance.light != null)
        {
            __instance.light.range += 5f * Time.deltaTime * __instance.speed;
        }
        if (__instance.whiteExplosion && (float)__instance.explosionTime > 0.1f)
        {
            __instance.whiteExplosion = false;
            __instance.mr.material = new Material(__instance.originalMaterial);
        }

        if (!__instance.fading) return false;
        __instance.materialColor.r -= 2f * Time.deltaTime;
        __instance.materialColor.g -= 2f * Time.deltaTime;
        __instance.materialColor.b -= 2f * Time.deltaTime;
        __instance.materialColor.a -= 2f * Time.deltaTime;

        if (__instance.light != null)
        {
            __instance.light.intensity -= 65f * Time.deltaTime;
        }
        __instance.mr.material.SetColor(Color1, __instance.materialColor);

        if (__instance.materialColor.a <= 0f)
        {
            Destroy(__instance.gameObject);
        }
        return false;
    }
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Explosion), "FixedUpdate")]
    private static void FixedUpdateExp(Explosion __instance)
    {
        __instance.transform.localScale += Vector3.one * 0.05f * __instance.speed;
        float num = __instance.transform.lossyScale.x * __instance.scol.radius;
        if (!__instance.fading && num > __instance.maxSize)
        {
            __instance.harmless = true;
            __instance.scol.enabled = false;
            __instance.fading = true;
            __instance.speed /= 4f;
        }
        if (!__instance.halved && num > __instance.maxSize / 2f)
        {
            __instance.halved = true;
            __instance.damage = Mathf.RoundToInt((float)__instance.damage / 1.5f);
        }
    }

    //[HarmonyPostfix]
    //[HarmonyPatch(typeof(Explosion), "Start")]
    //private static void expFader(Explosion __instance)
    //{
    //    __instance.gameObject.AddComponent<ExplosionFader>();
    //    var expMesh = __instance.transform.Find("Sphere_8");
    //    if (expMesh == null) return;
    //    expMesh.gameObject.AddComponent<ExplosionFader>();
    //    __instance.gameObject.AddComponent<ExplosionFader>();
    //}
}