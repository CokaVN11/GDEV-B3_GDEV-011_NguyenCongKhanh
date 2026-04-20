// DangerZoneController.cs
// ABOUTME: Manages a single danger zone's lifecycle: 3-second lifetime,
// red-square blink effect, and instant 1-HP damage on player contact.
// Consumed on hit (zone deactivates immediately when player enters).

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DangerZoneController : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private GameManager gameManager;

    private PrefabPool objectPool;
    private SpriteRenderer sr;

    public void SetPool(PrefabPool pool) => objectPool = pool;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(LifetimeRoutine());
        StartCoroutine(BlinkRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);
        Release();
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            Color c = sr.color;
            c.a = (c.a > 0.5f) ? 0.15f : 1f;
            sr.color = c;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.OnPlayerFailRescue();
            Release();
        }
    }

    private void Release()
    {
        if (objectPool != null)
            objectPool.Release(gameObject);
        else
            gameObject.SetActive(false);
    }
}
