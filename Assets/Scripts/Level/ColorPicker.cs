using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [HideInInspector] public Image[] colorButtonImage;
    [HideInInspector] public Image[] shapesButtonImage;

    private RectTransform _colorHighlighter;
    private RectTransform _shapeHighlighter;
    private Dictionary<string, Color> _colorsDictionary;

    private int _selectedColor;
    private int _selectedShape;


    private void Awake()
    {
        _colorsDictionary = new Dictionary<string, Color>();
    }

    public void InitializeColorAndShapeButtons()
    {
        SetColorButtonImages();
        SetShapeButtonImages();
    }

    private void SetColorButtonImages()
    {
        try
        {
            var parent = transform.GetChild(0).GetChild(0).GetChild(0);
            colorButtonImage = new Image[parent.childCount - 1];
            _colorHighlighter = parent.GetChild(parent.childCount - 1).GetComponent<RectTransform>();
            for (var i = 0; i < colorButtonImage.Length; i++)
            {
                colorButtonImage[i] = parent.GetChild(i).GetComponent<Image>();
                _colorsDictionary.Add(colorButtonImage[i].name, colorButtonImage[i].color);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Error setting Button color due to {0}", e.Message);
        }
    }

    private void SetShapeButtonImages()
    {
        try
        {
            var parent = transform.GetChild(1).GetChild(0).GetChild(0);
            shapesButtonImage = new Image[parent.childCount - 1];
            _shapeHighlighter = parent.GetChild(parent.childCount - 1).GetComponent<RectTransform>();
            for (var i = 0; i < shapesButtonImage.Length; i++)
            {
                shapesButtonImage[i] = parent.GetChild(i).GetComponent<Image>();
                shapesButtonImage[i].color = _colorsDictionary[colorButtonImage[_selectedColor].name];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Error setting shape color due to {0}", e.Message);
        }
    }

    private void UpdateSelectedColor(int index)
    {
        try
        {
            _selectedColor = index;
            _colorHighlighter.position = colorButtonImage[_selectedColor].rectTransform.position;
            UpdateShapeColors();
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Error setting Button color due to: {0}", e.Message);
        }
    }

    private void UpdateSelectedShape(int index)
    {
        try
        {
            _selectedShape = index;
            _shapeHighlighter.position = shapesButtonImage[_selectedShape].rectTransform.position;
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Error updating shape selection due to {0}", e.Message);
        }
    }

    private void UpdateShapeColors()
    {
        try
        {
            foreach (var shape in shapesButtonImage)
                shape.color =  _colorsDictionary[colorButtonImage[_selectedColor].name];
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Error updating shape color due to {0}", e.Message);
        }
    }

    public void OnColorOrShapeButtonClick()
    {
        try
        {
            var clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (clickedButton == null) return;
            var buttonName = clickedButton.transform.parent.name.Split(':')[1];

            switch (buttonName)
            {
                case "Color":
                    UpdateSelectedColor(clickedButton.transform.GetSiblingIndex());
                    break;
                case "Shape":
                    UpdateSelectedShape(clickedButton.transform.GetSiblingIndex());
                    break;
                default:
                    Debug.LogError("Invalid Input");
                    break;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogErrorFormat("Invalid Input : {0}", e.Message);
        }
    }

    public string GetSelectedShapeSprite()
    {
        return shapesButtonImage[_selectedShape].name;
    }

    public string GetSelectedColor()
    {
        return colorButtonImage[_selectedColor].name;
    }

    public (Sprite, Color) GetShapeSpriteAndColor()
    {
        return (shapesButtonImage[_selectedShape].sprite, colorButtonImage[_selectedColor].color);
    }
}
