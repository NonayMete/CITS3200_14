using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class buttonPressPolicy : MonoBehaviour
{
    
    public void getPath()
    {
        //StaticVar.location = "Milford"; //This should be changed when pressing search button but stay constant when at home
        StaticVar.path = StaticVar.location + "/" + gameObject.GetComponent<TextMeshProUGUI>().text.Replace(" ", "_") + "_p_";
        StaticVar.policyName = gameObject.GetComponent<TextMeshProUGUI>().text;
        StaticVar.policy = true;
        //Debug.Log("Path on firebase ... " + StaticVar.path);
        //SceneManager.LoadScene(1);
    }
}
