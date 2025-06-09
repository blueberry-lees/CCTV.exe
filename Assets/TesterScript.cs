using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TesterScript : MonoBehaviour
{

    public TextMeshProUGUI t;
    public void SayHello()
    {
        t.text = "";
        t.gameObject.SetActive(false);
        
        t.color = Color.red;
        t.text = "Hi motherfucker this is working";
        Invoke("ResetText", 0.3f);

    }

    public void ResetText()
    {
        
     
        t.gameObject.SetActive(true);

    }
}
