using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public GameObject PuntiCounter;
    public GameObject MosseCounter;
    Text puntiCounter;
    Text mosseCounter;

    int numPunti;
    int numMosse;

	void Start ()
    {
        numPunti = 0;
        numMosse = 0;

        if (!PuntiCounter)
            PuntiCounter = GameObject.FindGameObjectWithTag("puntiCounter");

        if (!mosseCounter)
            PuntiCounter = GameObject.FindGameObjectWithTag("mosseCounter");

        puntiCounter = PuntiCounter.GetComponent<Text>();
        mosseCounter = MosseCounter.GetComponent<Text>();
    }

    public void UpdatePunti(int nuoviPunti)
    {
        numPunti = numPunti + nuoviPunti;
        puntiCounter.text = numPunti.ToString();
        Debug.Log(numPunti);
    }

    public void UpdateMosse()
    {
        numMosse = numMosse + 1;
        mosseCounter.text = numMosse.ToString();
    }
}
