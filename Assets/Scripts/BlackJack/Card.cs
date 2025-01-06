using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] public CardGraphic card_graphic;

    private int figura;
    private int kolor;
    public void InitializeValues(int figura_, int kolor_)
    {
        figura = figura_;
        kolor = kolor_;
        card_graphic.InitializeGraphic(figura, kolor);
    }
    public void ZerowanieTranformu()
    {
        transform.position = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public int GetColor()
    {
        return kolor;
    }
    public int GetValue()
    {
        return figura;
    }
    public void SetValue(int i)
    {
        figura = i;
    }
}
