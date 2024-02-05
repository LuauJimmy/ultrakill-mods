using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace UltraColor
{
    public class Utils
    {
        private static string assetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D Tex = null;
            byte[] FileData;

            if (File.Exists(filePath))
            {
                Debug.Log("FOUND THE FILE!!!!!");
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
            }

            return Tex;
        }

        public static Sprite LoadPNG(string filePath)
        {
            Texture2D Tex = null;
            byte[] FileData;

            if (File.Exists(Path.Combine(assetPath, filePath)))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
            }

            return Sprite.Create(Tex, new Rect(0.0f, 0.0f, Tex.width, Tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }
    }
}