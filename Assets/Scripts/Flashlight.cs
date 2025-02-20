using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    private bool FlashlightEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(FlashlightEnabled == false)
            {
                FlashlightLight.gameObject.SetActive(true);
                FlashlightEnabled = true;
            }
            else
            {
                FlashlightLight.gameObject.SetActive(false);
                FlashlightEnabled = false;
            }
        }
    }
}
