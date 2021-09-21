using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetFileManager
{
    private AssetFileManager _instance;

    public AssetFileManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new AssetFileManager();
            }

            return _instance;
        }
    }

    /// <summary>
    /// 在对应目录下创建Sc资源文件，根目录为Assets
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="fileName">文件名</param>
    /// <typeparam name="T">Sc类型</typeparam>
    public static void CreateScriptObject<T>(string path, string fileName) where T : ScriptableObject
    {
        if (String.IsNullOrEmpty(path) || String.IsNullOrEmpty(fileName)) 
        {
            Debug.LogError("path Or fileName is Null");
            return;
        }

        path = path.Last() == '/' ? path : path + '/';
        try
        {
            var level = ScriptableObject.CreateInstance<T>(); 
            AssetDatabase.CreateAsset(level, path + fileName + ".asset");
            AssetDatabase.SaveAssets(); //存储资源
            AssetDatabase.Refresh(); //刷新
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
