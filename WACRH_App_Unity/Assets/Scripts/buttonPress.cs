using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class buttonPress : MonoBehaviour
{
    
    public void getPath()
    {
        //StaticVar.location = "Milford"; //This should be changed when pressing search button but stay constant when at home
        StaticVar.path = StaticVar.location + "/" + gameObject.GetComponent<TextMeshProUGUI>().text.Replace(" ", "_");
        //Debug.Log("Path on firebase ... " + StaticVar.path);
        StaticVar.policy = false;
        //SceneManager.LoadScene(1);
    }
}
