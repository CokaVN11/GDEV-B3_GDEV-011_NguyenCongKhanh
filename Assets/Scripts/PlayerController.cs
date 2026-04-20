using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rescueDuration = 3f;
    [SerializeField] private GameManager gameManager;

    public static event System.Action OnSoldierRescued;

    private Vector2 mousePosition;
    private Coroutine rescueCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        transform.position = (Vector3)gameManager.MapCenter;
        mousePosition = gameManager.MapCenter;
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        SoliderController soldier = collision.GetComponent<SoliderController>();
        if (soldier != null)
            rescueCoroutine = StartCoroutine(RescueRoutine(soldier));
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<SoliderController>() != null && rescueCoroutine != null)
        {
            StopCoroutine(rescueCoroutine);
            rescueCoroutine = null;
        }
    }

    IEnumerator RescueRoutine(SoliderController soldier)
    {
        yield return new WaitForSeconds(rescueDuration);
        OnSoldierRescued?.Invoke();
        soldier.Rescue();
        rescueCoroutine = null;
    }

}
