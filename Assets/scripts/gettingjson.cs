using UnityEngine;
using LitJson;
using System;
using Facebook.Unity;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class parseJSON
{
    //public string title;
    //public string id;
    public ArrayList article_author;
    public ArrayList article_title;
    public ArrayList article_image;
    public ArrayList article_url;
    public ArrayList article_description;
    public ArrayList article_time;
}
public class gettingjson : MonoBehaviour
{
    public GameObject sports;
    public GameObject business;
    public GameObject general;
    public GameObject tech;
    public GameObject section_selection;
    public GameObject MainPanel;
    public GameObject[] NewsPanel;
    private GameObject[] solid;
    private string s;
    private int childs;
    private Text Headlines;
    private Text author_name;
    private Text description;
    private Image img;
    private Image img1;
    private parseJSON json;
    private Text userIdText;
    private string url= "https://newsapi.org/v1/articles?source=the-next-web&sortBy=latest&apiKey=c733d0839a9c452fac241d0848c945d9";
    private string previous_url="";

 
    // Sample JSON for the following script has attached.
    IEnumerator Start()
    {
      
        solid = new GameObject[transform.childCount];
        childs = transform.childCount;

        for (int i = 0; i < transform.childCount; i++)
            solid[i] = transform.GetChild(i).gameObject;

        //getting the data from the www servers
        
        WWW www = new WWW(url);
        yield return www;
        if (www.error == null)
        {
            json = Processjson(www.text);
            fill_element(solid, json, childs);

            //puting the image
            for (int i = 0; i < json.article_image.Count; i++)
            {
                //putting image on canavas
                s = (string)json.article_image[i];
                GameObject x = solid[i].transform.GetChild(0).gameObject;
                GameObject imageObj = x.transform.GetChild(1).gameObject;
                img = imageObj.GetComponent<Image>();

                //putting image on tthe other canavas
                GameObject z = NewsPanel[i].transform.GetChild(0).gameObject;
                GameObject imgObj = z.transform.GetChild(3).gameObject;
                img1 = imgObj.GetComponent<Image>();

                WWW y = new WWW(s);
                yield return y;
                //gettting the image and printing it
                if (y.error == null)
                {
                    Texture2D tex;
                    tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
                    tex = y.texture;
                    Sprite image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
                    img.sprite = image; // Assuming this script is attached to an object that already has a SpriteRenderer attached
                    img1.sprite = image;
                }
                else
                    Debug.Log("ERROR: " + y.error);
            }
        }
        else
            Debug.Log("ERROR: " + www.error);

    }



    private void fill_element(GameObject[] solid, parseJSON json, int childs)
    {
        int n;

        if (childs >= json.article_author.Count)
            n = json.article_author.Count;
        else
            n = childs;

        // Debug.Log((string)json.article_author[2]);

        for (int i = 0; i < n; i++)
        {
            //putting the article titile
            string s = (string)json.article_title[i];
            GameObject x = solid[i].transform.GetChild(0).gameObject;
            GameObject z = x.transform.GetChild(0).gameObject;
            GameObject textObj = z.transform.GetChild(0).gameObject;
            Headlines = textObj.GetComponent<Text>();
            Headlines.text = s;
            //putting headlines in news panel
            GameObject y = NewsPanel[i].transform.GetChild(0).gameObject;
            Debug.Log(y);
            textObj = y.transform.GetChild(0).gameObject;
            Headlines = textObj.GetComponent<Text>();
            Headlines.text = s;
            //putting author name sin news panel
            textObj = y.transform.GetChild(1).gameObject;
            author_name = textObj.GetComponent<Text>();
            author_name.text = (string)json.article_author[i];
            //putting the description
            textObj = y.transform.GetChild(2).gameObject;
            description = textObj.GetComponent<Text>();
            description.text = (string)json.article_description[i];

        }

    }

    private parseJSON Processjson(string jsonString)
    {

        JsonData jsonvale = JsonMapper.ToObject(jsonString);
        parseJSON parsejson;
        parsejson = new parseJSON();

        parsejson.article_author = new ArrayList();
        parsejson.article_title = new ArrayList();
        parsejson.article_image = new ArrayList();
        parsejson.article_url = new ArrayList();
        parsejson.article_description = new ArrayList();
        parsejson.article_time = new ArrayList();


        for (int i = 0; i < jsonvale["articles"].Count; i++)
        {
            parsejson.article_author.Add(jsonvale["articles"][i]["author"].ToString());
            parsejson.article_title.Add(jsonvale["articles"][i]["title"].ToString());
            parsejson.article_image.Add(jsonvale["articles"][i]["urlToImage"].ToString());
            parsejson.article_url.Add(jsonvale["articles"][i]["url"].ToString());
            parsejson.article_description.Add(jsonvale["articles"][i]["description"].ToString());
            parsejson.article_time.Add(jsonvale["articles"][i]["publishedAt"].ToString());

        }

        return parsejson;
    }

