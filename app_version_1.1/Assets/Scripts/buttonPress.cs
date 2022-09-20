using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonPress : MonoBehaviour
{
    
    public void LoadSwitch()
    {
        StaticVar.path = gameObject.transform.name + ".png";
        Debug.Log(StaticVar.path);
        SceneManager.LoadScene(1);
    }
}
