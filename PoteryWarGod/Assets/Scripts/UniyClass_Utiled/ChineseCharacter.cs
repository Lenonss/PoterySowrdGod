using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buffs 
{
    AttackSpeed,
    MoveSpeed,
    AttackDamage,
    capacity
}
[Serializable]
public class ChineseCharacter
{
    [Tooltip("自己本身")]
    public string content;

    [Tooltip("附带的增幅Buff")]
    public Buffs _buff;
}
