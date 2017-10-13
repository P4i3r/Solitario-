using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazzoManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject cartaPrefab;
    public GameObject pilaScarti;

    GameObject pila;
    PilaManager pilaScript;

    GameObject carta;
    Image cartaImage;
    CardManager scriptCarta;

    public List<string> listaMazzo;
    public List<string> listaScarti;

    private void Start()
    {
        CreateOrder();

        CreateBoard();  //IN TESTING

        CreateCard();               //Genero la prima carta che rimane coperta
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (listaMazzo.Count == 0  && listaScarti.Count > 0)    //Se ho carte nella pila degli scarti e non ho un mazzo, rimetti le carte nel mazzo.
        {
            foreach (var cartaName in listaScarti)              
            {
                listaMazzo.Add(cartaName);

                carta = pilaScarti.transform.Find(cartaName).gameObject;
                print(carta.name + " " + cartaName);
                Destroy(carta);
            }
            listaScarti.Clear();            //Svuoto la lista degli scarti
            CreateCard();
            Debug.Log("Ho rimescolato il mazzo e ho creato " + listaMazzo[0]);
        }
        else if (listaMazzo.Count == 1)     //Caso in cui ho un'unica carta. La muovo, ma non ne creo di nuove perchè non ne ho.
        {
            MoveCard();
            Debug.Log("Muovo " + listaScarti[ (listaScarti.Count-1) ] + " nella pila degli scarti");

        }else if (listaMazzo.Count > 1)     //Caso in cui ho più di 1 carta rimanente nel mazzo. 
        {
            MoveCard();
            CreateCard();
            Debug.Log("Muovo " + listaScarti[ (listaScarti.Count - 1) ] + " nella pila degli scarti. Creo " + listaMazzo[0] + ".");
        }
    }

    void CreateOrder()
    {
        //Popolo il mazzo di 52 carte
        for (int i = 1; i < 14; i++)
        {
            listaMazzo.Add("s" + i);
            listaMazzo.Add("d" + i);
            listaMazzo.Add("h" + i);
            listaMazzo.Add("c" + i);
        }

        listaMazzo.Sort((x, y) => Random.value < 0.5f ? -1 : 1);     //DA CAMBIARE: Implementare Fisher-Yates o altra alternativaa
    }

    //Creo la carta, il nome lo prendo dalla lista precostruita
    void CreateCard()
    {
        GameObject nuovaCarta = Instantiate(cartaPrefab, transform.position, transform.rotation);

        nuovaCarta.name = listaMazzo[0];
        nuovaCarta.transform.SetParent(transform);

        //Disabilito il raycast, questo mi impedisce di muovere carte coperte nelle pile. Viene riattivato con il Reveal()
        Image nuovaCartaImage;
        nuovaCartaImage = nuovaCarta.GetComponent<Image>();
        nuovaCartaImage.raycastTarget = false;

        carta = nuovaCarta;     //Mi serve per il MoveCard()
    }

    void MoveCard()
    {
        carta.transform.SetParent(pilaScarti.transform);
        CardManager scriptCarta = carta.GetComponent<CardManager>();
        scriptCarta.RevealCard();

        listaScarti.Add(listaMazzo[0]);     //Aggiungo la carta alla lista degli scarti
        listaMazzo.RemoveAt(0);             //La rimuovo da questa
    }

    public void RemoveCardFromList()        //Quando muovo la prima carta degli scarti in una pila la rimuovo dalla lista
    {
        Debug.Log("Ho rimosso " + listaScarti[listaScarti.Count - 1]);
        listaScarti.RemoveAt(listaScarti.Count - 1);            // listaScarti.Count - 1);
    }

    void CreateBoard()
    {   

        for (int i = 1; i < 8; i++)
        {
            string nomePilaTemp = "pila";
            nomePilaTemp = nomePilaTemp + i;

            Debug.Log(nomePilaTemp);
            for (int j = 0; j < i; j++)
            {
                CreateCard();
                MoveToPila(nomePilaTemp, i, j);
            }
        }
    }

    void MoveToPila(string nomePila, int primo, int secondo)
    {
        pila = GameObject.Find(nomePila);
        carta.transform.SetParent(pila.transform);

        pilaScript = pila.GetComponent<PilaManager>();
        pilaScript.RefreshCardList();
        if (secondo == primo - 1) {
            pilaScript.RevealLastCard();
        }

        listaMazzo.RemoveAt(0);             //La rimuovo da questa
    }
}
