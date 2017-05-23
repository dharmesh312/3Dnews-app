using UnityEngine;
using System.Collections;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections.Generic;


public class fbsharing : MonoBehaviour
{
    public Text userIdText;

    // Use this for initialization
    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();

        }

    }

    public void LOGIN()
    {
        // FB.Init();
        FB.LogInWithReadPermissions(callback: onlogin);
    }


    private void onlogin(ILoginResult result)

    {
        if (FB.IsLoggedIn)
        {
            AccessToken token = AccessToken.CurrentAccessToken;
            //userIdText.text = token.UserId;
        }
        else
        {
            Debug.Log("canceled login");

        }
    }



    public void Share()
    {
        FB.ShareLink(
            contentTitle: "news by app made ",
            contentURL: new System.Uri("http://youtube.com"),
            contentDescription: "hello all",
            callback: onshare
            );
    }

    private void onshare(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("sharelink error: " + result.Error);
        }
        else if (!string.IsNullOrEmpty(result.PostId))
        {
            Debug.Log(result.PostId);
        }
        else
            Debug.Log("share succeed");

    }




}
