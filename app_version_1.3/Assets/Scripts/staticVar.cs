using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVar : MonoBehaviour
{
    public static string path = "";
    public static string location = "";
    public static string home = "";

    public static bool policy = false;
    public static string policyName = "";

    public static bool map = false;             // All seperate checks if the newest version has been downloaded already
    public static bool emergency = false;
    public static bool announcement = false;
}
