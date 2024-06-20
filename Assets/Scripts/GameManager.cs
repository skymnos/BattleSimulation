using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private float[] gameSpeeds;
    [SerializeField] private Button AddUnitButton;
    [SerializeField] private GameObject unitPrefab;

    private float previousGameSpeed;
    private int currentGameSpeedIndex;
    private bool gamePaused;

    private List<BatallionManager> selectedBatallions = new List<BatallionManager>();


    public bool onUI;

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
        if (!context.started)
        {
            return;
        }


        if(!onUI)
        {
            foreach (BatallionManager batallion in selectedBatallions)
            {
                batallion.Unselect();
            }
            selectedBatallions.Clear();
            AddUnitButton.onClick.RemoveAllListeners();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //reconstruit le vecteur direction en 3D depuis l'écran vers le monde
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
        }

        if (Physics.Raycast(ray, out hit))
        {
            BatallionManager batallionSelected = hit.collider.GetComponentInParent<BatallionManager>();
            if (batallionSelected != null)
            {
                batallionSelected.Select();
                selectedBatallions.Add(batallionSelected);
                AddUnitButton.onClick.AddListener(AddUnit);
            }
        }
    }

    void AddUnit()
    {
        selectedBatallions[0].AddUnit(unitPrefab);
    }
}
