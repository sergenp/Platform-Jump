using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShapeShiftButtonUI : MonoBehaviour
{

    public ShapeChanger shapeChanger;

    Button button;
    TextMeshProUGUI buttonName;
    ShapeType shapeType;

    void Start()
    {
        button = GetComponent<Button>();
        buttonName = GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.RemoveAllListeners();
        buttonName.text = "Sphere";
        button.onClick.AddListener(() => ShapeShiftOnClick());
    }


    void ShapeShiftOnClick()
    {
        if (shapeType == ShapeType.Cube)
        {
            shapeType = ShapeType.Sphere;
            buttonName.text = "Cube";
        }
        else
        {
            shapeType = ShapeType.Cube;
            buttonName.text = "Sphere";
        }
        shapeChanger.ChangeShape(shapeType);
    }
}
