using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class policyAccept : MonoBehaviour
{
    private IEnumerator addPolicy(string _policy)
    {
        string alreadyRead = "";

        var Data = AuthManager.DB.Child("users").Child(AuthManager.User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => Data.IsCompleted);
        if (Data.Exception !=null)
        {
            Debug.LogWarning(message: $"Failed {Data.Exception}");
        }
        else
        {
            DataSnapshot snapshot = Data.Result;
            alreadyRead = snapshot.Child("policy_read").Value.ToString();
        }
        if (alreadyRead.Contains(_policy)) {
            // Do nothing as the policy has already been read
        } else {
            var DataBase = AuthManager.DB.Child("users").Child(AuthManager.User.UserId).Child("policy_read").SetValueAsync(alreadyRead+_policy);
            yield return new WaitUntil(predicate:()=>DataBase.IsCompleted);
            if (DataBase.Exception!=null)
            {
                Debug.LogWarning(message:$"Could not register on database{DataBase.Exception}");
            }
            else
            {
                // Successfull
            }
        }
    }

    public void acceptPolicy()
    {
        Debug.Log("Add this policy as one of the accepted ones on firebase... " + StaticVar.policyName);
        StartCoroutine(addPolicy(("["+StaticVar.policyName+"],")));
    }
}
