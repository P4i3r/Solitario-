using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardStateManager : MonoBehaviour {

    void Start()
    {
        //Muovo n carte da una lista A ad una lista B. Se Translate è ON la traslazione del gO è effettuata dallo script
        //Mi serve una referenza all'obj della lista di destinazione, in modo da avere le coordinate a cui spostare la carta
        //e in modo da poterla rendere child della lista
        /*
        void MoveCard(List<string> startList, List<string> targetList, int num, bool translate = false, GameObject targetPila=null)
    {
        for (int i = 0; i < num; i++)
        {
            if (translate)
            {
                try
                {
                    carta = GameObject.Find(startList[0]);
                    carta.transform.position = targetPila.transform.position;
                    carta.transform.SetParent(targetPila.transform);
                }
                catch (System.Exception)
                {
                    Debug.Log("Errore, non ho trovato: "+startList[0]);
                    throw;
                }

            }
            targetList.Add(startList[0]);
            startList.RemoveAt(0);

        }
    }
    */
    }
}
