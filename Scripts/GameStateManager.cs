using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameStateManager : MonoBehaviour  {

    public int numeroPileComplete;
    public GameObject VittoriaText;

    bool isRunningOnAndroid;

	void Start ()
    {
        numeroPileComplete = 0;
        isRunningOnAndroid = false;

        if (!VittoriaText)
            VittoriaText = GameObject.FindGameObjectWithTag("vittoriaText");

        if (Application.platform == RuntimePlatform.Android)        //Controllo se l'app sta runnando su Android
            isRunningOnAndroid = true;
    }

    void Update()
    {
        if ( (Input.GetKeyUp(KeyCode.Escape)) && (isRunningOnAndroid == true) )
        {
            Application.Quit();
            return;
        }        
    }    

    public void PilaFinalStatus(int num)    //WIP. È un sistema funzionante e funzionale allo scopo, ma poco elegante
    {
        numeroPileComplete = numeroPileComplete + num;
        if (numeroPileComplete == 4)
            VittoriaText.SetActive(true);
    }
}
