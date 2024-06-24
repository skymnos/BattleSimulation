using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public enum unitsShape
{
    Square,
    Rectangle,
    Triangle,
    Circle,
    Line
}

public enum unitsState
{
    Charge,
    Hold,
    Walk
}

public class BatallionManager : MonoBehaviour, ISelectable
{
    public bool selected { get; set; }

    private List<GameObject> units = new List<GameObject>();

    private unitsShape currentUnitsShape = unitsShape.Rectangle;

    private unitsState currentUnitsState = unitsState.Walk;

    private NavMeshAgent navMeshAgent;

    private Color batallionColor;

    [SerializeField] private float unitsSpeed; 


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
                r = batallionColor.r;
                g = batallionColor.g * 1.3f;
                b = batallionColor.b * 1.3f;
            }
            else if (mat.color.g == 255)
            {
                r = batallionColor.r * 1.3f;
                g = batallionColor.g;
                b = batallionColor.b * 1.3f;
            }
            else
            {
                r = batallionColor.r * 1.3f;
                g = batallionColor.g * 1.3f;
                b = batallionColor.b;
            }
            mat.color = new Color(r, g, b, batallionColor.a);
        }
        selected = true;
    }

    public void Unselect()
    {
        foreach (GameObject unit in units)
        {
            Material mat = unit.GetComponent<Renderer>().material;
            mat.color = batallionColor;
        }
        selected = false;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        batallionColor = gameObject.GetComponentInChildren<Renderer>().material.color;

        foreach (Transform child in transform)
        {
            units.Add(child.gameObject);
        }
        ChangeUnitsShape(unitsShape.Square);
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
        //Unselect();
        GameObject newUnit = Instantiate(unit);
        newUnit.transform.parent = transform;
        newUnit.transform.position = transform.position + new Vector3(-2, 0, -2);
        units.Add(newUnit);
        ChangeUnitsShape(currentUnitsShape);
        Select();
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
        int xOffset = 0;
        int zOffset = 0;
        int spawnedUnits = 0;
        while(spawnedUnits < units.Count)
        {
            for (int i = 0; i < Mathf.Ceil(Mathf.Sqrt(units.Count)); i++)
            {
                Vector3 regroupingPoint = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
                if (spawnedUnits < units.Count)
                {
                    units[spawnedUnits].GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
                }
                xOffset += 2;
                spawnedUnits++;
            }
            xOffset = 0;
            zOffset += 2;
        }
    }

    private void ChangeToRectangle()
    {
        int xOffset = 0;
        int zOffset = 0;
        int spawnedUnits = 0;
        while (spawnedUnits < units.Count)
        {
            for (int i = 0; i < (int)(2 * Mathf.Sqrt(units.Count)); i++)
            {
                Vector3 regroupingPoint = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
                if (spawnedUnits < units.Count)
                {
                    units[spawnedUnits].GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
                }
                xOffset += 2;
                spawnedUnits++;
            }
            xOffset = 0;
            zOffset += 2;
        }
    }

    private void ChangeToTriangle() 
    { 
        int xOffset = 0;
        float zOffset = 0;
        int spawnedUnits = 0;
        int unitsPerLine = 1;
        while (spawnedUnits < units.Count)
        {
            for (int j = 0; j < unitsPerLine; j++)
            {
                Vector3 regroupingPoint = new Vector3(transform.position.x + xOffset, transform.position.y, transform.position.z + zOffset);
                if (spawnedUnits < units.Count)
                {
                    units[spawnedUnits].GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
                }
                xOffset += 2;
                spawnedUnits++;
            }
            xOffset = -1 * unitsPerLine;
            zOffset += 1.8f;
            unitsPerLine++;
        }
    }

    private void ChangeToCircle()
    {
        if (units.Count == 0)
            return;

        float circumference = 2 * units.Count;
        float radius = circumference / (2 * Mathf.PI);
        float angleStep = 360.0f / units.Count;

        for (int i = 0; i < units.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convertir en radians
            Vector3 regroupingPoint = new Vector3(transform.position.x + Mathf.Cos(angle) * radius, transform.position.y, transform.position.z + Mathf.Sin(angle) * radius);
            units[i].GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
        }
    }
    #endregion

    #region Movement
    
    public void ChangeUnitsState(unitsState newUnitsState)
    {
        currentUnitsState = newUnitsState;
        Invoke("ChangeTo" + newUnitsState, 0f);
    }

    public void MoveTo(Vector3 position)
    {
        Vector3 previousPosition = transform.position;

        foreach (GameObject unit in units)
        {
            unit.transform.parent = null;
        }

        transform.position = position;

        foreach (GameObject unit in units) 
        {
            Vector3 offset = unit.transform.position - previousPosition;
            Vector3 regroupingPoint = transform.position + offset;
            unit.GetComponent<EnemyManager>().ChangeRegroupingPoint(regroupingPoint);
            unit.transform.parent = transform;
            NavMeshAgent navMesh = unit.GetComponent<EnemyManager>().navMeshAgent;
            navMesh.speed = unitsSpeed;
            float velocityMultiplier = Random.Range(0.95f, 1.05f);
            navMesh.speed *= velocityMultiplier;
            while (navMesh.pathStatus == NavMeshPathStatus.PathPartial)
            {
            }
            ChangeUnitsShape(currentUnitsShape);
            
        }
    }

    

    #endregion
}
