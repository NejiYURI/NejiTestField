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

    public UnityEvent<string, bool> SetUIVisibility;

    public UnityEvent<string, float, float> SetUIImageFillAmount;

    public UnityEvent PlayerTurn;

    public UnityEvent PlayerTurnOver;

    public UnityEvent EnemyActionOver;

}
