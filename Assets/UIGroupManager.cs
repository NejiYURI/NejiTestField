using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIData
{
    public string ObjId;
    public GameObject Obj;
}

public class UIGroupManager : MonoBehaviour
{

    public List<UIData> UIList;

    private Dictionary<string, GameObject> UIDictionary;
    private void Start()
    {
        UIDictionary = new Dictionary<string, GameObject>();
        foreach (var item in UIList)
        {
            if (!string.IsNullOrEmpty(item.ObjId) && !UIDictionary.ContainsKey(item.ObjId))
            {
                UIDictionary.Add(item.ObjId, item.Obj);
            }
        }
        if (GameEventManager.gameEvent != null)
        {
            GameEventManager.gameEvent.SetUIVisibility.AddListener(SetUIVisibility);
            GameEventManager.gameEvent.SetUIImageFillAmount.AddListener(SetUIImageFillAmount);
        }
    }

    void SetUIVisibility(string i_ObjId, bool i_Show)
    {
        if (UIDictionary.ContainsKey(i_ObjId))
        {
            UIDictionary[i_ObjId].SetActive(i_Show);
        }
    }

    void SetUIImageFillAmount(string i_ObjId, float i_value,float i_Max)
    {
        if (UIDictionary.ContainsKey(i_ObjId) && UIDictionary[i_ObjId].GetComponent<Image>()!=null)
        {
            UIDictionary[i_ObjId].GetComponent<Image>().fillAmount = i_value / i_Max;
        }
    }
}
