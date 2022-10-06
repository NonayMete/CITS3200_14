using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;

public class tryOffline : MonoBehaviour
{
    public string localFileName;
    bool stillLoading;
    public Transform Parent;
    public GameObject initImg;
    FirebaseStorage storage;
    StorageReference storageReference;
    // Start is called before the first frame update

    void localLoad()
    {
        int fileNum = 0;
        Texture2D tex = null;
        byte[] fileData;
        RawImage textureChange;
        List<GameObject> rawImgs = new List<GameObject>();
        string dirPath = Application.persistentDataPath + "/" + localFileName;
        while (File.Exists(dirPath + "/" + fileNum + ".png")) 
        {
            //Debug.Log("here load number " + dirPath + "/" + fileNum);
            fileData = File.ReadAllBytes(dirPath + "/" + fileNum + ".png");
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            // Add RawImage to scroll on image load panel

            rawImgs.Add(Instantiate(initImg));
            Debug.Log("herer set parent");
            rawImgs[fileNum].transform.SetParent(Parent);
            rawImgs[fileNum].transform.localScale = new Vector2(1, 1);
            textureChange = rawImgs[fileNum].GetComponent<RawImage>();

            textureChange.texture = tex;

            fileNum = fileNum + 1;
        }
        if (fileNum == 0) // Indicates no image was loaded
        {
            Debug.Log("Error. Check internet connection!");
        }
    }

    public async void offlineLoad()
    {
        //StaticVar.location = "Milford"; //This should be changed when pressing search button but stay constant when at home
        //StaticVar.path = StaticVar.location + "/" + gameObject.GetComponent<TextMeshProUGUI>().text;
        bool check = false;
        if (localFileName == "map") 
        {
            check = StaticVar.map;
            StaticVar.map = true;
        }
        else if (localFileName == "emergency")
        {
            check = StaticVar.emergency;
            StaticVar.map = true;
        }
        else if (localFileName == "announcement")
        {
            check = StaticVar.announcement;
            StaticVar.map = true;
        }
        
        if(Application.internetReachability == NetworkReachability.NotReachable || check == true)
        {
            localLoad();
        } 
        else // if first time tying to load
        {
            var dirPath = Application.persistentDataPath + "/" + localFileName;
            if(!Directory.Exists(dirPath)) {    // create local directory if it doesnt exist
                Directory.CreateDirectory(dirPath);
            }
            storage = FirebaseStorage.DefaultInstance;
            storageReference = storage.GetReferenceFromUrl("gs://testing-d04b8.appspot.com/"+StaticVar.location+"/"+localFileName);

            for (int i = 0; i < 20; i++)
            {
                stillLoading = false;
                // Create local filesystem URL
                string localUrl = Application.persistentDataPath + "/" + localFileName + "/" + i + ".png";
                Debug.Log("here ->" + localUrl);
                // Download to the local filesystem
                StorageReference reference = storageReference.Child(""+i+".png"); // StaticVar.path is set in staticVar.cs and changed on button press
                Debug.Log("here ->" + "gs://testing-d04b8.appspot.com/"+StaticVar.location+"/"+localFileName);
                var task2 = reference.GetFileAsync(localUrl).ContinueWithOnMainThread(task => {
                    if (!task.IsFaulted && !task.IsCanceled) {
                        Debug.Log("File downloaded.");
                        stillLoading = true;
                    } else {
                        Debug.Log("no more File downloaded.");
                    }
                });
                await Task.WhenAll(task2);
                Debug.Log("here are we still loading ->" + stillLoading);
                if (!stillLoading) // if no more images to download break out of loop
                {
                    break;
                }
            }
            
            localLoad();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
