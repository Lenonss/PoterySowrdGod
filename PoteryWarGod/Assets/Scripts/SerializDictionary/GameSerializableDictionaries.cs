using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringArrayStorage : SerializableDictionary.Storage<List<String>> {}
[Serializable]
public class IntListStringDictionary : SerializableDictionary <int,List<String>,StringArrayStorage>{ }