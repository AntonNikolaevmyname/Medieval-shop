using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomeTests : MonoBehaviour
{
    [Range(0,5)]
    [TooltipAttribute("0 пауза")]
    public float tS = 1f;

    void Update()
    {
        Time.timeScale = tS;
    }
}
