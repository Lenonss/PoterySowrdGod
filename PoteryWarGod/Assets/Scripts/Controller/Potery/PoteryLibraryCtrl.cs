using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using Random = System.Random;
using Game.PathConfig;

public class PoteryLibraryCtrl : MonoBehaviour
{
    #region Params

    private static PoteryLibraryCtrl instance;

    public static PoteryLibraryCtrl Instance
    {
        get => instance;
    }

    public const string pLibPath = "ScriptObjects/Poterys/Controller/";
    [Tooltip("诗词库")] public PoteryLibrarySC _poteryLibrary;
    
    [Tooltip("动态字库字典")] public StringChineseCharDictionary _dynamicFontDic = new StringChineseCharDictionary();
    
    /// <summary>
    /// 字库容量
    /// </summary>
    public int _CharLibraryCapacity
    {
        get => _dynamicFontDic.Keys.Count;
    }
    /// <summary>
    /// 游戏中用到的诗词对应的PoterySC
    /// </summary>
    internal List<PoterySC> _gamingPoterys = new List<PoterySC>();
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        InitPoteryLibrary();//初始化诗词库资源
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            BornGameCharLib(new List<int>(){0,1,2,3});
        }
    }
    
    /// <summary>
    /// 初始并生成动态诗词库
    /// </summary> 
    public void InitPoteryLibrary()
    {
        var libSc = Resources.Load<PoteryLibrarySC>(pLibPath + "mainPoteryLibrary");
        if (libSc == null) 
        {
            AssetFileManager.CreateScriptObject<PoteryLibrarySC>("Assets/Resources/"+pLibPath,"mainPoteryLibrary");
            libSc = Resources.Load<PoteryLibrarySC>(pLibPath + "mainPoteryLibrary");
        }
        libSc.InitPages();
        _poteryLibrary = Instantiate(libSc);//获得一个副本
    }
    /// <summary>
    /// 生成游戏过程中用到的字库
    /// </summary>
    /// <param name="m_poteryIds">选择的诗词ID</param>
    public void BornGameCharLib(List<int> m_poteryIds)
    {
        if (_poteryLibrary == null) 
        {
            InitPoteryLibrary();
        }
        _gamingPoterys.Clear();//清空
        m_poteryIds = m_poteryIds.Union(m_poteryIds).ToList();
        try
        {
            //获得字库
            List<string> norCharLib = new List<string>();
            foreach (var pId in m_poteryIds)
            {
                int pageId = pId / 100;
                var poteryWords = _poteryLibrary.Pages[pageId][pId].PoteryWordsList;
            
                _gamingPoterys.Add(Instantiate(_poteryLibrary.Pages[pageId][pId]));//记录选中的诗词
                norCharLib.AddRange(poteryWords);
                norCharLib = norCharLib.Union(poteryWords).ToList<string>();//去重
            }
            //字库附加buff
            foreach (var Char in norCharLib)
            {
                int enumCapacity = (int) Buffs.capacity;
                var cc = new ChineseCharacter()
                {
                    content = Char,
                    _buff = (Buffs) UnityEngine.Random.Range(0, enumCapacity)
                };
            
                _dynamicFontDic.Add(cc.content, cc);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("生成字库失败");
            Console.WriteLine(e);
            throw;
        }

    }
    /// <summary>
    /// 根据诗词Id获取PoterySC
    /// </summary>
    /// <param name="pIds"></param>
    /// <returns></returns>
    public List<PoterySC> GetPoterySCsByPIds(List<int> pIds)
    {
        if (_poteryLibrary==null)
        {
            InitPoteryLibrary();
        }

        List<PoterySC> tempPScs = new List<PoterySC>();

        foreach (var pId in pIds)
        {
            int pageId = pId / 100;
            var pSc = Instantiate(_poteryLibrary.Pages[pageId][pId]);
            tempPScs.Add(pSc);
        }

        return tempPScs;
    }
    /// <summary>
    /// 根据字来获取对应的字块背景Buff颜色
    /// </summary>
    /// <param name="word"></param>
    /// <returns>Buff对应的背景颜色</returns>
    public Color GetWordColorByWord(string word)
    {
        //TODO:对照表是否存在对应word未进行完整处理
        var BuffColorConfig = Resources.Load<BuffColorConfig>(PathConfig.BuffColorConfigPath);
        //获得Buff类型
        var buffType = _dynamicFontDic[word]._buff;
        return BuffColorConfig._comparisonTable[buffType];
    }
}
