using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private static void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(string.Format("Level_{0}", levelName));
    }

    // Handles button click event to determine which level to load
    public void OnButtonClick()
    {
        var clickedButton = EventSystem.current.currentSelectedGameObject;
        if (clickedButton == null) return;
        var buttonName = clickedButton.name;

        if (!buttonName.Contains('/')) return;
        var level = buttonName.Split('/');

        SaveMapName("Map", level[1]);
        LoadLevel(level[0]);
    }

    // Saves the selected level map name
    private static void SaveMapName(string mapName, string levelName)
    {
        PlayerPrefs.SetString(mapName, levelName);
    }
}
