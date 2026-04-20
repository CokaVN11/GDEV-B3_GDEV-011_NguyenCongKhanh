using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playerHealth = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerFailRescue()
    {
        playerHealth -= 1f;
        Debug.Log("Player health: " + playerHealth);
        if (playerHealth <= 0f)
        {
            Debug.Log("Game Over!");
        }
    }
}
