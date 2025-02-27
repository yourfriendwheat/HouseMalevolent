using UnityEngine;

public class Player_Collision : MonoBehaviour
{
    public bool isGrounded;
    public bool getKey = false;
    public AudioClip ItemSound;
    public LayerMask groundLayer;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("Wood"))
        {
            isGrounded = true;
            Debug.Log("Player is grounded on: " + collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("Car"))
        {
            Debug.Log("You need a car key!");
        }

        if (collision.gameObject.CompareTag("CarKey"))
        {
            audioSource.PlayOneShot(ItemSound);
            getKey = true;
            Debug.Log("You got a car key!");
            Destroy(GameObject.FindWithTag("CarKey"));
        }

        if (collision.gameObject.CompareTag("Car") && getKey == true)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Win();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("Wood"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grass") || collision.gameObject.CompareTag("Road") || collision.gameObject.CompareTag("Wood"))
        {
            isGrounded = false;
            Debug.Log("Player is no longer grounded.");
        }
    }
}