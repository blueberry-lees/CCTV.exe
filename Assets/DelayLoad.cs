using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayLoad : MonoBehaviour
{
    public GameObject[] delayLoadObj;

    private void Start()
    {
        SetGameObjectActiveFalse();
        Invoke("SetGameObjectActive", 2f);
       
    }


    public void SetGameObjectActive()
    {


        foreach (GameObject obj in delayLoadObj)
        { 
            obj.SetActive(true);
        }
    }

    public void SetGameObjectActiveFalse()
    {


        foreach (GameObject obj in delayLoadObj)
        {
            obj.SetActive(false);
        }
    }


}
