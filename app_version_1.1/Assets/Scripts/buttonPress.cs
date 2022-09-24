using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonPress : MonoBehaviour
{
    
    public void LoadSwitch()
    {
        StaticVar.path = gameObject.transform.name;
        SceneManager.LoadScene(1);
    }
}
