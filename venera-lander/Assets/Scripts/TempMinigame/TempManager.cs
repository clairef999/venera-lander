using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempManager : MonoBehaviour
{
    public static TempManager instance { get; private set; }

    public float landerTemp;
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI sequenceText;

    public Slider tempSlider;
    public CanvasGroup minigameUI;
    public CanvasGroup tempUI;

    public float stableTemp;
    public SwitchBoard[] boardArray;

    public bool tempDecreasing = false;

    public int[] currentSequence;
    public int currentIndex;
    public SwitchBoard currentBoard;

    public bool canProgress = true;
    public bool debugging;

    private int previousBoardIndex = -1;

    [Range(0f, 100f)]
    public float cascadeChance = 25f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        TempCheck();
    }

    public void TempCheck()
    {
        if (landerTemp >= stableTemp && !tempDecreasing && currentBoard == null)
        {
            NewSequence();
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

    /*
    public void PrintOrderInConsole(int[] sequence)
    {
        string result = "";

        for (int i = 0; i < sequence.Length; i++)
        {
            result += (sequence[i] + 1).ToString();

            if (i < sequence.Length - 1)
                result += ", ";
        }

        Debug.Log("Correct switch order: " + result);
    }
    */

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

            currentBoard.DeactivateBoard();
            currentBoard = null;

            // Cooling(currentBoard.degreesCooled);

            NewSequence();
        }
    }

    public void Cooling(float coolingAmount)
    {
        landerTemp -= coolingAmount;
    }

    /* OLD SEQUENCE DISPLAY METHOD
    public void UpdateSequenceDisplay(int[] sequence)
    {
        string result = "";

        for (int i = 0; i < sequence.Length; i++)
        {
        
            result += (sequence[i] + 1).ToString();

            if (i < sequence.Length - 1)
            {
                result += " | ";
            }
        }

       
        if (sequenceText != null)
        {
            sequenceText.text = result;
        }


        if (debugging) Debug.Log("Correct switch order: " + result);
    }
    */
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
}