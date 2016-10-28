using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILog : MonoBehaviour {

    public static Transform textParent;
    public static Text textPrefab;
    public static int maxLength=40;
    static int showMax = 5;
    static Text[] textLog;


	// Use this for initialization
	void Start () {
        textParent = this.transform;
        textLog = new Text[showMax];
        textPrefab = FindObjectOfType<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public static void showLog()
    //{
    //    foreach (Text text in textLog)
    //    {
    //        Instantiate(text, textParent);
    //    }
    //}

    public static bool isFull()
    {
        for (int i = 0; i < showMax; i++)
            if (textLog[i] == null)
                return false;
        return true;
            
    }

    public static void removeText(int text)
    {
        if (text < 0 || text >= showMax)
            return;
        Destroy(textLog[text].gameObject);
        //Debug.Log("Destroying: "+textLog[text].text);
        //textLog[text] = null;
        for (int i = text; i < showMax; i++)
        {
            if (i + 1 < showMax)
                textLog[i] = textLog[i + 1];
            else
            {
                //Destroy(textLog[i]);
                textLog[i] = null;
                return;
            }
        }
    }

    public static void AddText(string text)
    {
        if(text.Length > maxLength)
        {
            AddText(text.Substring(0, maxLength));
            AddText(text.Substring(maxLength));
            return;
        }
        if (isFull())
            removeText(0);
        for(int i = 0; i < showMax; i++)
        {
            if (textLog[i] == null)
            {
                textLog[i] = (Text)Instantiate(textPrefab, textParent);
                textLog[i].text = text;
                return;
            }
        }
    }
}
