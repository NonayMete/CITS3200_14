using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : MonoBehaviour
{
    public GameObject[] Panels;
    public GameObject Navibar;

    public void clear()
    {
        for (int i = 0 ; i < Panels.Length;i++){
            Panels[i].SetActive(false);
        }
    }
    public void Home()
    {
        clear();
        Panels[0].SetActive(true);
    }
    public void Search()
    {
        clear();
        Panels[1].SetActive(true);
    }
    public void Profile()
    {
        clear();
        Navibar.SetActive(true);
        if (AuthManager.is_logged == false)
        {
            Panels[2].SetActive(true);
        }
        else
        {
            Panels[6].SetActive(true);
        }
        
    }
    public void Chat()
    {
        clear();
        Panels[3].SetActive(true);
    }
    public void Setting()
    {
        clear();
        Panels[4].SetActive(true);
    }
    public void Createacct()
    {
        clear();
        Navibar.SetActive(false);
        Panels[5].SetActive(true);
    }
    public void Logged()
    {
        clear();
        Panels[6].SetActive(true);
    }

    
}
