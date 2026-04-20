using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class SoliderController : MonoBehaviour
{
  [SerializeField] private float rescueRange = 0.2f; // Radius within which the soldier can rescue the player

  [SerializeField] private float lifetime = 3f; // 3 seconds before returning to pool

  [SerializeField] private GameManager gameManager;

  private PrefabPool objectPool;

  private CircleCollider2D soldierCollider;

  private Coroutine lifetimeCoroutine;

  public void SetPool(PrefabPool pool)
  {
    objectPool = pool;
  }


  private void OnEnable()
  {
    lifetimeCoroutine = StartCoroutine(ReturnToPool());
  }

  private IEnumerator ReturnToPool()
  {
    yield return new WaitForSeconds(lifetime);

    gameManager.OnPlayerFailRescue();
    Release();
  }

  public void Rescue()
  {
    if (lifetimeCoroutine != null)
      StopCoroutine(lifetimeCoroutine);
    Release();
  }

  private void Release()
  {
    if (objectPool != null)
      objectPool.Release(gameObject);
    else
      gameObject.SetActive(false);
  }

  void Awake()
  {
    soldierCollider = GetComponent<CircleCollider2D>();
    if (soldierCollider == null)
      Debug.LogError("No CircleCollider2D found on SoliderController.");

    soldierCollider.radius = rescueRange * Mathf.Max(transform.localScale.x, transform.localScale.y);
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

}
