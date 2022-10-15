using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;

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
    public GameObject initTextPolicy;
    public GameObject homeText;
    FirebaseStorage storage;
    StorageReference storageReference;

    //text reading
    //public TextAsset textFile;
    public static List<folder> structure = new List<folder>();
    public int numFolders;
    List<string> locationsList = new List<string>();

    public async void startHomeFilesLoad()
    {
        locationsList.Add("no more index 0");

        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://testing-d04b8.appspot.com/directory.txt");
        string theWholeFileAsOneLongString = "";
        string localUrl = Application.persistentDataPath + "/dir.text";

        var task = storageReference.GetFileAsync(localUrl).ContinueWithOnMainThread(task => {
            if (!task.IsFaulted && !task.IsCanceled) {
                Debug.Log("File downloaded.");
            }
        });

        await Task.WhenAll(task);

        //text reading
        //File.WriteAllText(Application.persistentDataPath + "/dir.text", "poopystinky123");
        theWholeFileAsOneLongString = File.ReadAllText(Application.persistentDataPath + "/dir.text");

        Debug.Log("Whole file: "+theWholeFileAsOneLongString);
        
        List<string> subFolder = new List<string>();
        string[] eachline = theWholeFileAsOneLongString.Split("\n");
        //Debug.Log("Whole eachlin: "+eachline);
        int depth = 0;
        int numFolders = 0;
        int numSubs = 0;

        for(int i = eachline.Length-2; i >= 0; i--)
        {
            string endsWith = eachline[i].Substring(eachline[i].Length-5,4);
            string endsWith2 = eachline[i].Substring(eachline[i].Length-4,4);
            //Debug.Log("Here is endswith : -" + endsWith+ "- and it is " + String.Equals(endsWith, ".png")+"has to be true: "+ String.Equals("hello","hello"));
            if ((String.Equals(endsWith, ".png")) || (String.Equals(endsWith, ".txt")) || (String.Equals(endsWith2, ".png")) || (String.Equals(endsWith2, ".txt")))
            {
                Debug.Log("Here is last4 " + endsWith);
                continue;
            }
            string[] splitline = eachline[i].Split("/");
            depth = splitline.Length-2;
            Debug.Log("Here is line " + splitline[0] + " and depth is " + depth);
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
                subFolder.Add(splitline[1]);
                numSubs++;
            }
        }

        homeFilesLoad();

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

            // from here use index-1 as index is based upon locationslist which has an extra item
            int numFiles = structure[index-1].numSubFolders;
            Debug.Log("test numfiles - " + numFiles);

            for (int i = 0; i < numFiles; i++)
            {
                // if subfolder is map, emergency, or announcment dont show
                string filename = structure[index-1].subFolders[i];
                if (filename == "map" || filename == "announcements" || filename == "emergency") {
                    continue;
                }
                string policyCheck = filename.Substring(filename.Length-4,3);
                string policyCheck2 = filename.Substring(filename.Length-3,3);
                if ((String.Equals(policyCheck, "_p_")) || (String.Equals(policyCheck2, "_p_"))) {
                    textList.Add(Instantiate(initTextPolicy));
                    TextMeshProUGUI mText = textList[i].GetComponent<TextMeshProUGUI>();
                    mText.text = filename.Substring(0,filename.Length-3).Replace("_", " ");
                } else {
                    textList.Add(Instantiate(initText));
                    TextMeshProUGUI mText = textList[i].GetComponent<TextMeshProUGUI>();
                    mText.text = filename.Replace("_", " ");
                }
                textList[i].transform.SetParent(Parent);
                textList[i].transform.localScale = new Vector2(1, 1);
            }
        }
    }
}
