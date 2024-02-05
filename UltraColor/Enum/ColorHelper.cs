using UltraColor;
using UnityEngine;

namespace EffectChanger.Enum
{
    public class ColorHelper
    {
        public enum MuzzleFlash
        {
            Default,
            Blue,
            Red,
            Purple
        }

        public enum BulletColor
        {
            Default,
            Purple,
            Yellow,
            Red,
            Blue
        }

        public enum ExplosionColor
        {
            Default,
            Orange,
            Red,
            Blue,
            Purple
        }

        public static Material LoadExplosionMaterial(ExplosionColor explosionColor)
        {
            string explosionMaterialPath;
            switch (explosionColor)
            {
                case ExplosionColor.Red:
                    explosionMaterialPath = "Assets/Materials/Explosion 1.mat";
                    break;

                case ExplosionColor.Blue:
                    explosionMaterialPath = "Assets/Materials/Explosion 2.mat";
                    break;

                case ExplosionColor.Orange:
                    explosionMaterialPath = "Assets/Materials/Explosion.mat";
                    break;

                case ExplosionColor.Purple:
                    explosionMaterialPath = "Assets/Materials/Sprites/Charge 1.mat";
                    break;

                default:
                    explosionMaterialPath = "Assets/Materials/Explosion.mat";
                    break;
            }
            return Plugin.Fetch<Material>(explosionMaterialPath);
        }

        public static Sprite LoadMuzzleFlashSprite(MuzzleFlash muzzleFlash)
        {
            string muzzleFlashPath;
            switch (muzzleFlash)
            {
                case MuzzleFlash.Blue:
                    muzzleFlashPath = "muzzleflashblue";
                    break;

                case MuzzleFlash.Red:
                    muzzleFlashPath = "muzzleflashnailgun";
                    break;

                case MuzzleFlash.Purple:
                    muzzleFlashPath = "muzzleflashturret";
                    break;

                default:
                    muzzleFlashPath = "muzzleflashshotgun";
                    break;
            }
            return Plugin.Fetch<Sprite>($"Assets/Textures/Sprites/{muzzleFlashPath}.png");
        }

        //public static RecolorPointLight

        //hacky as fuck but i literally dont give a fuck
        public static Material LoadBulletColor(BulletColor bulletColor)
        {
            return Plugin.Fetch<Material>($"Assets/Materials/Sprites/Charge {(int)bulletColor}.mat");
        }
    }
}