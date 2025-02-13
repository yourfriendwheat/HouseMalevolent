using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static int lives;
    // Start is called before the first frame update
    void Start()
    {
        lives = 3; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loseLife()
    {
        lives--;

        if(lives == 0)
        {
            Debug.Log("GameOver");
        }
    }
}
