using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;

public class ImageLoader : MonoBehaviour
{
    public Transform Parent;
    public GameObject initImg;
    public GameObject acceptButton;
    bool stillLoading = true;
    //int numLoads = 0;
    RawImage rawImage;
    FirebaseStorage storage;
    StorageReference storageReference;

    IEnumerator LoadImage(string MediaURL, RawImage rawImg) {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaURL);
        yield return request.SendWebRequest();
        // Error Handling (deprecated fix later)
        if (request.error != null) {
            //stillLoading = false;
        } else {
            rawImg.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }

    // Start is called before the first frame update
    async void Start()
    {
        List<GameObject> rawImgs = new List<GameObject>();
        //List<RawImage> rawImageLoop = new List<RawImage>();

        // Initialize Storage
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://testing-d04b8.appspot.com/"+StaticVar.path);
        
        rawImage = initImg.GetComponent<RawImage>();
        //StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/testing-d04b8.appspot.com/o/jake.png?alt=media&token=a0e47317-234b-4357-93af-6dfe6fa3994a"));
        // ^^ Hardcoded solution for getting image/data

        // Get image reference
        StorageReference image = storageReference.Child("0.png"); // StaticVar.path is set in staticVar.cs and changed on button press

        var task1 = image.GetDownloadUrlAsync().ContinueWithOnMainThread((Task<Uri> task) =>
        {
            //if (!task.isFaulted && !task.isCanceled) {
                StartCoroutine(LoadImage(Convert.ToString(task.Result), rawImage));
            //}// else {
            //    Debug.Log(task.Exception);
            //}
        });

        await Task.WhenAll(task1);

        for (int i = 0; i < 20; i++)
        
        //int i = 0;
        //while (stillLoading)
        {

            RawImage rawImageLoop;

            rawImgs.Add(Instantiate(initImg));
            rawImgs[i].transform.SetParent(Parent);
            rawImgs[i].transform.localScale = new Vector2(1, 1);
            //myRawImage[i] = Instantiate(img);
            rawImageLoop = (rawImgs[i].GetComponent<RawImage>());
            

            //StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/testing-d04b8.appspot.com/o/jake.png?alt=media&token=a0e47317-234b-4357-93af-6dfe6fa3994a"));
            // ^^ Hardcoded solution for getting image/data
            // Get image reference
            StorageReference thisImage = storageReference.Child(""+(i+1)+".png"); // StaticVar.path is set in staticVar.cs and changed on button press

            var task2 = thisImage.GetDownloadUrlAsync().ContinueWithOnMainThread((Task<Uri> task) =>
            {

                //Debug.Log("Here is i " + i + " and we have numLoads " + numLoads);
                if (!task.IsFaulted && !task.IsCanceled) {
                    StartCoroutine(LoadImage(Convert.ToString(task.Result), rawImageLoop));
                } else {
                //    Debug.Log(task.Exception);
                    stillLoading = false;
                }
            });

            await Task.WhenAll(task2);

            if (!stillLoading)
            {
                Destroy(rawImgs[i]);
                break;
            }
        }

        if (StaticVar.policy == true)
        {
            acceptButton.transform.SetParent(Parent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
