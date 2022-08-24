using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void HomeSwitch() 
    {
        SceneManager.LoadScene(1);
    }

    public void SearchSwitch() 
    {
        SceneManager.LoadScene(2);
    }

}
