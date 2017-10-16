using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PilaManager : MonoBehaviour, IDropHandler
{
    GameObject carta;
    public List<string> listaPila;  //Lista degli elementi in questa pila
    string colorePila;              //Colore richiesto
    public int numPila;             //Numero richiesto. Pubblico perchè utilizzato da CardManager per evitare conteggi errati di punti.
    bool isCardInLadder;            //True quando la carta puntata fa parte di una scala

    //QUANDO UNA CARTA VIENE DROPPATA CONTROLLA CHE IL DROP SIA CORRETTO, IN CASO POSITIVO AGGIORNA LA LISTA E LA CARTA RICHIESTA
    public void OnDrop(PointerEventData eventData)
    {
        carta = eventData.pointerDrag;              //Usando event data risalgo alla carta droppata
        Debug.Log("Ho droppato " + carta.name);
        CardManager cardScript = carta.GetComponent<CardManager>();

        bool legitDrop = false;

        //Devo verificare che il drop sia lecito
        if (cardScript.thisCard.number == numPila)  //Se il numero è corretto mi vado ad interessare del seme
        {
            if (colorePila == "undefined")
            {
                if (cardScript.thisCard.suit == 'c' || cardScript.thisCard.suit == 's')
                    colorePila = "red";
                else
                    colorePila = "black";

                legitDrop = true;
            }
            else if (colorePila == "black" && (cardScript.thisCard.suit == 'c' || cardScript.thisCard.suit == 's'))
            {
                legitDrop = true;
                colorePila = "red";
            }
            else if (colorePila == "red" && (cardScript.thisCard.suit == 'h' || cardScript.thisCard.suit == 'd'))
            {
                legitDrop = true;
                colorePila = "black";
            }
        }

        if (legitDrop)
        {
            numPila = cardScript.thisCard.number - 1;
            cardScript.targetTransformParent = this.transform;
        }
    }

    void Start()
    {
        RefreshCardList();          //Allo startup refresho la lista
        GetColorAndNumber();        //Ricavo colore e numero richiesto

        if (listaPila.Count > 0)
            RevealLastCard();

        isCardInLadder = false;     //Variabile utilizzata nel check se la carta draggata fa parte di una scala
    }

    public void RefreshCardList()
    {
        listaPila.Clear();
        foreach (Transform child in transform)
        {
            listaPila.Add(child.name);
        }
        isCardInLadder = false;
    }

    public void GetColorAndNumber()
    {
        if (listaPila.Count == 0)  //Se la lista è vuota richiedo un K di colore indefinito
        {
            colorePila = "undefined";
            numPila = 13;
        }
        else                       //In caso contrario vado a ricavare colore e numbero richiesto dal nome dell'ultimo elemento della lista
        {
            string coloreUltimaCarta = listaPila[listaPila.Count - 1].Remove(1);    //Prende il nome e rimuove tutto ciò che è oltre il primo carattere

            if (coloreUltimaCarta == "c" || coloreUltimaCarta == "s")               //Inverto perchè mi serve il valore richiesto, non quello dell'ultima
                colorePila = "red";
            else
                colorePila = "black";

            numPila = Convert.ToInt32(listaPila[listaPila.Count - 1].Remove(0, 1)) - 1;
        }
    }

    public void RevealLastCard()
    {
        carta = gameObject.transform.Find(listaPila[listaPila.Count - 1]).gameObject;           //Trovo l'ultima carta
        CardManager cardScript = carta.GetComponent<CardManager>();

        if (!cardScript.thisCard.isRevealed)            //Se quella carta non è rivelata fallo
            cardScript.RevealCard();
    }

    public bool CheckIfLadder(string cardName)          //In nome della carta che mi ha chiamato
    {
        int cardIndex = listaPila.IndexOf(cardName);
        isCardInLadder = false;

        if ((listaPila.Count > 1) && (cardIndex != (listaPila.Count - 1)))    //Se ho più di una carta nella lista e se questa carta non è l'ultima
        {
            string nameCardToCheck = listaPila[cardIndex + 1];
            GameObject cardToCheck = transform.Find(nameCardToCheck).gameObject;
            CardManager cardScript = cardToCheck.GetComponent<CardManager>();
            Debug.Log("Sto controllando " + cardToCheck);
            if (cardScript.thisCard.isRevealed)
            {
                cardToCheck.transform.SetParent(GameObject.Find(cardName).transform);
                Image cardToCheckImage = cardToCheck.GetComponent<Image>();
                cardToCheckImage.raycastTarget = false;

                CheckIfLadder(nameCardToCheck);
                isCardInLadder = true;
            }
        }
        return isCardInLadder;
    }
}
