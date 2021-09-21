using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CSharpExtensionTool.ListExtension;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewPotery", menuName = "Game/PoterySC", order = 1),Serializable]
public class PoterySC : ScriptableObject
{
    [Header("诗词基本属性")]
    [Tooltip("诗词序号")]
    public int PoteryId;

    [Tooltip("诗词名")]
    public String PoteryName;
    
    [Tooltip("句块字典：[句块序号 : 字块数组]")]
    public IntListStringDictionary WordsDic = new IntListStringDictionary();

    [Tooltip("字块字典：存放着整首诗用到的字(不重复的)")]
    public List<String> PoteryWordsList = new List<string>();
    

    /// <summary>
    /// 诗词内容的字符串，去掉了标点符号
    /// </summary>
    [Tooltip("诗词内容的字符串"),Multiline(10)] public string _poteryWords = "";
    
    [Header("效果相关")]
    [Tooltip("buff持续时间")] public float buffTime;
    
    [Header("生成相关")]
    [Tooltip("诗词内容，解析内容生成数据"),Multiline(10)]
    public string bornContent;
    
    public void BornPoteryWordsList()
    {
        if (WordsDic.Count==0)
        {
            Debug.LogError("句块字典不能为空");
            return;
        }
        
        //添加
        foreach (var words in WordsDic)
        {
            foreach (var word in words.Value)
            {
                if (PoteryWordsList.Contains(word))
                {
                    continue;
                }
                PoteryWordsList.Add(word);
            }
        }
    }
    [ContextMenu("根据bornContent初始化")]
    public void InitSelfByContent()
    {
        if (String.IsNullOrEmpty(bornContent)) return;
        _poteryWords = "";
        WordsDic.Clear();
        PoteryWordsList.Clear();
        
        int index = 0;
        List<string> everywords = new List<string>();
        
        foreach (var word in bornContent)
        {
            if (Char.IsPunctuation(word))
            {
                //值处理
                var addValue = everywords;
                everywords = new List<string>();
                everywords.Clear();
                
                //功能实现
                WordsDic.Add(index,addValue);
                index++;
                continue;
            }

            if (word>127)
            {
                everywords.Add(word.ToString());
                _poteryWords += word;
            }
        }
        
        BornPoteryWordsList();
        InitBuffTime();
    }
    
    public void InitbornContent()
    {
        if (WordsDic.Count==0)
        {
            return;
        }

        _poteryWords = "";
        foreach (var words in WordsDic)
        {
            foreach (var word in words.Value)
            {
                _poteryWords += word;
            }
        }
    }

    [ContextMenu("清空字块字典")]
    public void ClearPoteryWordsList()
    {
        PoteryWordsList.Clear();
    }
    [ContextMenu("清空句块字典")]
    public void ClearWordDicList()
    {
        WordsDic.Clear();
    }

    public void InitBuffTime()
    {
        buffTime = _poteryWords.Length * 0.4f;
    }
}
