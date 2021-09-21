using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newPoteryLibrarySC",menuName = "Game/PoteryLibrarySC",order = 2)]
public class PoteryLibrarySC : ScriptableObject
{
    [Tooltip("分页存储诗词,List<Dic<PoteryId,PoterySC>>")]
    public List<IntPoterySCDictionary> Pages = new List<IntPoterySCDictionary>();

    [ContextMenu("初始化诗词库(Pages)")]
    public void InitPages()
    {
        //数据处理
        var PoterySCList = Resources.LoadAll<PoterySC>("ScriptObjects/Poterys");
        Pages.Clear();
        //约束情况
        if (PoterySCList == null || PoterySCList.Length == 0) 
        {
            Debug.LogError("Path ScriptObjects/Poterys has't any PoterySC");
            return;
        }
        
        IntPoterySCDictionary singleItem = new IntPoterySCDictionary();
        for (int i = 0; i < PoterySCList.Length; i++)
        {
            singleItem.Add(PoterySCList[i].PoteryId, PoterySCList[i]);
            
            if ((i + 1) % 100 == 0 || i == PoterySCList.Length - 1) 
            {
                var addItem = singleItem;
                singleItem = new IntPoterySCDictionary();
                singleItem.Clear();
                
                Pages.Add(addItem);
            }
        }
    }
}
