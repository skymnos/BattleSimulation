using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class BatallionManager : MonoBehaviour, ISelectable
{
    public bool selected { get; set; }

    private List<GameObject> units = new List<GameObject>();
    private List<CapsuleCollider> unitsColliders = new List<CapsuleCollider>();

    public void Select()
    {
        foreach(GameObject unit in units)
        {
            Material mat = unit.GetComponent<Renderer>().material;
            mat.color = new Color(mat.color.r * 1.3f, mat.color.g * 1.3f, mat.color.b, mat.color.a);
        }
        selected = true;
    }

    public void Unselect()
    {
        foreach (GameObject unit in units)
        {
            Material mat = unit.GetComponent<Renderer>().material;
            mat.color = new Color(mat.color.r / 1.3f, mat.color.g / 1.3f, mat.color.b, mat.color.a);
        }
        selected = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            units.Add(child.gameObject);
            /*CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.isTrigger = true;
            capsuleCollider.radius = child.GetComponent<CapsuleCollider>().radius * 1.5f;
            capsuleCollider.height = child.GetComponent<CapsuleCollider>().height * 1.5f;
            capsuleCollider.center = child.GetComponent<CapsuleCollider>().center;
            unitsColliders.Add(capsuleCollider);*/

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        for (int i  = 0; i < units.Count; i++)
        {
            //unitsColliders[i].center = units[i].GetComponent<CapsuleCollider>().center;
        }
    }
}
