// DangerZoneSpawner.cs
// ABOUTME: Spawns danger zones every 2 seconds on random floor tiles.
// Respects a max-active cap of 5 and stops spawning on game over / win.
// Sizes each zone to 2x the player's world-space CircleCollider2D radius.

using System.Collections;
using UnityEngine;

public class DangerZoneSpawner : MonoBehaviour
{
    [SerializeField] private PrefabPool dangerZonePool;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxActiveZones = 5;

    private Transform player;
    private CircleCollider2D playerCollider;

    private void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        player = playerObj.transform;
        playerCollider = playerObj.GetComponent<CircleCollider2D>();

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (gameManager.isGameOver || gameManager.isWon) continue;
            if (dangerZonePool.CountActive >= maxActiveZones) continue;

            SpawnZone();
        }
    }

    private void SpawnZone()
    {
        // World-space radius accounts for non-unit player scale
        float worldRadius = playerCollider.radius
            * Mathf.Max(player.localScale.x, player.localScale.y);
        float sideLength = 2f * worldRadius;

        // Random floor tile — exclude wall border (col/row 0 and mapWidth/Height-1)
        int col = Random.Range(1, gameManager.MapWidth - 1);
        int row = Random.Range(1, gameManager.MapHeight - 1);

        GameObject zone = dangerZonePool.Get();
        zone.transform.position = new Vector3(col, row, 0f);
        zone.transform.localScale = new Vector3(sideLength, sideLength, 1f);
    }
}
