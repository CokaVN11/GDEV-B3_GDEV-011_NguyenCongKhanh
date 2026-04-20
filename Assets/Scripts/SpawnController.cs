using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField]
    private PrefabPool soliderPool;

    private Camera mainCamera;

    [SerializeField] private GameObject player;

    [SerializeField] private Grid map;

    [SerializeField] private float spawnInterval = 1f;

    [SerializeField] private float increasingDifficultyRate = 0.1f; // How much to decrease spawn interval per second

    [SerializeField] private int maxActiveSoldiers = 3;


    void Start()
    {
        mainCamera = Camera.main;
        if (player == null)
        {
            Debug.LogError("Player reference not set on SoliderController.");
            player = GameObject.FindWithTag("Player");
        }

        if (map == null)
        {
            Debug.LogError("Grid reference not set on SpawnController.");
            map = FindObjectOfType<Grid>();
        }
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
        // need to spawn within map, and not too close to player.
        Vector3 playerPos = player.transform.position;

        // TODO: convert to spawn within map bounds, not just camera bounds. This is a bit tricky because the map is a Grid and we want to spawn on valid cells.

        Vector3 cameraBounnds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float spawnX = Random.Range(-cameraBounnds.x, cameraBounnds.x);
        float spawnY = Random.Range(-cameraBounnds.y, cameraBounnds.y);
        Vector3 spawnPosition = new(spawnX, spawnY, 0);

        return spawnPosition;
    }
}
