using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public enum unitsShape
{
    Square,
    Rectangle,
    Triangle,
    Circle,
    Line
}

public class BatallionManager : MonoBehaviour, ISelectable
{
    public bool selected { get; set; }

    private List<GameObject> units = new List<GameObject>();

    private unitsShape currentUnitsShape = unitsShape.Rectangle;


    #region Selection
    public void Select()
    {
        float r;
        float g;
        float b;
        foreach (GameObject unit in units)
        {
            Material mat = unit.GetComponent<Renderer>().material;

            if (mat.color.r == 255)
            {
                r = mat.color.r;
                g = mat.color.g * 1.3f;
                b = mat.color.b * 1.3f;
            }
            else if (mat.color.g == 255)
            {
                r = mat.color.r * 1.3f;
                g = mat.color.g;
                b = mat.color.b * 1.3f;
            }
            else
            {
                r = mat.color.r * 1.3f;
                g = mat.color.g * 1.3f;
                b = mat.color.b;
            }
            mat.color = new Color(r, g, b, mat.color.a);
        }
        selected = true;
    }

    public void Unselect()
    {
        float r;
        float g;
        float b;

        foreach (GameObject unit in units)
        {
            Material mat = unit.GetComponent<Renderer>().material;

            if (mat.color.r == 255)
            {
                r = mat.color.r;
                g = mat.color.g / 1.3f;
                b = mat.color.b / 1.3f;
            }
            else if (mat.color.g == 255)
            {
                r = mat.color.r / 1.3f;
                g = mat.color.g;
                b = mat.color.b / 1.3f;
            }
            else
            {
                r = mat.color.r / 1.3f;
                g = mat.color.g / 1.3f;
                b = mat.color.b;
            }

            mat.color = new Color(r, g, b, mat.color.a);
        }
        selected = true;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            units.Add(child.gameObject);
        }
        ChangeUnitsShape(unitsShape.Line);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
    
    }

    public void AddUnit(GameObject unit)
    {
        Unselect();
        GameObject newUnit = Instantiate(unit);
        newUnit.transform.parent = transform;
        newUnit.transform.position = transform.position + new Vector3(-2, 0, -2);
        units.Add(newUnit);
        ChangeUnitsShape(currentUnitsShape);
        if (selected)
        {
            Select();
        }
    }

    public void RemoveUnit(GameObject unit)
    {
        units.Remove(unit);
        ChangeUnitsShape(currentUnitsShape);
    }


    #region UnitsShape
    public void ChangeUnitsShape(unitsShape newUnitsShape)
    {
        currentUnitsShape = newUnitsShape;
        Invoke("ChangeTo" + newUnitsShape, 0f);
    }

    private void ChangeToLine()
    {
        int xOffset = 0;
        foreach(GameObject unit in units)
        {
            Vector3 regroupingPoint = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z);
            unit.GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
            xOffset += 2;
        }
    }

    private void ChangeToSquare()
    {

    }

    private void ChangeToRectangle()
    {

    }

    private void ChangeToTriangle() 
    { 
        
    }

    private void ChangeToCircle()
    {

    }
    #endregion
}
