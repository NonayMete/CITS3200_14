using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void HomeSwitch() 
    {
        SceneManager.LoadScene(0);
    }

    public void CreateacctSwitch() 
    {
        SceneManager.LoadScene(1);
    }
    public void Profile() 
    {
        SceneManager.LoadScene(2);
    }
    public void Search() 
    {
        SceneManager.LoadScene(3);
    }
    public void Chat() 
    {
        SceneManager.LoadScene(4);
    }
    public void Setting() 
    {
        SceneManager.LoadScene(5);
    }
}
