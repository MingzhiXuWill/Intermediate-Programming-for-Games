using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubModule : MonoBehaviour
{
    void Start()
    {
        TestManager.instance.enemyLoaded += LoadEventHandler;
    }

    private void OnDestroy()
    {
        TestManager.instance.enemyLoaded -= LoadEventHandler;
    }

    public void LoadEventHandler()
    {
        print(name + " detected load data event!");
        TestManager.instance.enemyLoaded -= LoadEventHandler;
    }
}
