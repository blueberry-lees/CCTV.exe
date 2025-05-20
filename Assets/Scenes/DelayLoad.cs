using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayLoad : MonoBehaviour
{
    public GameObject[] delayLoadObj;
    public float activationTime = 1f;
    public float deactivationTime = 1f;


    private void OnEnable()
    {
        SetGameObjectActiveFalse();
    }

    private void Start()
    {
       
       DelayActivation(activationTime);
       
    }


    public void DelayActivation(float s)
    {
        Invoke("SetGameObjectActive", s);
    }


    public void DelayDeactivation(float s)
    {
        Invoke("SetGameObjectActiveFalse", s);
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


    //private void Start()
    //{
    //    StartCoroutine(WaitForSomeTime(1f));

    //}

    //private IEnumerator WaitForSomeTime(float s)
    //{
    //    yield return new WaitForSeconds(s);
    //    options.gameObject.SetActive(true);

    //}

}
