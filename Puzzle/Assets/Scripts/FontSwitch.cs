using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSwitch : MonoBehaviour
{
    public Font font;

    private void Start()
    {
        GetComponentInParent<Text>().font = font;
    }
}
