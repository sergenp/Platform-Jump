using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChanger : MonoBehaviour
{
    public new Renderer renderer;
    public ColorPicker picker;

    [Header("Failed Panel")]
    public GameObject customizeFailedPanel;
    public GameObject successfullyAppliedPanel;

    private Color Color;
    
    // Use this for initialization
    void Start()
    {
        picker.CurrentColor = PlayerDataManager.instance.GetCubeColor();
        picker.onValueChanged.AddListener(color =>
        {
            renderer.material.color = color;
            Color = color;
        });
    }

    public void ApplyChange()
    {
        bool successful = PlayerDataManager.instance.DecreaseGold(500);

        if (successful)
        {
            PlayerDataManager.instance.SetCubeColor(Color);
            successfullyAppliedPanel.SetActive(true);
        }
        else
        {
            customizeFailedPanel.SetActive(true);
        }
    }
}
