using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private PrefabPool soliderPool;

    [SerializeField] private GameObject player;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float spawnInterval = 1f;

    [SerializeField] private float increasingDifficultyRate = 0.1f; // How much to decrease spawn interval per second

    [SerializeField] private int maxActiveSoldiers = 3;


    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player reference not set on SpawnController.");
            player = GameObject.FindWithTag("Player");
        }

        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        // Gradually increase difficulty by decreasing spawn interval over time.
        spawnInterval = Mathf.Max(0.1f, spawnInterval - increasingDifficultyRate * Time.deltaTime);
    }

    private IEnumerator Spawn()
    {
        if (soliderPool == null)
        {
            Debug.LogError("[SpawnController] SoliderPool was not assigned in the Inspector.");
            yield break;
        }

        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (soliderPool.CountActive >= maxActiveSoldiers) continue;

            var spawnedObject = soliderPool.Get();
            spawnedObject.transform.position = GetSpawnPosition();
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Bounds floor = gameManager.FloorBounds;
        float spawnX = Random.Range(floor.min.x, floor.max.x);
        float spawnY = Random.Range(floor.min.y, floor.max.y);
        return new Vector3(spawnX, spawnY, 0);
    }
}
