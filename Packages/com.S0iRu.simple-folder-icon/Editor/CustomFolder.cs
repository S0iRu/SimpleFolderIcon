using System.IO;
using UnityEditor;
using UnityEngine;

namespace SimpleFolderIcon.Editor
{
    [InitializeOnLoad]
    public class CustomFolder
    {
        static CustomFolder()
        {
            IconDictionaryCreator.BuildDictionary();
            EditorApplication.projectWindowItemOnGUI += DrawFolderIcon;
        }

        static void DrawFolderIcon(string guid, Rect rect)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var iconDictionary = IconDictionaryCreator.IconDictionary;

            if (path == "" ||
                Event.current.type != EventType.Repaint ||
                !File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                return;
            }

            var folderName = Path.GetFileName(path);
            Texture texture = null;

            // まず完全一致を試す（大文字小文字を区別しない）
            if (iconDictionary.ContainsKey(folderName))
            {
                texture = iconDictionary[folderName];
            }
            else
            {
                // 完全一致が見つからない場合、部分一致で検索（大文字小文字を区別しない）
                var folderNameLower = folderName.ToLowerInvariant();
                foreach (var kvp in iconDictionary)
                {
                    var keyLower = kvp.Key.ToLowerInvariant();
                    if (folderNameLower.Contains(keyLower) || keyLower.Contains(folderNameLower))
                    {
                        texture = kvp.Value;
                        break;
                    }
                }
            }

            if (texture == null)
            {
                return;
            }

            Rect imageRect;

            if (rect.height > 20)
            {
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.width + 2);
            }
            else if (rect.x > 20)
            {
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.height + 2, rect.height + 2);
            }
            else
            {
                imageRect = new Rect(rect.x + 2, rect.y - 1, rect.height + 2, rect.height + 2);
            }

            GUI.DrawTexture(imageRect, texture);
        }
    }
}
