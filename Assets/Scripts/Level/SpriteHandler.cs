using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    private Image[] _emptyImage;

    // Set sprites for the Empty images and return the target score
    public int SpriteSetup(Sprite emptySprite, Sprite filledSprite, ColorPicker colorPicker)
    {
        return SetSprite(emptySprite, filledSprite, colorPicker);
    }

    private int SetSprite(Sprite emptySprite, Sprite filledSprite, ColorPicker colorPicker)
    {
        try
        {
            // Load sub-sprites from the main sprite
            var emptySprites = GetSubSprites(emptySprite, "Empty");
            var filledSprites = GetSubSprites(filledSprite, "Filled");
            _emptyImage = new Image[transform.childCount];

            var targetScore = 0;
            foreach (var keyValue in emptySprites)
            {
                var spriteInfo = emptySprites[keyValue.Key].name.Split('_');
                if (spriteInfo.Length != 5) continue;

                _emptyImage[keyValue.Key] = transform.GetChild(keyValue.Key).GetComponent<Image>();
                _emptyImage[keyValue.Key].sprite = emptySprites[keyValue.Key];
                _emptyImage[keyValue.Key].name = string.Format("{0}/{1}", colorPicker.shapesButtonImage[int.Parse(spriteInfo[4])].name, colorPicker.colorButtonImage[int.Parse(spriteInfo[3])].name);
                targetScore++;

                // Set child image's sprite to the matching filled sprite
                _emptyImage[keyValue.Key].transform.GetChild(0).GetComponent<Image>().sprite = filledSprites[keyValue.Key];
            }
            return targetScore;
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Failed to Set Sprites :{0} ", e.Message);
            return -1;
        }
    }

    //Load Sub Sprites from the main sprite
    private static Dictionary<int, Sprite> GetSubSprites(Sprite mainSprite, string type)
    {
        try
        {
            var spriteMap = new Dictionary<int, Sprite>();
            // Asset path of the mainSprite's texture
#if UNITY_EDITOR
            var texturePath = AssetDatabase.GetAssetPath(mainSprite.texture);
            var assets = AssetDatabase.LoadAllAssetsAtPath(texturePath);
            var subSprites = assets.OfType<Sprite>().ToArray();

#else
            var name = type == "Filled" ? "Filled_" : "Empty_";
            name += mainSprite.name.Split('_')[1];
            string resourcePath = $"{name}";
            Sprite[] subSprites = Resources.LoadAll<Sprite>(resourcePath);
#endif
            foreach (var subSprite in subSprites)
            {
                var info = subSprite.name.Split('_');
                spriteMap.Add(int.Parse(info[2]), subSprite);
            }
            return spriteMap;
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Failed to load sub-sprites: {0}", e);
            return null;
        }
    }

    // Set the visibility of the filled sprite and return if score needs increment or decrement
    public int UpdateSprite(int index)
    {
        var img = _emptyImage[index].transform.GetChild(0).gameObject;
        img.SetActive(!img.activeSelf);
        return img.activeSelf ? -1 : 1;
    }
}