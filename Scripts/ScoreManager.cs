using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public Text puntiCounter;
    public Text mosseCounter;

    int numPunti;
    int numMosse;

    void Start()
    {
        numPunti = 0;
        numMosse = 0;
    }

    public void UpdatePunti(int nuoviPunti)
    {
        if (puntiCounter == null)
            puntiCounter = GameObject.FindGameObjectWithTag("puntiCounter").GetComponent<Text>();

        numPunti = numPunti + nuoviPunti;
        puntiCounter.text = numPunti.ToString();
    }

    public void UpdateMosse(int nuovaMossa)         //Non uso +1 in quanto devo essere in grado di fare Redo
    {
        if (mosseCounter == null)
            mosseCounter = GameObject.FindGameObjectWithTag("mosseCounter").GetComponent<Text>();

        numMosse = numMosse + nuovaMossa;
        mosseCounter.text = numMosse.ToString();    //WIP : Nel caso in cui venga chiamato da CardManager genera un NullError
    }
}
