using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSwitch : MonoBehaviour
{
    public GameObject[] Panels;
    public GameObject Navibar;
    public GameObject Logo;
    public GameObject BackButton;
    public Transform Parent;

    void Start()
    {
        Panels[7].SetActive(false);
    }
    public void clear()
    {
        for (int i = 0 ; i < Panels.Length;i++){
            Panels[i].SetActive(false);
        }
        BackButton.SetActive(false);
    }
    public void Home()
    {
        clear();
        //BackButton.SetActive(false);
        for(int i = Parent.childCount - 1; i >= 0; i--)
        {
            Destroy(Parent.GetChild(i).gameObject);
        }
        StaticVar.location = StaticVar.home;
        Logo.SetActive(true);
        Navibar.SetActive(true);
        Panels[0].SetActive(true);
    }
    public void Search()
    {
        clear();
        StaticVar.location = "";
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
    public void password_reset_on()
    {
        Panels[7].SetActive(true);
    }
    public void password_reset_off()
    {
        Panels[7].SetActive(false);
    }
    public void update_details()
    {
        clear();
        Navibar.SetActive(false);
        Panels[8].SetActive(true);
    }
    public void load_image()
    {
        clear();
        BackButton.SetActive(true);
        Logo.SetActive(false);
        Navibar.SetActive(false);
        Panels[9].SetActive(true);
    }
    
}
