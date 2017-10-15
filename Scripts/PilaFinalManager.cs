using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PilaFinalManager : MonoBehaviour, IDropHandler
{   
    GameObject carta;

    public List<string> listaPila;      //Lista degli elementi in questa pila
    public char semePila;               //Colore richiesto
    public int numPila;                 //Numero richiesto

    public GameObject Scripts;
    GameStateManager gameStateScript;

    //QUANDO UNA CARTA VIENE DROPPATA CONTROLLA CHE IL DROP SIA CORRETTO, IN CASO POSITIVO AGGIORNA LA LISTA E LA CARTA RICHIESTA
    public void OnDrop(PointerEventData eventData)
    {
        //Usando event data risalgo alla carta droppata
        carta = eventData.pointerDrag;
        CardManager cardScript = carta.GetComponent<CardManager>();
        //Controllo seme e numero
        if (cardScript.thisCard.suit == semePila && cardScript.thisCard.number == numPila)
        {
            cardScript.targetTransformParent = this.transform;  //Dico alla carta di renderla child di questa pila
            listaPila.Add(carta.name);
            numPila = listaPila.Count + 1;

            if (numPila > 13)  //Comunico che la pila è completa
                gameStateScript.PilaFinalStatus(1);
        }
    }

    void Start()
    {   
        if (!Scripts)
            Scripts = GameObject.FindGameObjectWithTag("scripts");

        gameStateScript = Scripts.GetComponent<GameStateManager>();
        numPila = 1;    //Il numero della prima carta richiesta è sempre 1
    }

    public void RemoveCardFromList()
    {   
        if(numPila == 13)   //Se il numero di carte nella pila era 13, e ora ne sto rimuovendo una allora ho una pila completa in meno
            gameStateScript.PilaFinalStatus(-1);

        listaPila.RemoveAt(listaPila.Count - 1);
        numPila = listaPila.Count + 1;

    }
}
