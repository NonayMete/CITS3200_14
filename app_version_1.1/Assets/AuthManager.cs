using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    public PanelSwitch pscript;
    
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DB;
    public DependencyStatus dependencyStatus;
    public static bool is_logged = false;

    [Header("Register Variables")]
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField passwordr;
    public TMP_InputField Mobile;
    public TMP_Text warningRegisterText;
    public TMP_Dropdown Locations;
    public TMP_Dropdown relationship;
    [Header("Login Variables")]
    public TMP_Text warningLoginText;
    public TMP_Text welcome;
    public TMP_Text Location_display;
    public TMP_Text email_display;
    public TMP_Text mobile_display;
    public TMP_Text relationship_display;
    public TMP_InputField usernamel;
    public TMP_InputField passwordl;
    [Header("Edit Details Variables")]
    public TMP_Dropdown Locations_e;
    public TMP_Dropdown relationship_e;
    public TMP_InputField Mobile_e;
    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(usernamel.text, passwordl.text));
    }
    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DB = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void ResetPassButton()
    {
        string email = usernamel.text;
        if (email != "") {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled) {
            Debug.LogError("SendPasswordResetEmailAsync was canceled.");
            return;
            }
            if (task.IsFaulted) {
            Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
            return;
            }

            Debug.Log("Password reset email sent successfully.");
        });
        }
    }

    private void clearLogin()
    {
        usernamel.text = "";
        passwordl.text = "";
    }

    public void logoutButton()
    {
        is_logged=false;
        auth.SignOut();
        pscript.clear();
        pscript.Profile();
        clearLogin();
    }
    public void editDetailsButton()
    {
        StartCoroutine(editDetails(Locations_e.options[Locations_e.value].text,Mobile_e.text,relationship_e.options[relationship_e.value].text));
        StartCoroutine(LoadData());
        pscript.clear();
        pscript.Profile();
        Mobile_e.text = "";
        
    }
    public void RegisterButton()
    {
        StartCoroutine(Register(email.text,passwordr.text,username.text,Locations.options[Locations.value].text,Mobile.text,relationship.options[relationship.value].text));
    }
    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            User = LoginTask.Result;
            //Logged In
            StartCoroutine(LoadData());
            yield return new WaitForSeconds(1);
            is_logged = true;
            warningLoginText.text = "";
            pscript.clear();
            pscript.Logged();
            
        }
    }

    private IEnumerator Register(string _email, string _password, string _username, string _location, string _phone, string _relationship)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if(_phone == "")
        {
            warningRegisterText.text = "Missing Mobile";
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Poor Email Format";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //error handling
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username set
                        StartCoroutine(Database_Update(_username, _location, _phone,_email,_relationship));
                        pscript.clear();
                        pscript.Profile();
                        
                        warningRegisterText.text = "";
                        Mobile.text = "";
                        username.text = "";
                        email.text = "";
                        passwordr.text = "";
                    }
                }
            }
        }
    }
    private IEnumerator SetProfile(string _username)
    {
        UserProfile pf = new UserProfile {DisplayName = _username};
        var Profile = User.UpdateUserProfileAsync(pf);
        yield return new WaitUntil(predicate:()=>Profile.IsCompleted);
        if (Profile.Exception!=null)
        {
            Debug.LogWarning(message: $"Could not create user profile {Profile.Exception}");
        }
        else
        {
            // Profile creating successful
        }
    }
    private IEnumerator Database_Update(string _username, string _location, string _phone, string _email, string _relationship)
    {
        var DataBase = DB.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
        yield return new WaitUntil(predicate:()=>DataBase.IsCompleted);
        if (DataBase.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase2 = DB.Child("users").Child(User.UserId).Child("location").SetValueAsync(_location);
        yield return new WaitUntil(predicate:()=>DataBase2.IsCompleted);
        if (DataBase2.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase2.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase3 = DB.Child("users").Child(User.UserId).Child("phone").SetValueAsync(_phone);
        yield return new WaitUntil(predicate:()=>DataBase3.IsCompleted);
        if (DataBase3.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase3.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase4 = DB.Child("users").Child(User.UserId).Child("email").SetValueAsync(_email);
        yield return new WaitUntil(predicate:()=>DataBase4.IsCompleted);
        if (DataBase4.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase4.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase5 = DB.Child("users").Child(User.UserId).Child("relationship").SetValueAsync(_relationship);
        yield return new WaitUntil(predicate:()=>DataBase5.IsCompleted);
        if (DataBase5.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase5.Exception}");
        }
        else
        {
            // Successfull
        }

    }
    private IEnumerator LoadData()
    {
        var Data = DB.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => Data.IsCompleted);
            if (Data.Exception !=null)
            {
                Debug.LogWarning(message: $"Failed {Data.Exception}");
            }
            else
            {
                DataSnapshot snapshot = Data.Result;
                welcome.text = "Welcome " + snapshot.Child("username").Value.ToString();
                Location_display.text = "Location: "+ snapshot.Child("location").Value.ToString();
                email_display.text = "Email: "+ snapshot.Child("email").Value.ToString();
                mobile_display.text = "Phone: "+snapshot.Child("phone").Value.ToString();
                relationship_display.text = "Relationship to WACRH: " +snapshot.Child("relationship").Value.ToString();
            }
    }
    private IEnumerator editDetails(string _location, string _phone, string _relationship)
    {
        var DataBase = DB.Child("users").Child(User.UserId).Child("phone").SetValueAsync(_phone);
        yield return new WaitUntil(predicate:()=>DataBase.IsCompleted);
        if (DataBase.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase1 = DB.Child("users").Child(User.UserId).Child("location").SetValueAsync(_location);
        yield return new WaitUntil(predicate:()=>DataBase.IsCompleted);
        if (DataBase1.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase1.Exception}");
        }
        else
        {
            // Successfull
        }
        var DataBase2 = DB.Child("users").Child(User.UserId).Child("relationship").SetValueAsync(_relationship);
        yield return new WaitUntil(predicate:()=>DataBase.IsCompleted);
        if (DataBase2.Exception!=null)
        {
            Debug.LogWarning(message:$"Could not register on database{DataBase2.Exception}");
        }
        else
        {
            // Successfull
        }
    }


}
