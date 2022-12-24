using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager gameEvent;

    private void Awake()
    {
        gameEvent = this;
    }
    public UnityEvent<string> ActionSelect;

}
