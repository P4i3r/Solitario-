using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    public struct MyCard
    {
        public bool isRevealed;
        public int number;
        public char suit;

        public MyCard(bool revealed, int num, char sut)
        {
            isRevealed = revealed;
            number = num;
            suit = sut;
        }
    }

    public Transform targetTransformParent;     //Parent a cui tornare se lo spostamento è errato
    Transform lastTransformParent;              //Parent usato per comparazione per vedere se si è cambiato lista
    string thisCardName;                        //Nome del gO assegnato dal generatore di carte
    public MyCard thisCard = new MyCard();      //struct della mia carta
    PilaManager pilaScript;
    PilaFinalManager pilaFinalScript;
    MazzoManager mazzoScript;
    bool cardIsInLadder;                        //Se questa carta fa parte, o meno, di una scala ordinata

    //public GameObject Scripts;                //Necessari per il calcolo del punteggio
    //ScoreManager scriptScore;

    // ---<[ GRAFICA]>---
    Animator animator;
    Image largeSuitImage;                       //seme centrale
    Image smallSuitImage;                       //seme in alto a destra
    Image numberImage;                          //numero
    Image cardImage;                            //fondo/retro carta
    public Sprite spriteFronte;                 //Il mio fronte
    public Sprite[] suitsArray;                 //Array di possibili semi
    public Sprite[] figuresArray;               //Array delle possibili figure
    public Sprite[] numberArray;                //Array di possibili numeri

    public GameObject number;                   //Linkati nell'inspector
    public GameObject largeSuit;
    public GameObject smallSuit;

    public void OnPointerClick(PointerEventData eventData)
    {
        //TODO: CODICE PER IL DOUBLE CLICK
        //In caso di double click vai a controllare il Manifesto (classe TODO nella quale listo le carte richieste da tutte le pile)
        //Se nel Manifesto trovo una pila compatibile sposto la carta la.
        //Nota: in caso una pila e una pilaFinal richiedano la stessa carta dare la precedenza a pilaFinal

        if (!thisCard.isRevealed)               //Non dovrebbe mai essere il caso
            RevealCard();
    }

    //Giro la carta e imposto le sprites
    public void RevealCard()
    {
        animator = GetComponent<Animator>();    //Uso GetComponent qui al posto che in start per evitare che 52 carte chiamino GetComponent all'avvio
        animator.SetTrigger("ruota");           //Attraverso l'animazione ruota chiamo RevealGraphics, questo mi permette di fare lo switch delle sprites esattamente quando il transform è a 90°
    }

    public void RevealGraphics()
    {
        thisCardName = transform.name;          //Nome della carta: xNN, dove x è uguale alle lettere c,h,d,s rappresentanti il formato, e NN è il numero da 1 a 13.
        thisCard.suit = thisCardName[0];        //Determino il seme guardando la prima lettera del nome, successivamente dal seme posso determinare anche il colore
        thisCard.number = Convert.ToInt32(thisCardName.Remove(0, 1));   //Determino il numero togliendo la prima leggera dalla stringa del nome     

        //CAMBIO L'ASPETTO DELLA CARTA
        cardImage = gameObject.GetComponent<Image>();
        cardImage.sprite = spriteFronte;        //Camnio il retro
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
            numberImage.color = new Color32(196, 47, 55, 255);  //Nel caso in cui la carta sia rossa cambio il colore della sprite del numero di conseguenza
        }

        numberImage.preserveAspect = true;      //Forzo l'aspect rateo corretto
        smallSuitImage.preserveAspect = true;
        largeSuitImage.preserveAspect = true;
        cardImage.raycastTarget = true;         //Nel caso in cui la carta si trovasse sotto una pila era stata generata con il raycast target disabilitato

        /*if (!Scripts)                         //Script per il punteggio
            Scripts = GameObject.FindGameObjectWithTag("scripts");

        scriptScore = Scripts.GetComponent<ScoreManager>();*/

        thisCard.isRevealed = true;             //La carta è finalmente rivelata
    }


    //__________ MOVIMENTO DELLA CARTA __________
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardImage = gameObject.GetComponent<Image>();
        cardImage.raycastTarget = false;                //Disattivo il raycastTarget della carta in modo che il mio raycast possa colpire la pila

        targetTransformParent = transform.parent;       //Salvo il mio parent di partenza, in modo da poterci tornare in caso di spostamento non permesso
        lastTransformParent = transform.parent;         //Ne salvo una copia per confronto

        if (transform.parent.tag == "pila")             //Se la carta è stata draggata a partire da una pila controlla se fa parte di una scala
        {
            pilaScript = lastTransformParent.GetComponent<PilaManager>();
            cardIsInLadder = pilaScript.CheckIfLadder(transform.name);
        }

        transform.SetParent(transform.parent.parent);   //Rimuovo la mia carta dalla pila nell'inspector (mi permette di spostarla)
    }

    //Muovo la carta
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //TODO : Aggiungere opzioni di smooth follow rispetto al puntatore
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.raycastTarget = true;                 //Riabilito il raycast della carta   
        transform.SetParent(targetTransformParent);     //Sposto la carta

        if (cardIsInLadder)                             //Se la carta era in una scala allora devo spacchettarla
        {
            Spacchetta(thisCardName, transform);
        }

        //Nel caso in cui cui questa carta sia stata spostata via permanentemente da una Pila dico a quella pila di ricontrollare
        if (transform.parent != lastTransformParent)    //la sua lista degli elementi e le sue richieste
        {
            //scriptScore.UpdateMosse();

            if (transform.parent.tag == "pila")         // Dico a questa pila di refreshare la lista perchè la carta è stata aggiunta
            {
                pilaScript = transform.parent.GetComponent<PilaManager>();
                pilaScript.RefreshCardList();
                pilaScript.GetColorAndNumber();
                //Controllo se sto muovendo la carta tra due pile con richiesta similare, in modo da evitare l'accumulo di punti
                /*PilaManager scriptLastPila;
                scriptLastPila = lastTransformParent.GetComponent<PilaManager>();
                if (scriptLastPila != null)             //lastTransform potrebbe anche essere anche il mazzo, in quel caso non controllare
                {
                    if(pilaScript.numPila != scriptLastPila.numPila)
                        scriptScore.UpdatePunti(5);     // 5 punti se la carta è stata spostata da una pila
                }
            }
            else if (transform.parent.tag == "pilaFinal")
            {
                scriptScore.UpdatePunti(15);*/
            }


            //Vado a dire alla mia vecchia pila che non ne faccio più parte
            if (lastTransformParent.tag == "pila")
            {
                pilaScript = lastTransformParent.GetComponent<PilaManager>();
                pilaScript.RefreshCardList();           //Ricontrolla quali sono gli elementi della lista
                pilaScript.GetColorAndNumber();         //Aggiorna la mia carta richiesta

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

                //scriptScore.UpdatePunti(-15);
            }
            else if (lastTransformParent.tag == "pilaScarti")
            {
                mazzoScript = GameObject.FindGameObjectWithTag("pilaMazzo").GetComponent<MazzoManager>();
                mazzoScript.RemoveCardFromList();

                Debug.Log("Rimuovo " + transform.name + " dalla lista del mazzo.");
            }
        }
    }

    void Spacchetta(string cardName, Transform cartaTransform)
    {
        GameObject childCard = null;

        if (cartaTransform.childCount > 3)
        {
            childCard = cartaTransform.GetChild(3).gameObject;
            Debug.Log("Provo a spacchettare " + childCard.name);

            if (childCard.tag == "carta")
            {
                Debug.Log("Setto il parent di " + childCard.name + " a :" + transform.parent.name);
                childCard.transform.SetParent(transform.parent);

                Image childCardImage = childCard.GetComponent<Image>();
                childCardImage.raycastTarget = true;
                Spacchetta(childCard.name, childCard.transform);    //Ricorsione
            }
        }
        return;
    }
}
