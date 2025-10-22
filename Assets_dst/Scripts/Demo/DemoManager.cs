using UnityEngine;
using UnityEngine.Tilemaps;

public class DemoManager : MonoBehaviour
{
    private Camera _cam;
    private PlayerMovement _player;

    [SerializeField] private Tilemap[] levels;
    [SerializeField] private Transform spawnPoint;

    private int _currentTilemapIndex;
    private Color _currentForegroundColor;

    public SceneData SceneData;

    private void Awake()
    {
        _cam = FindFirstObjectByType<Camera>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        SetSceneData(SceneData);
        SwitchLevel(0);
    }

    public void SetSceneData(SceneData data)
    {
        SceneData = data;

        _cam.orthographicSize = data.camSize;
        _cam.backgroundColor = data.backgroundColor;
        levels[_currentTilemapIndex].color = data.foregroundColor;

        _currentForegroundColor = data.foregroundColor;
    }


    public void SwitchLevel(int index)
    {
        levels[_currentTilemapIndex].gameObject.SetActive(false);
        levels[index].gameObject.SetActive(true);
        levels[index].color = _currentForegroundColor;

        _player.transform.position = spawnPoint.position;

        _currentTilemapIndex = index;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchLevel((_currentTilemapIndex == levels.Length - 1) ? 0 : _currentTilemapIndex + 1);
        }

    }
}
