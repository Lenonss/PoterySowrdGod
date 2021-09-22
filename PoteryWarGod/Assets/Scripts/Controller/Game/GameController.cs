using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using URandom = UnityEngine.Random;
public class GameController : Singleton<GameController>
{
    public struct FinishedArg
    {
        public bool isFinished;
        public PoterySC finishedPotery;
    }

    [SerializeField]
    public ShowPCharanel panel;
    //诗词字符串数组
    private List<PoterySC> _gamingPoterys = new List<PoterySC>();
    //当前显示的字块
    private List<string> _curWords;
    //截止当前的输入字符
    private string _curChosedStr = "";
    //玩家选中字块了?
    private bool _chosed = false;
    //是否需要重新循环
    private bool _isRestart = false;
    //是否完成输入
    private FinishedArg _finishedArg = new FinishedArg() {isFinished = false};

    private void OnEnable()
    {
        GameInputManager.OnChooseChar += OnWordChosed;
        GameInputManager.OnReStartChose += OnReStartChosed;
    }

    private void OnReStartChosed()
    {
        //重新开始游戏
        ReStartGame();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果重新开始，停止协程,重新StartGame
    }

    private void OnDisable()
    {
        GameInputManager.OnChooseChar -= OnWordChosed;
        GameInputManager.OnReStartChose -= OnReStartChosed;
    }

    /// <summary>
    /// 初始化几首诗词的内容字符串存储
    /// </summary>
    /// <param name="_poterys"></param>
    public void InitPoteryStrList(List<PoterySC> _poterys)
    {
        _gamingPoterys.Clear();

        _gamingPoterys = _poterys;
    }

    public void StartGame()
    {
        //输出四个字块
        ShowFirstWords();
        StartCoroutine(AfterChose());
    }
    /// <summary>
    /// 之后的出字函数
    /// </summary>
    /// <returns></returns>
    private IEnumerator AfterChose()
    {
        while (true)
        {
            yield return new WaitUntil(() => _chosed);
            _chosed = false;
            if (!ShowAfterWords())
            {
                if (_finishedArg.isFinished)
                {
                    Debug.Log("finished");
                    panel.Panel.SetActive(false);
                    Debug.Log("Buff Start");
                    yield return new WaitForSecondsRealtime(_finishedArg.finishedPotery.buffTime);
                    Debug.Log("Buff Over");
                    panel.Panel.SetActive(true);
                }
                break;
            }
        }
        ReStartGame();
    }

    private void ReStartGame()
    {
        StopCoroutine(AfterChose());//不管有没有，先停止再说
        //参数重置
        _curWords.Clear();
        _curChosedStr = "";
        _chosed = false;
        _finishedArg.isFinished = false;
        _finishedArg.finishedPotery = null;
        StartGame();
    }

    /// <summary>
    /// 展示之后的字符
    /// </summary>
    /// <returns></returns>
    private bool ShowAfterWords()
    {
        List<string> matchSucStrs = new List<string>();
        List<string> sortedWords = new List<string>();
        int chosedNum = _curChosedStr.Length;
        
        foreach (var pSc in _gamingPoterys)
        {
            var tempStr = pSc._poteryWords.Substring(0, chosedNum);//比较字符串
            if (_curChosedStr.Equals(tempStr))
            {
                //匹配后判断是否输入完全
                if (chosedNum==pSc._poteryWords.Length)
                {
                    _finishedArg.isFinished = true;
                    _finishedArg.finishedPotery = pSc;
                    return false;
                }
                matchSucStrs.Add(pSc._poteryWords);
            }
        }
        //有没有成功匹配的字符串(是否输入错误)
        if (matchSucStrs.Count == 0)
        {
            return false;
        }
        //获得顺序之后的字
        foreach (var str in matchSucStrs)
        {
            sortedWords.Add(str[chosedNum].ToString());//添加后一位字,由于是数组，不需要再加
        }
        AddRandomWord(panel.ShowNum-sortedWords.Count,sortedWords);//获得随机字
        //显示到UI
        _curWords = sortedWords.ToList();
        randomList(_curWords);//打乱数组元素
        ShowWordToUI(_curWords);
        return true;
    }

