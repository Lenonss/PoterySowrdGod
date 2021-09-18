using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPotery", menuName = "Game/PoterySC", order = 1)]
public class PoterySC : ScriptableObject
{
    [Tooltip("诗词序号")]
    public int PoteryId;

    [Tooltip("诗词名")]
    public String PoteryName;
    
    [Tooltip("句块字典：[句块序号 : 字块数组]")]
    public IntListStringDictionary WordsDic = new IntListStringDictionary();

    [Tooltip("字块字典：存放着整首诗用到的字(不重复的)")]
    public List<String> PoteryWordsList = new List<string>();
    
    [ContextMenu("生成字块字典")]
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

    [ContextMenu("清空字块字典")]
    public void ClearPoteryWordsList()
    {
        PoteryWordsList.Clear();
    }
}
