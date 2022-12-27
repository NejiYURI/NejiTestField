using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameEventManager.gameEvent.SetUIVisibility.AddListener(SetUI);
        }
    }

    void SetUI(string i_ObjId, bool i_Show)
    {
        if (UIDictionary.ContainsKey(i_ObjId))
        {
            UIDictionary[i_ObjId].SetActive(i_Show);
        }
    }
}
