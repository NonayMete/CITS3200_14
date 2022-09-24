using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testAdd : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform Parent;
    public GameObject img;

    void Start()
    {
        List<GameObject> rawImgs = new List<GameObject>();
        //GameObject[] myRawImage = new GameObject[8];

        for (int i = 0; i < 8; i++)
        {
            rawImgs.Add(Instantiate(img));
            //myRawImage[i] = Instantiate(img);
            rawImgs[i].transform.SetParent(Parent);
            rawImgs[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
