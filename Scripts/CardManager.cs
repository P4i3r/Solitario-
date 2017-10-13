using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{

    public struct myCard
    {
        public bool isRevealed;
        public int number;
        public char suit;

        public myCard(bool revealed, int num, char sut)
        {
            isRevealed = revealed;
            number = num;
            suit = sut;
        }
    }

    public Transform targetTransformParent;     //Parent a cui tornare se lo spostamento è errato
    Transform lastTransformParent;              //Parent usato per comparazione per vedere se si è cambiato lista
    string thisCardName;                        //Nome del gO assegnato dal generatore di carte
    public myCard thisCard = new myCard();      //struct della mia carta
    //Animator animator;
    PilaManager pilaScript;
    PilaFinalManager pilaFinalScript;
    MazzoManager mazzoScript;

    // ---<[ GRAFICA]>---
    Image largeSuitImage;                       //seme centrale
    Image smallSuitImage;                       //seme in alto a destra
    Image numberImage;                          //numero
    Image cardImage;                            //fondo/retro carta
    public Sprite spriteFronte;                 //Il mio fronte
    public Sprite[] suitsArray;                 //Array di possibili semi
    public Sprite[] figuresArray;               //Array delle possibili figure
    public Sprite[] numberArray;                //Array di possibili numeri

    public GameObject number;                   //Linkati nell'inspector
    public GameObject largeSuit;                //O in caso alternativo in start DA FARE
    public GameObject smallSuit;

    public void OnPointerClick(PointerEventData eventData)
    {   
        //TODO: CODICE PER IL DOUBLE CLICK
        if (!thisCard.isRevealed)               //Non dovrebbe mai essere il caso
            RevealCard();
    }

    //Giro la carta e imposto le sprites
    public void RevealCard()
    {
        thisCardName = transform.name;          //Prendo il nome
        thisCard.isRevealed = false;            //Inizializzo come carta coperta, poi si scopre automaticamente.
        thisCard.suit = thisCardName[0];        //Determino il seme (dal seme posso determinare il colore)
        thisCard.number = Convert.ToInt32(thisCardName.Remove(0, 1));   //Determino il numero togliendo la prima leggera dalla stringa del nome

        //animator = GetComponent<Animator>();    //Uso GetComponent qui al posto che in start per evitare che 52 carte chiamino GetComponent all'avvio
        //animator.Play("ruotaCarta");

        //CAMBIO L'ASPETTO DELLA CARTA
        cardImage = gameObject.GetComponent<Image>();
        cardImage.sprite = spriteFronte;
        //Acquisisco l'Image renderer delle 3 porzioni della carta
        numberImage = number.GetComponent<Image>();
        smallSuitImage = smallSuit.GetComponent<Image>();
        largeSuitImage = largeSuit.GetComponent<Image>();
        //Attivo il component Image. (originariamente disattivato per evitare che si vedano i quadrati neri e bianchi)
        numberImage.enabled = true;
        smallSuitImage.enabled = true;
        largeSuitImage.enabled = true;

        //Imposto il numero
        numberImage.sprite = numberArray[thisCard.number - 1];  //-1 perchè ho nominato le carte da 1-13

        //CASI CARTE NERE
        if (thisCard.suit == 'c' || thisCard.suit == 's')
        {
            //Imposto la figura grande, il colore lo so grazie al seme
            switch (thisCard.number)
            {
                case 11:                //Jack
                    largeSuitImage.sprite = figuresArray[0];
                    break;
                case 12:                //Donna
                    largeSuitImage.sprite = figuresArray[1];
                    break;
                case 13:                //Re
                    largeSuitImage.sprite = figuresArray[2];
                    break;
                default:                //Se non è una figura uso la sprite del seme
                    if (thisCard.suit == 'c') { largeSuitImage.sprite = suitsArray[0]; }
                    else { largeSuitImage.sprite = suitsArray[3]; }
                    break;
            }
            //Imposto la figura piccola
            if (thisCard.suit == 'c')   //clubs
            {
                smallSuitImage.sprite = suitsArray[0];
            }
            else                        //spades
            {
                smallSuitImage.sprite = suitsArray[3];
            }
        }
        //CASI CARTE ROSSE
        if (thisCard.suit == 'h' || thisCard.suit == 'd')
        {
            //Imposto la figura grande, il colore lo so grazie al seme
            switch (thisCard.number)
            {
                case 11:                //Jack
                    largeSuitImage.sprite = figuresArray[3];
                    break;
                case 12:                //Donna
                    largeSuitImage.sprite = figuresArray[4];
                    break;
                case 13:                //Re
                    largeSuitImage.sprite = figuresArray[5];
                    break;
                default:                //Se non è una figura uso la sprite del seme
                    if (thisCard.suit == 'h') { largeSuitImage.sprite = suitsArray[2]; }
                    else { largeSuitImage.sprite = suitsArray[1]; }
                    break;
            }
            //Imposto la figura piccola
            if (thisCard.suit == 'h')   //clubs
            {
                smallSuitImage.sprite = suitsArray[2];
            }
            else                        //spades
            {
                smallSuitImage.sprite = suitsArray[1];
            }

        }

        numberImage.preserveAspect = true;
        smallSuitImage.preserveAspect = true;
        largeSuitImage.preserveAspect = true;
        cardImage.raycastTarget = true;     //Nel caso in cui la carta si trovasse sotto una pila era stata generata con il raycast target disabilitato
        thisCard.isRevealed = true;
    }

    //__________ MOVIMENTO DELLA CARTA __________

    //Salvo la posizione di partenza in modo da poter resettare il movimento se il giocatore compie un'azione non permessa
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardImage = gameObject.GetComponent<Image>();
        cardImage.raycastTarget = false;                //Disattivo il raycastTarget della carta in modo che il mio raycast possa colpire la pila

        targetTransformParent = transform.parent;       //Salvo il mio parent di partenza, in modo da poterci tornare in caso di spostamento non permesso
        lastTransformParent = transform.parent;

        //TODO : Devo verificare se sono la prima carta della pila o se faccio parte di un blocco ordinato

        transform.SetParent(transform.parent.parent);   //Rimuovo la mia carta dalla pila nell'inspector (mi permette di spostarla)
    }

    //Muovo la carta
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.raycastTarget = true;     //Riabilito il raycast della carta   
        transform.SetParent(targetTransformParent); //Sposto la carta

        //Nel caso in cui cui questa carta sia stata spostata via permanentemente da una Pila dico a quella pila di ricontrollare
        if (this.transform.parent != lastTransformParent)           //la sua lista degli elementi e le sue richieste
        {
            if (lastTransformParent.tag == "pila")
            {
                pilaScript = lastTransformParent.GetComponent<PilaManager>();
                pilaScript.RefreshCardList();
                pilaScript.GetColorAndNumber();

                Debug.Log("Rimuovo " + transform.name + " dalla lista della pila.");
                
                //Se la pila ha più di 0 elementi vado a rivelare l'ultima carta
                if (pilaScript.listaPila.Count > 0)
                    pilaScript.RevealLastCard();
            }
            else if (lastTransformParent.tag == "pilaFinal")
            {
                pilaFinalScript = lastTransformParent.GetComponent<PilaFinalManager>();
                pilaFinalScript.RemoveCardFromList();

                Debug.Log("Rimuovo " + transform.name + " dalla lista della pila Final.");
            }
            else if (lastTransformParent.tag == "pilaScarti")
            {
                mazzoScript = GameObject.FindGameObjectWithTag("pilaMazzo").GetComponent<MazzoManager>();
                mazzoScript.RemoveCardFromList();

                Debug.Log("Rimuovo " + transform.name + " dalla lista del mazzo.");

            }
        }
    }
}
