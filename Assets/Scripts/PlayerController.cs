using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 mousePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse position in world space after click
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void FixedUpdate()
    {
        // Move towards the mouse position
        Vector2 direction = (mousePosition - rb.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

}
