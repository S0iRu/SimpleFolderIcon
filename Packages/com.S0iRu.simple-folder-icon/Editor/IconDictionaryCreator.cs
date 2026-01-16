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

            var appDirPath = Application.dataPath.Replace("Assets","Packages");
            var dir = new DirectoryInfo(appDirPath + "/" + AssetsPath);
            // サブディレクトリも含めて検索
            FileInfo[] info = dir.GetFiles("*.png", SearchOption.AllDirectories);
            foreach(FileInfo f in info)
            {
                // サブディレクトリからの相対パスを取得
                var relativePath = f.FullName.Substring(dir.FullName.Length + 1).Replace("\\", "/");
                var assetPath = $"Packages/{AssetsPath}/{relativePath}";
                var texture = (Texture)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
                var key = Path.GetFileNameWithoutExtension(f.Name);
                
                // 重複キーを避けるため、既に存在する場合はスキップ
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, texture);
                }
            }

            FileInfo[] infoSO = dir.GetFiles("*.asset");
            foreach (FileInfo f in infoSO) 
            {
                var folderIconSO = (FolderIconSO)AssetDatabase.LoadAssetAtPath($"Packages/{AssetsPath}/{f.Name}", typeof(FolderIconSO));

                if (folderIconSO != null) 
                {
                    var texture = (Texture)folderIconSO.icon;

                    foreach (string folderName in folderIconSO.folderNames) 
                    {
                        if (folderName != null) 
                        {
                            // dictionary.TryAdd(folderName, texture);
                            dictionary.Add(folderName, texture);
                        }
                    }
                }
            }
            
            IconDictionary = dictionary;
        }
    }
}
