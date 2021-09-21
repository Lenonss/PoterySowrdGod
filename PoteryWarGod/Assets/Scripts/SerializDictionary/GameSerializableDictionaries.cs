using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringArrayStorage : SerializableDictionary.Storage<List<String>> {}
[Serializable]
public class IntListStringDictionary : SerializableDictionary <int,List<String>,StringArrayStorage>{ }

/// <summary>
/// Id,PoterySC类字典
/// </summary>
[Serializable] public class IntPoterySCDictionary : SerializableDictionary<int, PoterySC> { }
/// <summary>
/// Char,ChineseCharacter类字典
/// </summary>
[Serializable] public class StringChineseCharDictionary : SerializableDictionary<string, ChineseCharacter> { }
/// <summary>
/// Buff,Colo颜色对照表
/// </summary>
[Serializable] public class Buff_ColorDictionary : SerializableDictionary<Buffs, Color> { }