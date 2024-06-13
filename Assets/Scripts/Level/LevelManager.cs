using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Sprite[] gridEmptySprites;
    [SerializeField] private Sprite[] gridFilledSprites;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private GameObject spriteRb;

    private ColorPicker _colorPicker;
    private SpriteHandler _spriteHandler;

    private int _targetScore;

    private void Awake()
    {
        _spriteHandler = transform.GetChild(0).GetComponent<SpriteHandler>();
        _colorPicker = transform.GetChild(1).GetComponent<ColorPicker>();
    }

    private void Start()
    {
        if (!InitialSetup())
        {
            Debug.LogError("Initial setup failed.");
        }
    }

    private bool InitialSetup() {
        try
        {
            _colorPicker.InitializeColorAndShapeButtons();
            var mapName = PlayerPrefs.GetString("Map");
            for (var i = 0; i < gridEmptySprites.Length; i++)
            {
                var gridName = gridEmptySprites[i].name.Split('_')[1];
                if (!gridName.Equals(mapName)) continue;

                _targetScore = _spriteHandler.SpriteSetup(gridEmptySprites[i], gridFilledSprites[i], _colorPicker);

                //Update key for the next map
                PlayerPrefs.SetString("Map", gridEmptySprites[i + 1 == gridEmptySprites.Length ? 0 : i + 1].name.Split('_')[1]);
                return true;
            }
            return false;
        }
        catch (System.Exception ex)
        {
            Debug.LogErrorFormat("Failed to Setup initial board due to:{0}", ex);
            return false;
        }
    }

    public void OnButtonClick()
    {
        uiManager.PlayClickSound();
        var clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (clickedButton == null) return;

        var buttonName = clickedButton.name;
        var info = buttonName.Split('/');
        if (buttonName.Length == 0 || _colorPicker.GetSelectedShapeSprite() != info[0] || _colorPicker.GetSelectedColor() != info[1])
        {
            SpawnFallingShape(clickedButton.transform.position);
            return;
        }
        _targetScore += _spriteHandler.UpdateSprite(clickedButton.transform.GetSiblingIndex());
        uiManager.CheckResult(_targetScore);
    }

    private void SpawnFallingShape(Vector2 spawnPoint)
    {
        var rb = Instantiate(spriteRb, spawnPoint, Quaternion.identity).GetComponent<Rigidbody2D>();

        // Set sprite and color of the falling shape
        var spriteRenderer = rb.GetComponent<SpriteRenderer>();
        var info = _colorPicker.GetShapeSpriteAndColor();
        spriteRenderer.sprite = info.Item1;
        spriteRenderer.color = info.Item2;

        var angle = Random.Range(0, 360) * Mathf.Deg2Rad; //Random angle
        var forceDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        rb.AddForce(forceDirection * 2, ForceMode2D.Impulse);
    }
}