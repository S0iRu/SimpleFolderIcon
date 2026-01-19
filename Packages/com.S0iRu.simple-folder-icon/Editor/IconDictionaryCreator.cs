using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace SimpleFolderIcon.Editor
{
    public class IconDictionaryCreator : AssetPostprocessor
    {
        private const string AssetsPath = "com.S0iRu.simple-folder-icon/Icons";
        internal static Dictionary<string, Texture> IconDictionary;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!ContainsIconAsset(importedAssets) &&
                !ContainsIconAsset(deletedAssets) &&
                !ContainsIconAsset(movedAssets) &&
                !ContainsIconAsset(movedFromAssetPaths))
            {
                return;
            }

            BuildDictionary();
        }

        private static bool ContainsIconAsset(string[] assets)
        {
            foreach (string str in assets)
            {
                var dirPath = ReplaceSeparatorChar(Path.GetDirectoryName(str));
                // サブディレクトリ（Customフォルダ等）も検知
                if (dirPath.StartsWith("Packages/" + AssetsPath))
                {
                    return true;
                }
            }
            return false;
        }

        private static string ReplaceSeparatorChar(string path)
        {
            return path.Replace("\\", "/");
        }

        internal static void BuildDictionary()
        {
            // 大文字小文字を区別しない辞書
            var dictionary = new Dictionary<string, Texture>(System.StringComparer.OrdinalIgnoreCase);

            var packagePath = $"Packages/{AssetsPath}";
            
            // AssetDatabaseを使用してパッケージ内のテクスチャを検索
            // これによりVPM/UPMでインストールされた場合でも正しく動作する
            var textureGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { packagePath });
            foreach (var guid in textureGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                if (texture != null)
                {
                    var key = Path.GetFileNameWithoutExtension(assetPath);
                    // 重複キーを避けるため、既に存在する場合はスキップ
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, texture);
                    }
                }
            }

            // ScriptableObjectを検索
            var soGuids = AssetDatabase.FindAssets("t:FolderIconSO", new[] { packagePath });
            foreach (var guid in soGuids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var folderIconSO = AssetDatabase.LoadAssetAtPath<FolderIconSO>(assetPath);

                if (folderIconSO != null && folderIconSO.icon != null) 
                {
                    var texture = (Texture)folderIconSO.icon;

                    foreach (string folderName in folderIconSO.folderNames) 
                    {
                        if (!string.IsNullOrEmpty(folderName) && !dictionary.ContainsKey(folderName)) 
                        {
                            dictionary.Add(folderName, texture);
                        }
                    }
                }
            }
            
            IconDictionary = dictionary;
        }
    }
}
