using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;

public class ImageLoader : MonoBehaviour
{
    RawImage rawImage;
    FirebaseStorage storage;
    StorageReference storageReference;

    IEnumerator LoadImage(string MediaURL) {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaURL);
        yield return request.SendWebRequest();
        // Error Handling (deprecated fix later)
        //if (request.isNetworkError || request.isHttpError) {
        //    Debug.Log(request.error);
        //} else {
            rawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        rawImage = gameObject.GetComponent<RawImage>();
        //StartCoroutine(LoadImage("https://firebasestorage.googleapis.com/v0/b/testing-d04b8.appspot.com/o/jake.png?alt=media&token=a0e47317-234b-4357-93af-6dfe6fa3994a"));
        // ^^ Hardcoded solution for getting image/data

        // Initialize Storage
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://testing-d04b8.appspot.com");

        // Get image reference
        StorageReference image = storageReference.Child(StaticVar.path); // StaticVar.path is set in staticVar.cs and changed on button press

        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            //if (!task.isFaulted && !task.isCanceled) {
                StartCoroutine(LoadImage(Convert.ToString(task.Result)));
            //} else {
            //    Debug.Log(task.Exception);
            //}
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