    public void MenuButton()
    {
        MainPanel.SetActive(false);
        sports.SetActive(false);
        business.SetActive(false);
        tech.SetActive(false);
        general.SetActive(false);
        section_selection.SetActive(true);
    }

    public void HomeButtom()
    {
        for (int i = 0; i < 18; i++)
        {
            NewsPanel[i].SetActive(false);

        }
        MainPanel.SetActive(true);
        sports.SetActive(false);
        business.SetActive(false);
        tech.SetActive(false);
        general.SetActive(false);
        section_selection.SetActive(false);
      
    }

    public void SportsButton()
    {
        sports.SetActive(true);
        section_selection.SetActive(false);
    }

    public void BusinessButton()
    {
        business.SetActive(true);
        section_selection.SetActive(false);
    }

    public void GeneralButton()
    {
        general.SetActive(true);
        section_selection.SetActive(false);
    }

    public void TechButton()
    {
        tech.SetActive(true);
        section_selection.SetActive(false);
    }

    public void goToArticle(GameObject canvas)
    {
        int i;
        for (i = 0; i < 18; i++)
        {
            if (canvas.GetInstanceID() == NewsPanel[i].GetInstanceID())
                break;
        }
        string s = (string)json.article_url[i];
        Application.OpenURL(s);

    }

    public void OnButtonClick(GameObject canavas)
    {
        int i;
        for (i = 0; i < 18; i++)
        {
            if (canavas.GetInstanceID() == solid[i].GetInstanceID())
                break;
        }
        NewsPanel[i].SetActive(true);
        MainPanel.SetActive(false);
    }


    public void ABCnewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=abc-news-au&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void BBCnewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=bbc-news&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void BBCsportsButton()
    {
        url = "https://newsapi.org/v1/articles?source=bbc-sport&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void BusinessInsiderButton()
    {
        url = "https://newsapi.org/v1/articles?source=business-insider&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void CNNnewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=business-insider&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void MailDailynewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=daily-mail&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void ESPNnewsButton()
    {
        url = " https://newsapi.org/v1/articles?source=espn&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void FinancialTimenewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=financial-times&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

    public void FortunenewsButton()
    {
        url = " https://newsapi.org/v1/articles?source=fortune&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }

     public void GogglenewsButton()
     {
         url = "https://newsapi.org/v1/articles?source=google-news&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
         StartCoroutine(Start());
     }
     public void TOInewsButton()
     {
         url = "https://newsapi.org/v1/articles?source=the-times-of-india&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
         StartCoroutine(Start());
     }
     public void TheHindunewsButton()
     {
         url = " https://newsapi.org/v1/articles?source=the-hindu&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
         StartCoroutine(Start());
     }

    public void TechCrunchnewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=techcrunch&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    public void ARSnewsButton()
    {
        url = " https://newsapi.org/v1/articles?source=ars-technica&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    public void TechRadarnewsButton()
    {
        url = "  https://newsapi.org/v1/articles?source=techradar&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    public void RecodenewsButton()
    {
        url = "https://newsapi.org/v1/articles?source=recode&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    public void T3NnewsButton()
    {
        url = " https://newsapi.org/v1/articles?source=t3n&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    public void REDDITnewsButton()
    {
        url = " https://newsapi.org/v1/articles?source=reddit-r-all&sortBy=top&apiKey=b42c4ef1d9dd4088a4fae6dcb8a7bd41";
        StartCoroutine(Start());
    }
    

    public void FbShare(GameObject canavas)
    {
        int i;
        for (i = 0; i < 18; i++)
        {
            if (canavas.GetInstanceID() == NewsPanel[i].GetInstanceID())
                break;
        }
        Debug.Log(i);
        Debug.Log(canavas.GetInstanceID());

        string s = (string)json.article_url[i];
        string y = (string)json.article_title[i];
        FB.ShareLink(
            contentTitle: y,
            contentURL: new System.Uri(s),

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