    private int record = 0;
    public void ShowFirstWords()
    {
        
        //TODO:代码需要重新设计
        var firstWords = GetFirstShowWords();
        _curWords = firstWords.ToList();//当前字块数组赋值
        randomList(_curWords);//打乱数组元素
        ShowWordToUI(_curWords);
        record++;
        Debug.Log(record);
        if (record==2)
        {
            Debug.Log("in");
        }
    }

    /// <summary>
    /// 把字块显示到视图层
    /// </summary>
    /// <param name="words">需要显示的字块数组</param>
    public void ShowWordToUI(List<string> words)
    {
        panel.ShowCube1.transform.GetChild(0).GetComponent<Text>().text = words[0];
        panel.ShowCube1.GetComponent<Image>().color = PoteryLibraryCtrl.Instance.GetWordColorByWord(words[0]);
        panel.ShowCube2.transform.GetChild(0).GetComponent<Text>().text = words[1];
        panel.ShowCube2.GetComponent<Image>().color = PoteryLibraryCtrl.Instance.GetWordColorByWord(words[1]);
        panel.ShowCube3.transform.GetChild(0).GetComponent<Text>().text = words[2];
        panel.ShowCube3.GetComponent<Image>().color = PoteryLibraryCtrl.Instance.GetWordColorByWord(words[2]);
        panel.ShowCube4.transform.GetChild(0).GetComponent<Text>().text = words[3];
        panel.ShowCube4.GetComponent<Image>().color = PoteryLibraryCtrl.Instance.GetWordColorByWord(words[3]);
    }

    /// <summary>
    /// 获得首次显示字的数组
    /// </summary>
    /// <returns></returns>
    private List<string> GetFirstShowWords()
    {
        List<char> firstWords = new List<char>();
        List<string> result = new List<string>();
        Dictionary<char, int> tempDic = new Dictionary<char, int>();
        //获得所有的第一个字(含重复)
        foreach (var pStr in _gamingPoterys)
        {
            firstWords.Add(pStr._poteryWords[0]);
        }
        //获得所有第一个字(非重复)，并记录其出现次数
        foreach (var VARIABLE in firstWords)
        {
            if (tempDic.ContainsKey(VARIABLE))
            {
                tempDic[VARIABLE]++;
            }
            else
            {
                tempDic.Add(VARIABLE, 1);
            }
        }
        //添加诗词第一个字
        int randomWords = 0;
        foreach (var item in tempDic)
        {
            if (item.Value > 1)
                randomWords += item.Value - 1;
            result.Add(item.Key.ToString());
        }
        //添加针对于重复字的随机字
        AddRandomWord(randomWords, result);
        
        return result;
    }
    /// <summary>
    /// 从字库获取指定数量的随机字
    /// </summary>
    /// <param name="randomCount">随机字数量</param>
    /// <param name="result">结果</param>
    private void AddRandomWord(int randomCount, List<string> result)
    {
        List<int> randomList = new List<int>();//随机序号组
        while (randomList.Count<randomCount)
        {
            int randomIndex = URandom.Range(0, PoteryLibraryCtrl.Instance._CharLibraryCapacity - 1);
            if (!randomList.Contains(randomIndex))
            {
                randomList.Add(randomIndex);
            }
        }
        for (int i = 0; i < randomCount; i++)
        {
            var KeysArr = PoteryLibraryCtrl.Instance._dynamicFontDic.Keys.ToArray();
            result.Add(KeysArr[randomList[i]]);
        }
    }

    /// <summary>
    /// 选择响应事件函数
    /// </summary>
    /// <param name="arg"></param>
    private void OnWordChosed(ChooseCharArgs arg)
    {
        var word = _curWords[arg.ChooseId - 1];//ChooseId从1开始的
        _curChosedStr += word;
        Debug.Log("Input:" + word + "  " + _curChosedStr);
        _chosed = true;
    }
    /// <summary>
    /// 打乱数组元素
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    private void randomList<T>(List<T> list)
    {
        Random ran = new Random();
        int index = 0;
        T temp;
        for (int i = 0; i < list.Count; i++)
        {
            index = ran.Next(0, list.Count - 1);
            if (index!=i)
            {
                temp = list[index];
                list[index] = list[i];
                list[i] = temp;
            }
        }
    }
}
[Serializable]
public class ShowPCharanel
{
    public int ShowNum = 4;
    public GameObject ShowCube1;
    public GameObject ShowCube2;
    public GameObject ShowCube3;
    public GameObject ShowCube4;
    public GameObject Panel;
}
