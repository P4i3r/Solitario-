using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PilaManager : MonoBehaviour, IDropHandler
{
    GameObject carta;
    public List<string> listaPila;      //Lista degli elementi in questa pila
    public string colorePila;           //Colore richiesto
    public int numPila;                 //Numero richiesto

    //QUANDO UNA CARTA VIENE DROPPATA CONTROLLA CHE IL DROP SIA CORRETTO, IN CASO POSITIVO AGGIORNA LA LISTA E LA CARTA RICHIESTA
    public void OnDrop(PointerEventData eventData)
    {
        carta = eventData.pointerDrag;              //Usando event data risalgo alla carta droppata
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
            RefreshCardList();
        }
    }

    void Start()
    {
        RefreshCardList();          //Allo startup refresho la lista
        GetColorAndNumber();        //Ricavo colore e numero richiesto

        if (listaPila.Count > 0)
            RevealLastCard();
    }

    public void RefreshCardList()
    {
        listaPila.Clear();
        foreach (Transform child in transform)
        {
            listaPila.Add(child.name);
        }
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

    public void CheckCardPosition(string cardName)      //In nome della carta che mi ha chiamato
    {
        int cardIndex = listaPila.IndexOf(cardName);
        //Ho il numero della carta, ora devo controllare se ho una carta sopra, se non ce l'ho esco, se ce l'ho vedo se è rivelata, se non è esco
        //se è rivelata controllo se è in scala con quella che sto trascinando, se lo è ripeto il passaggio fino a quando trovo una carta che non
        //lo è. Ogni volta aggiunto quella carta come child.
        //Impacchetto la scala, ma poi la devo anche far spacchettare.

        Debug.Log(cardIndex);
    }

}
