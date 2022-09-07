using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    Material material;
    float iTime;
    Vector4 iDate;
    Vector4 iMouse;

    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        iTime = 0f;    
    }

    void Update()
    {
        iTime += Time.deltaTime;
        material.SetFloat("iTime", iTime);

        iDate = new Vector4(2022, 9, 7, iTime);
        material.SetVector("iDate", iDate);

        iMouse = new Vector4(0, 0, 0, 0);
        material.SetVector("iMouse", iMouse);
    }
}
