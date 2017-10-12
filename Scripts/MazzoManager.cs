using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MazzoManager : MonoBehaviour, IPointerClickHandler
{
    public GameObject cartaPrefab;
    public GameObject pilaScarti;

    GameObject carta;
    Image cartaImage;
    CardManager scriptCarta;

    public List<string> listaMazzo;
    public List<string> listaScarti;

    bool primoCicloMazzo;

    private void Start()
    {
        primoCicloMazzo = true;
        CreateOrder();
        CreateCard();               //Genero la prima carta che rimane coperta
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        //Verifico se ho carte nel mazzo
        if (listaMazzo.Count == 0)
        {
            listaScarti.Reverse();                          //Giro le carte nella lista
            foreach (var cartaName in listaScarti)
            {   
                listaMazzo.Insert(0, cartaName);            //Insert al posto di Add in quanto giro le carte nella lista
                carta = pilaScarti.transform.Find(cartaName).gameObject;

                cartaImage = carta.GetComponent<Image>();   //Disattivo il raycast delle carte
                cartaImage.raycastTarget = false;

                carta.transform.SetParent(transform);
            }

            listaScarti.Clear();            //Svuoto la lista degli scarti
            primoCicloMazzo = false;        //Non è più il primo ciclo del mazzo
            Debug.Log("Resetto");
            return;
        }

        MoveCard();
        
        if (primoCicloMazzo && listaMazzo.Count > 0)   //Se non è il primo ciclo non creare nuove carte 
        {
            Debug.Log("Creo");
            CreateCard();
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
        //Eseguo lo shuffle del mazzo
        //var random = new Random();
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
}
