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

    [Header("Letter UI")]
    public GameObject fourSwitchUI;
    public GameObject fiveSwitchUI;
    public GameObject sixSwitchUI;

    [Header("Sub-Sequences")]
    [Min(1)]
    public int requiredSubSequences = 3;

    public int completedSubSequences = 0;

    public TextMeshProUGUI progressText;
    public TextMeshProUGUI sequenceCompleteText;

    public float timeToSwitch = 2f;
    public float sequenceCompleteDuration = 2f;

    private bool changingBoards = false;



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
        if (changingBoards) return;

        currentIndex = 0;
        completedSubSequences = 0;

        UpdateProgressText();

        int nextBoard;

        do
        {
            nextBoard = Random.Range(0, boardArray.Length);
        }
        while (boardArray.Length > 1 && nextBoard == previousBoardIndex);

        previousBoardIndex = nextBoard;

        currentBoard = boardArray[nextBoard];

        UpdateSequenceUI(currentBoard.switches.Length);

        currentBoard.ActivateBoard();

        StartNewSubSequence();
    }
    private void StartNewSubSequence()
    {
        currentIndex = 0;

        currentSequence = GenerateSequence(currentBoard.switches.Length);

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
        if (currentSequence == null || currentBoard == null)
            return;

        if (currentIndex < currentSequence.Length)
            return;

        completedSubSequences++;
        UpdateProgressText();

        if (completedSubSequences < requiredSubSequences)
        {
            Debug.Log(
                "Sub-sequence complete: " +
                completedSubSequences + "/" +
                requiredSubSequences
            );

            StartNewSubSequence();
        }
        else
        {
            Debug.Log("All sub-sequences complete");

            StartCoroutine(CompleteBoardSequence());
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
                result += "|";
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

    private void UpdateSequenceUI(int switchCount)
    {
        fourSwitchUI.SetActive(false);
        fiveSwitchUI.SetActive(false);
        sixSwitchUI.SetActive(false);

        switch (switchCount)
        {
            case 4:
                fourSwitchUI.SetActive(true);
                break;

            case 5:
                fiveSwitchUI.SetActive(true);
                break;

            case 6:
                sixSwitchUI.SetActive(true);
                break;
        }
    }

    private void UpdateProgressText()
    {
        if (progressText == null)
            return;

        progressText.text =
            completedSubSequences + "/" + requiredSubSequences;
    }

    private IEnumerator CompleteBoardSequence()
    {
        changingBoards = true;

        float degreesCooled = currentBoard.degreesCooled;

        currentBoard.boardLocked = true;
        currentBoard.DeactivateBoard();

        CoolingSequence(degreesCooled);

        StartCoroutine(CompletionTextSequence());

        yield return new WaitForSeconds(timeToSwitch);

        currentBoard = null;
        changingBoards = false;

        NewSequence();
    }

    private IEnumerator CompletionTextSequence()
    {
        if (sequenceCompleteText == null)
            yield break;

        if (sequenceText != null)
            sequenceText.gameObject.SetActive(false);

        float elapsed = 0f;
        bool visible = true;

        while (elapsed < sequenceCompleteDuration)
        {
            sequenceCompleteText.gameObject.SetActive(visible);
            visible = !visible;

            yield return new WaitForSeconds(0.25f);
            elapsed += 0.25f;
        }

        sequenceCompleteText.gameObject.SetActive(false);

        if (sequenceText != null)
            sequenceText.gameObject.SetActive(true);
    }
}