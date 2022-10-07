using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class folder
{
    public int numSubFolders;
    public string path;
    public List<string> subFolders;
}

public class homeLoad : MonoBehaviour
{
    public Transform Parent;
    public GameObject initText;
    public GameObject homeText;

    //text reading
    public TextAsset dictionaryTextFile;
    private string theWholeFileAsOneLongString;
    public static List<folder> structure = new List<folder>();
    public int numFolders;
    List<string> locationsList = new List<string>();


    void Start()
    {
        //text reading
        theWholeFileAsOneLongString = dictionaryTextFile.text;
        List<string> subFolder = new List<string>();
        string[] eachline = theWholeFileAsOneLongString.Split("\n");
        int depth = 0;
        int numFolders = 0;
        int numSubs = 0;

        for(int i = eachline.Length-1; i >= 0; i--)
        {
            string[] splitline = eachline[i].Split("-");
            depth = splitline.Length - 1;
            //Debug.Log("Here is line " + splitline[0] + " and depth is " + depth);
            if (depth == 0)
            {   
                structure.Add(new folder());
                structure[numFolders].subFolders = subFolder;
                structure[numFolders].path = splitline[0];
                locationsList.Add(splitline[0]);
                //Debug.Log("locations - " + locationsList);
                structure[numFolders].numSubFolders = numSubs;
                numSubs = 0;
                subFolder = new List<string>();
                numFolders++;
            } else {
                subFolder.Add(splitline[depth]);
                numSubs++;
            }
        }

        /*foreach (var item in structure)
        {
            Debug.Log("Folder name: " + item.path);
            foreach (var sub in item.subFolders)
            {
                Debug.Log("SubFolder name: " + sub);
            }
        }*/
    }

    public void homeFilesLoad()
    {
        // finding index
        for(int i = Parent.childCount - 1; i >= 0; i--)
        {
            Destroy(Parent.GetChild(i).gameObject);
        }

        int index = locationsList.FindIndex(a => a.Contains(StaticVar.home));
        Debug.Log("index - " + index);

        if (index == 0) {
            TextMeshProUGUI hText = homeText.GetComponent<TextMeshProUGUI>();
            hText.text = "Not Logged In";

            GameObject logInText = Instantiate(initText);
            TextMeshProUGUI mText = logInText.GetComponent<TextMeshProUGUI>();
            mText.text = "Please Log In to access files";
            logInText.transform.SetParent(Parent);
            logInText.transform.localScale = new Vector2(1, 1);
        } else {
            // write to screen
            TextMeshProUGUI hText = homeText.GetComponent<TextMeshProUGUI>();
            hText.text = StaticVar.home;

            List<GameObject> textList = new List<GameObject>();

            int numFiles = structure[index].numSubFolders;

            for (int i = 0; i < numFiles; i++)
            {
                textList.Add(Instantiate(initText));
                TextMeshProUGUI mText = textList[i].GetComponent<TextMeshProUGUI>();
                mText.text = structure[index].subFolders[i];
                textList[i].transform.SetParent(Parent);
                textList[i].transform.localScale = new Vector2(1, 1);
            }
        }
    }
}
