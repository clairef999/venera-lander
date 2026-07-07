using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TempManager : MonoBehaviour
{
    public static TempManager instance { get; private set; }

    public TextMeshProUGUI sequenceText;

    public Slider tempSlider;
    public CanvasGroup minigameUI;
    public CanvasGroup tempUI;

    public float stableTemp;
    public SwitchBoard[] boardArray;



    public int[] currentSequence;
    public int currentIndex;
    public SwitchBoard currentBoard;

    public bool canProgress = true;
    public bool debugging;

    private int previousBoardIndex = -1;

    [Range(0f, 100f)]
    public float cascadeChance = 25f;

    public float landerTemp = 0f;
    public bool tempDecreasing = false;

    [Header("Temperature UI")]
    public TextMeshProUGUI temperatureText;
    public TextMeshProUGUI coolingText;

    [Header("Temperature Settings")]
    public float heatRate = 1f;
    public float coolingTime = 2f;
    public float cautionaryTemp = 50f;
    public float extremeTemp = 75f;

    [Header("Temperature Colors")]
    public Color acceptableColor;
    public Color cautionaryColor;
    public Color extremeColor;



    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        HeatOverTime();
        UpdateTemperatureUI();
        TempCheck();
    }

    public void TempCheck()
    {
        if (currentBoard == null)
        {
            NewSequence();
        }
    }

    private void HeatOverTime()
    {
        if (tempDecreasing) return;

        landerTemp += heatRate * Time.deltaTime;
    }

    private void UpdateTemperatureUI()
    {
        if (temperatureText == null) return;

        temperatureText.text = landerTemp.ToString("F0") + "°";

        if (landerTemp < cautionaryTemp)
        {
            temperatureText.color = acceptableColor;
        }
        else if (landerTemp < extremeTemp)
        {
            temperatureText.color = cautionaryColor;
        }
        else
        {
            temperatureText.color = extremeColor;
        }
    }


    public void NewSequence()
    {
        currentIndex = 0;

        int nextBoard;

        do
        {
            nextBoard = Random.Range(0, boardArray.Length);
        }
        while (boardArray.Length > 1 && nextBoard == previousBoardIndex);

        previousBoardIndex = nextBoard;

        currentBoard = boardArray[nextBoard];
        currentSequence = GenerateSequence(currentBoard.switches.Length);

        currentBoard.ActivateBoard();

        UpdateSequenceDisplay(currentSequence);
    }
    private int[] GenerateSequence(int length)
    {
        int[] order = new int[length];

        bool useCascade = Random.Range(0f, 100f) < cascadeChance;

        if (useCascade)
        {
            bool goUp = Random.value < 0.5f;

            for (int i = 0; i < length; i++)
            {
                order[i] = goUp ? i : length - 1 - i;
            }

            return order;
        }

        for (int i = 0; i < length; i++)
        {
            order[i] = i;
        }

        for (int i = length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            int temp = order[i];
            order[i] = order[j];
            order[j] = temp;
        }

        return order;
    }

    public void Progress()
    {
        if (currentIndex >= currentSequence.Length)
        {
            Debug.Log("Sequence Complete");

            CoolingSequence(currentBoard.degreesCooled);

            currentBoard.DeactivateBoard();

            currentBoard = null;
            NewSequence();
        }
    }

    public void CoolingSequence(float degreesCooled)
    {
        if (!tempDecreasing)
        {
            StartCoroutine(CoolingRoutine(degreesCooled));
        }
    }
    public void UpdateSequenceDisplay(int[] sequence)
    {
        int[] displayOrder = new int[sequence.Length];

        for (int orderIndex = 0; orderIndex < sequence.Length; orderIndex++)
        {
            int switchIndex = sequence[orderIndex];

            displayOrder[switchIndex] = orderIndex + 1;
        }

        string result = "";

        for (int i = 0; i < displayOrder.Length; i++)
        {
            result += displayOrder[i].ToString();

            if (i < displayOrder.Length - 1)
            {
                result += " | ";
            }
        }

        if (sequenceText != null)
        {
            sequenceText.text = result;
        }

        Debug.Log("Displayed switch order: " + result);
    }

    private IEnumerator CoolingRoutine(float degreesCooled)
    {
        tempDecreasing = true;

        if (coolingText != null)
        {
            coolingText.gameObject.SetActive(true);
        }

        float startTemp = landerTemp;
        float targetTemp = landerTemp - degreesCooled;

        float elapsed = 0f;

        while (elapsed < coolingTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / coolingTime;

            landerTemp = Mathf.Lerp(startTemp, targetTemp, t);

            UpdateTemperatureUI();

            yield return null;
        }

        landerTemp = targetTemp;

        if (coolingText != null)
        {
            coolingText.gameObject.SetActive(false);
        }

        tempDecreasing = false;
    }
}