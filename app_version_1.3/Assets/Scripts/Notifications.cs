using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;
using System;

public class Notifications : MonoBehaviour
{
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Reg Token:" + token.Token);
    }
    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new Message from:" + e.Message.From);
    }

}
