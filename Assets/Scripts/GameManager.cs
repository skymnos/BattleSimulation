using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private float[] gameSpeeds;
    
    [SerializeField] private Button AddUnitButton;
    [SerializeField] private Button LineShapeButton;
    [SerializeField] private Button SquareShapeButton;
    [SerializeField] private Button RectangleShapeButton;
    [SerializeField] private Button TriangleShapeButton;
    [SerializeField] private Button CircleShapeButton;

    [SerializeField] private GameObject unitPrefab;

    private float previousGameSpeed;
    private int currentGameSpeedIndex;
    private bool gamePaused;

    private List<BatallionManager> selectedBatallions = new List<BatallionManager>();

    public bool onUI;

    private bool holdClick;
    private Vector2 selectionStartingPoint;
    [SerializeField] private SelectionRectangle selectionRectangle;


    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;

        //find the index corresponding to a gameSpeed of 1
        for (int i = 0; i < gameSpeeds.Length; i++)
        {
            if (gameSpeeds[i] == 1)
            {
                currentGameSpeedIndex = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PauseBattle()
    {
        previousGameSpeed = Time.timeScale;
        Time.timeScale = 0f;
        gamePaused = true;
    }

    private void UnpauseBattle()
    {
        Time.timeScale = previousGameSpeed;
        gamePaused = false;
    }

    private void AccelerateBattle()
    {
        currentGameSpeedIndex = (currentGameSpeedIndex == gameSpeeds.Length-1)? gameSpeeds.Length - 1 : currentGameSpeedIndex + 1;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    private void SlowBattle()
    {
        currentGameSpeedIndex = (currentGameSpeedIndex == 0) ? 0 : currentGameSpeedIndex - 1;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    public void PauseButton()
    {
        if (gamePaused)
        {
            UnpauseBattle();
            pauseButtonText.text = "||";
        }
        else
        {
            PauseBattle();
            pauseButtonText.text = "|>";
        }
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }

    public void AccelerateButton()
    {
        AccelerateBattle();
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }

    public void SlowButton()
    {
        SlowBattle();
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }

    public void SelectObject(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            holdClick = false;
            selectionStartingPoint = Input.mousePosition;
        }

        if (context.performed)
        {
            holdClick = true;
            selectionRectangle.gameObject.SetActive(true);
            selectionRectangle.startingPoint = selectionStartingPoint;

        }

        if (context.canceled && holdClick)
        {
            selectionRectangle.gameObject.SetActive(false);
            Vector2 selectionEndingPoint = Input.mousePosition;

            if (onUI)
            {
                return;
            }

            foreach (BatallionManager batallion in selectedBatallions)
            {
                batallion.Unselect();
            }
            selectedBatallions.Clear();
            AddUnitButton.onClick.RemoveAllListeners();
            LineShapeButton.onClick.RemoveAllListeners();
            SquareShapeButton.onClick.RemoveAllListeners();
            RectangleShapeButton.onClick.RemoveAllListeners();
            TriangleShapeButton.onClick.RemoveAllListeners();
            CircleShapeButton.onClick.RemoveAllListeners();

            // create a ray every 10 and select batallion if it hit one, might consume a lot of ressources 
            // need to add a way to have more ray if camera is far and less if close
            for (float i = (int) selectionStartingPoint.x; i < (int) selectionEndingPoint.x; i+=10)
            {
                for (float j = (int) selectionEndingPoint.y; j < (int) selectionStartingPoint.y; j+=10) 
                {
                    Ray ray = Camera.main.ScreenPointToRay( new Vector2( i, j ) );
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        //Debug.DrawLine(ray.origin, hit.point, Color.red, 10f);
                        BatallionManager batallionSelected = hit.collider.GetComponentInParent<BatallionManager>();
                        if (batallionSelected != null && !batallionSelected.selected)
                        {
                            batallionSelected.Select();
                            selectedBatallions.Add(batallionSelected);
                            AddUnitButton.onClick.AddListener(() => batallionSelected.AddUnit(unitPrefab));
                            LineShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Line));
                            SquareShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Square));
                            RectangleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Rectangle));
                            TriangleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Triangle));
                            CircleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Circle));

                        }
                    }
                }
            }
        }

        if (context.canceled && !holdClick)
        {
            if (onUI)
            {
                return;
            }

            foreach (BatallionManager batallion in selectedBatallions)
            {
                batallion.Unselect();
            }
            selectedBatallions.Clear();
            AddUnitButton.onClick.RemoveAllListeners();
            LineShapeButton.onClick.RemoveAllListeners();
            SquareShapeButton.onClick.RemoveAllListeners();
            RectangleShapeButton.onClick.RemoveAllListeners();
            TriangleShapeButton.onClick.RemoveAllListeners();
            CircleShapeButton.onClick.RemoveAllListeners();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //reconstruit le vecteur direction en 3D depuis l'écran vers le monde
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BatallionManager batallionSelected = hit.collider.GetComponentInParent<BatallionManager>();
                if (batallionSelected != null)
                {
                    batallionSelected.Select();
                    selectedBatallions.Add(batallionSelected);
                    AddUnitButton.onClick.AddListener(() => batallionSelected.AddUnit(unitPrefab));
                    LineShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Line));
                    SquareShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Square));
                    RectangleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Rectangle));
                    TriangleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Triangle));
                    CircleShapeButton.onClick.AddListener(() => batallionSelected.ChangeUnitsShape(unitsShape.Circle));

                }
            }
        }
    }

    public void Action(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //reconstruit le vecteur direction en 3D depuis l'écran vers le monde
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            foreach(BatallionManager batallion in selectedBatallions)
            {
                batallion.MoveTo(hit.point);
            }
        }
    }
}
