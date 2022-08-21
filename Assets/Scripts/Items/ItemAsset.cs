using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAsset : MonoBehaviour
{
    public static ItemAsset Instance{ get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite SwordSprite;
    public Sprite AxeSprite;
    public Sprite HelmetSprite;
    public Sprite BreastplateSprite;
    public Sprite PantsSprite;
    public Sprite BootsSprite;
    public Sprite MaceSprite;
}
