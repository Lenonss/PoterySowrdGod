using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputManager : Singleton<GameInputManager>
{
    [Tooltip("诗词序号Index")] public List<int> _poteryIndexes = new List<int>();

    [Tooltip("字块1对应按键")] public KeyCode firstChooseKey = KeyCode.UpArrow;

    [Tooltip("字块2对应按键")] public KeyCode secondChooseKey = KeyCode.DownArrow;

    [Tooltip("字块3对应按键")] public KeyCode thirdChooseKey = KeyCode.LeftArrow;

    [Tooltip("字块4对应按键")] public KeyCode fourthChooseKey = KeyCode.RightArrow;
    
    /// <summary>
    /// 选择字块事件
    /// </summary>
    public event Action<ChooseCharArgs> OnChooseChar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EventTrigger();//检测事件是否触发
    }

    public void EventTrigger()
    {
        //触发选中字块事件
        if (Input.GetKeyDown(firstChooseKey))
        {
            ChooseCharArgs arg = new ChooseCharArgs()
            {
                ChooseId = 1
            };
            OnChooseChar?.Invoke(arg);
        }
        else if (Input.GetKeyDown(secondChooseKey))
        {
            ChooseCharArgs arg = new ChooseCharArgs()
            {
                ChooseId = 2
            };
            OnChooseChar?.Invoke(arg);
        }
        else if (Input.GetKeyDown(thirdChooseKey))
        {
            ChooseCharArgs arg = new ChooseCharArgs()
            {
                ChooseId = 3
            };
            OnChooseChar?.Invoke(arg);
        }
        else if (Input.GetKeyDown(fourthChooseKey))
        {
            ChooseCharArgs arg = new ChooseCharArgs()
            {
                ChooseId = 4
            };
            OnChooseChar?.Invoke(arg);
        }
    }

    public void PoteryIndexBtnFunc(int poteryid)
    {
        if (!_poteryIndexes.Contains(poteryid))
        {
            _poteryIndexes.Add(poteryid);
        }
    }

    public void ConfirmButFunc()
    {
        //传递参数
        PoteryLibraryCtrl.Instance.BornGameCharLib(_poteryIndexes);//生成字库
        GameController.Instance.InitPoteryStrList(
            PoteryLibraryCtrl.Instance.GetPoterySCsByPIds(_poteryIndexes));//生成诗词字符串数组
        //清空数组
        _poteryIndexes.Clear();
        //开始游戏
        GameController.Instance.StartGame();
    }
}

public class ChooseCharArgs
{
    /// <summary>
    /// 选择的字块是第几个
    /// </summary>
    public int ChooseId;
}
