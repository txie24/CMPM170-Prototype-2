using UnityEngine;
using UnityEngine.Tilemaps;

public class DemoManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;                 // optional: assign in inspector
    [SerializeField] private Transform player;           // we only need the Transform
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Tilemap[] levels;

    [Header("State")]
    private int currentTilemapIndex = 0;
    private Color currentForegroundColor = Color.white;

    public SceneData SceneData;

    private void Awake()
    {
        if (!cam) cam = FindFirstObjectByType<Camera>();

        if (!player)
        {
            var p = GameObject.FindWithTag("Player");
            if (p) player = p.transform;
            else Debug.LogWarning("DemoManager: No object with tag 'Player' found.");
        }
    }

    private void Start()
    {
        SetSceneData(SceneData);
        SwitchLevel(0);
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;

        if (cam)
        {
            cam.orthographicSize = data.camSize;
            cam.backgroundColor = data.backgroundColor;
        }

        if (levels != null && levels.Length > 0 && levels[currentTilemapIndex])
        {
            levels[currentTilemapIndex].color = data.foregroundColor;
        }

        currentForegroundColor = data.foregroundColor;
    }

    public void SwitchLevel(int index)
    {
        if (levels == null || levels.Length == 0)
        {
            Debug.LogWarning("DemoManager: No levels assigned.");
            return;
        }

        if (index < 0 || index >= levels.Length)
        {
            Debug.LogWarning($"DemoManager: Level index {index} out of range.");
            return;
        }

        if (levels[currentTilemapIndex])
            levels[currentTilemapIndex].gameObject.SetActive(false);

        if (levels[index])
        {
            levels[index].gameObject.SetActive(true);
            levels[index].color = currentForegroundColor;
        }

        if (player && spawnPoint)
            player.position = spawnPoint.position;
        else
            Debug.LogWarning("DemoManager: Missing player or spawnPoint reference.");

        currentTilemapIndex = index;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int next = (currentTilemapIndex == levels.Length - 1) ? 0 : currentTilemapIndex + 1;
            SwitchLevel(next);
        }
    }
}