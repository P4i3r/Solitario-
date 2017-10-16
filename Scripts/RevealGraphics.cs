using UnityEngine;

public class RevealGraphics : MonoBehaviour
{

    CardManager scriptCarta;

    void Start()
    {
        scriptCarta = transform.GetComponent<CardManager>();
        scriptCarta.RevealGraphics();
    }
}
