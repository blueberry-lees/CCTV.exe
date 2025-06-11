using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputToDisable : MonoBehaviour
{
    [Header("Disable")]

    [Tooltip("Time before 'Disabling' the obj this script is attached to")][Range (0, 20)] 
    public float dt;

     private bool isTimerUp = false;


    private void Awake()
    {
        this.gameObject.SetActive(false);

    }

    private void OnEnable()
    {
        isTimerUp = false;
        StartCoroutine(TimeBeforeTurnOff());
    }

    public IEnumerator TimeBeforeTurnOff()
    {
        yield return new WaitForSeconds(dt);
        isTimerUp = true;

    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isTimerUp || Input.GetMouseButtonDown(0) && isTimerUp)
        {
            isTimerUp = false;

            this.gameObject.SetActive(false);
       }
    }
}
