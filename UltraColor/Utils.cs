using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace UltraColor
{
    public class Utils
    {
        private static string assetPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

        public sealed class AssetDir : SortedDictionary<string, object>;

        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D Tex = null;
            byte[] FileData;

            if (File.Exists(filePath))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
            }

            return Tex;
        }

        public static Sprite LoadPNG(string filePath)
        {
            Debug.Log($"trying filePath: {filePath}");
            Texture2D Tex = null;
            byte[] FileData;
            if (File.Exists(filePath))
            {
                FileData = File.ReadAllBytes(filePath);
                Tex = new Texture2D(2, 2);
                Tex.LoadImage(FileData);
                Tex.filterMode = FilterMode.Point;
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

        public static void DumpAssetPaths(IEnumerable<string> assetPaths)
        {
            var root = new AssetDir();
            foreach (var path in assetPaths)
            {
                var segments = path.Split('/');
                var n = segments.Length - 1;

                var cwd = root;

                foreach (var segment in segments.Take(n))
                {
                    if (!cwd.ContainsKey(segment))
                    {
                        cwd.Add(segment, new AssetDir());
                    }
                    cwd = (AssetDir)cwd[segment];
                }

                cwd.Add(segments[n], path);
            }

            var jason = JsonConvert.SerializeObject(root, Formatting.Indented);
            File.WriteAllText("F:/assets.json", jason);
        }
    }
}