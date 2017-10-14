using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public int numeroPileComplete;

	void Start ()
    {
        numeroPileComplete = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PilaFinalStatus(int num)    //WIP: È un sistema estremamente rozzo, ma è anche vero che un int è sufficiente allo scopo.
    {
        numeroPileComplete = numeroPileComplete + num;
        if (numeroPileComplete == 4)
            Debug.Log("Hai vinto!");
    }
}
