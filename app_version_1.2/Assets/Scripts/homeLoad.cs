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

    //text reading
    public TextAsset dictionaryTextFile;
    private string theWholeFileAsOneLongString;
    public static List<folder> structure;
    public int numFolders;


    void Start()
    {
        //text reading
        theWholeFileAsOneLongString = dictionaryTextFile.text;
        List<folder> structure = new List<folder>();
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
                structure[numFolders].numSubFolders = numSubs;
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

        // write to screen
        List<GameObject> textList = new List<GameObject>();

        int numFiles = structure[8].numSubFolders;

        for (int i = 0; i < numFiles; i++)
        {
            textList.Add(Instantiate(initText));
            TextMeshProUGUI mText = textList[i].GetComponent<TextMeshProUGUI>();
            mText.text = structure[8].subFolders[i];
            textList[i].transform.SetParent(Parent);
            textList[i].transform.localScale = new Vector2(1, 1);
        }
    }
}
