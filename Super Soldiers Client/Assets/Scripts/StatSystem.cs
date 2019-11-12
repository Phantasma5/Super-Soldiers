using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    public enum StatType
    {
        Health,
        Movespeed,
        Damage,
        Fuel,
        JumpStrength
    }
    public delegate void StatUpdateFunction(StatType aStat, float aOldValue, float aNewValue);
    [System.Serializable]
    public class Stat
    {
        public StatType statType;
        public float value;
        public float maxValue;
    }
    [SerializeField] List<Stat> myStats = new List<Stat>();
    Dictionary<StatType, StatUpdateFunction> statCallbacks = new Dictionary<StatType, StatUpdateFunction>();

    public float GetValue(StatType aStatType)
    {
        foreach (Stat s in myStats)
        {
            if (s.statType == aStatType)
            {
                return s.value;
            }
        }
        return (float.MinValue);
    }
    public float GetMaxValue(StatType aStatType)
    {
        foreach (Stat s in myStats)
        {
            if (s.statType == aStatType)
            {
                return s.maxValue;
            }
        }
        return (float.MinValue);
    }

    public void SetValue(StatType aStatType, float aValue)
    {
        foreach (Stat s in myStats)
        {
            if (s.statType == aStatType)
            {
                if (s.value == aValue)
                {
                    return;
                }
                float old = aValue;
                s.value = aValue;

                if (statCallbacks.ContainsKey(s.statType))
                {
                    statCallbacks[s.statType](s.statType, old, s.value);
                }

                break;
            }
        }
    }//end SetValue()
    public void AddValue(StatType aStatType, float aValue)
    {
        Debug.Log("AddValue: " + aValue);
        foreach (Stat s in myStats)
        {
            if (s.statType == aStatType)
            {
                float old = aValue;
                s.value += aValue;

                if (statCallbacks.ContainsKey(s.statType))
                {
                    statCallbacks[s.statType](s.statType, old, s.value);
                }

                break;
            }
        }
    }//end AddValue()
    public void AddCallback(StatType aStat, StatUpdateFunction aFunc)
    {
        if (statCallbacks.ContainsKey(aStat))
        {
            statCallbacks[aStat] += aFunc;
        }
        else
        {
            statCallbacks[aStat] = aFunc;
        }
    }//end AddCallBack()
}//end class
