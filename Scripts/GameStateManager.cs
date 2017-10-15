using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public int numeroPileComplete;
    public GameObject VittoriaText;

	void Start ()
    {
        numeroPileComplete = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PilaFinalStatus(int num)    //WIP. Attualmente è un sistema funzionale allo scopo, ma poco elegante
    {
        numeroPileComplete = numeroPileComplete + num;
        if (numeroPileComplete == 4)
            VittoriaText.SetActive(true);
    }
}
