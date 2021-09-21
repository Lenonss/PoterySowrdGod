using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntListStringDictionary))]
[CustomPropertyDrawer(typeof(IntPoterySCDictionary))]
[CustomPropertyDrawer(typeof(StringChineseCharDictionary))]
[CustomPropertyDrawer(typeof(Buff_ColorDictionary))]
public class GameSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}
