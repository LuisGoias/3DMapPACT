using System.Collections;
using System.Collections.Generic;
using TS.ColorPicker;
using UnityEngine;

public class ColorPickerManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private ColorPicker _colorPicker;



    private Color _color;
    private Ray _ray;
    private RaycastHit _hit;

    private Material chosenMaterial;


    // Start is called before the first frame update
    void Start()
    {
        _colorPicker.OnChanged.AddListener(ColorPicker_OnChanged);
        _colorPicker.OnSubmit.AddListener(ColorPicker_OnSubmit);
        _colorPicker.OnCancel.AddListener(ColorPicker_OnCancel);

        _color = _renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100))
            {
                _colorPicker.Open(_color);
            }
        }
    }

    private void ColorPicker_OnChanged(Color color)
    {
        _renderer.material.color = color;
    }
    private void ColorPicker_OnSubmit(Color color)
    {
        _color = color;
        chosenMaterial.color = color;
    }
    private void ColorPicker_OnCancel()
    {
        _renderer.material.color = _color;
    }

    public void SetChoseMaterial(Material newMaterial)
    {
        chosenMaterial = newMaterial;
    }
}
