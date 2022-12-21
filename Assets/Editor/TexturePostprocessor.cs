using UnityEditor;
using System.IO;
using UnityEngine;

namespace Editor
{
    public class TexturePostprocessor : AssetPostprocessor
    {
        private const string ExtensionVal = ".png";
        private const string PixelMapSuffix = ".map";
        private const string PixelWeightsPrefix = "weights.";

        private void OnPostprocessTexture(Texture2D i_texture)
        {
            var filename = Path.GetFileNameWithoutExtension(assetPath);
            var extension = Path.GetExtension(assetPath);
            if (!extension.Equals(ExtensionVal))
                return;
            if (filename.EndsWith(PixelMapSuffix))
                ProcessPixelMap(i_texture, filename);
            else if (filename.StartsWith(PixelWeightsPrefix))
                ProcessPixelWeights(i_texture, filename); ;

        }
        private void ProcessPixelWeights(Texture2D i_texture, string i_filename)
        {
            Debug.Log("Enter Pixel Weights");
            if (!i_filename.Contains('_'))
                return;
            var mapName = i_filename.Split('_')[0].Replace(PixelWeightsPrefix, "") + PixelMapSuffix;
            var map = FindPixelMap(mapName);
            if (map == null)
            {
                Debug.Log("Unable to find Pixelmap:" + mapName);
                return;
            }

            var skinData = i_texture.GetPixels32();
            for (var i = 0; i < skinData.Length; i++)
            {
                if (map.lookupMap.TryGetValue(skinData[i], out var pos))
                {
                    skinData[i].r = (byte)pos.x;
                    skinData[i].g = (byte)pos.y;
                    skinData[i].b = 0;
                    skinData[i].a = 255;
                }
                else
                {
                    skinData[i] = Color.clear;
                }
            }

            var path = assetPath.Replace(PixelWeightsPrefix, "");
            var skinTexture = new Texture2D(i_texture.width, i_texture.height);
            skinTexture.SetPixels32(skinData);
            File.WriteAllBytes(path, skinTexture.EncodeToPNG());
            AssetDatabase.ImportAsset(path);
        }
        private void ProcessPixelMap(Texture2D i_texture, string i_fileName)
        {
            //Debug.Log("Enter Pixel Map");
            var map = FindPixelMap(i_fileName);
            if (map == null)
            {
                map = ScriptableObject.CreateInstance<PixelMap>();
                map.name = i_fileName;
                AssetDatabase.CreateAsset(
                    map,
                    Path.Combine(Path.GetDirectoryName(assetPath), $"{i_fileName}.asset")
                    );
            }

            map.data = i_texture.GetPixels32();
            map.lookupMap.Clear();
            for (var i = 0; i < map.data.Length; i++)
            {
                if (map.data[i].a > 0)
                {
                    map.lookupMap[map.data[i]] = new Vector2Int(
                        i % i_texture.width,
                        i / i_texture.width
                        );
                }
            }
            EditorUtility.SetDirty(map);
            AssetDatabase.SaveAssets();
        }

        private static PixelMap FindPixelMap(string i_fileName)
        {
            var guids = AssetDatabase.FindAssets("t:" + nameof(PixelMap));
            foreach (var guid in guids)
            {
                var asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as PixelMap;
                if (asset != null && asset.name == i_fileName)
                    return asset;
            }

            return null;
        }
    }
}
