using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TesterScript : MonoBehaviour
{

    public TextMeshProUGUI t;
    public void SayHello()
    {
        t.text = "";
        t.gameObject.SetActive(false);
        
        t.color = Color.red;
        t.text = "Hi motherfucker this is working";
        Invoke("DelayTextAppear", 0.3f);

    }

    public void DelayTextAppear()
    {
     
        t.gameObject.SetActive(true);

    }


    public void TestSceneTrans()
    {
        SceneManager.LoadScene("Tester");
    }
}
