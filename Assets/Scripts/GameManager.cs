using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float surviveDuration = 60f;

    [Header("Map Dimensions")]
    [SerializeField] private int mapWidth = 20;
    [SerializeField] private int mapHeight = 20;

    public Vector2 MapCenter => new Vector2((mapWidth - 1) / 2f, (mapHeight - 1) / 2f);
    public int MapWidth => mapWidth;
    public int MapHeight => mapHeight;

    // Floor tiles occupy cols 1..(mapWidth-2), rows 1..(mapHeight-2) in world space
    public Bounds FloorBounds => new Bounds(
        new Vector3(MapCenter.x, MapCenter.y, 0),
        new Vector3(mapWidth - 2, mapHeight - 2, 0)
    );

    public float playerHealth = 3f;
    public int soldiersRescued { get; private set; }
    public int soldiersDied { get; private set; }
    public float timeRemaining { get; private set; }
    public bool isGameOver { get; private set; }
    public bool isWon { get; private set; }

    void Start()
    {
        timeRemaining = surviveDuration;
    }

    void Update()
    {
        if (isGameOver || isWon) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isWon = true;
            Debug.Log("You Win!");
        }
    }

    void OnEnable()
    {
        PlayerController.OnSoldierRescued += OnPlayerRescueSoldier;
    }

    void OnDisable()
    {
        PlayerController.OnSoldierRescued -= OnPlayerRescueSoldier;
    }

    public void OnPlayerRescueSoldier()
    {
        soldiersRescued++;
        Debug.Log("Soldiers rescued: " + soldiersRescued);
    }

    public void OnPlayerFailRescue()
    {
        soldiersDied++;
        playerHealth -= 1f;
        Debug.Log("Soldiers died: " + soldiersDied + " | Health: " + playerHealth);
        if (playerHealth <= 0f)
        {
            isGameOver = true;
            Debug.Log("Game Over!");
        }
    }
}
