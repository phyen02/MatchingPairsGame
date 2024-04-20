using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    [SerializeField]
    private Sprite backgroundImg;
    [SerializeField]
    public Sprite[] sprArray;
    private bool firstClick, secondClick;
    string firstName, secondName;
    int firstIndex, secondIndex, winTarget, correctGuess = 0, wrongGuess = 0;
    public List<Sprite> sprList = new List<Sprite>();
    private List<Button> btnList = new List<Button>();

    void Awake()
    {
        // load all resources to sprArray
        sprArray = Resources.LoadAll<Sprite>("Sprites/Puzzles");
    }

    // Start is called before the first frame update
    void Start()
    {
        getButtons();
        winTarget = btnList.Count / 2;
        addListener();
        addSprite();
        shuffleSprite(sprList);
        Debug.Log("win target: " + winTarget);
    }

    void addSprite()
    {
        int size = btnList.Count;
        int index = 0;
        /*
        ex: size: 16 elements
        add each sprite from sprArray[index] to sprList 
        index must to < size/2 -> so that puzzle can repeat twice
        */
        for (int i = 0; i < size; i++)
        {
            if (i == size / 2)
            {
                index = 0;
            }
            sprList.Add(sprArray[index]);
            index++;
        }
    }

    // get all buttons with tag "PuzzleButton" add to btnList[]
    void getButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");
        for (int i = 0; i < objects.Length; i++)
        {
            btnList.Add(objects[i].GetComponent<Button>());
            btnList[i].image.sprite = backgroundImg;
        }
    }

    void addListener()
    {
        foreach (Button btn in btnList)
        {
            btn.onClick.AddListener(() => pickPuzzle());
        }
    }

    void pickPuzzle()
    {
        if (!firstClick && sprList != null)
        {
            firstClick = true;
            firstIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstName = sprList[firstIndex].name;
            btnList[firstIndex].image.sprite = sprList[firstIndex];
            //Debug.Log("Sprite's index: " + firstIndex + " Sprite's name: " + firstName);
        }
        else if (!secondClick)
        {
            GameObject currentSelected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (currentSelected != btnList[firstIndex].gameObject)
            {
                wrongGuess++;
                secondClick = true;
                secondIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
                secondName = sprList[secondIndex].name;
                btnList[secondIndex].image.sprite = sprList[secondIndex];
                //Debug.Log("Sprite's index: " + secondIndex + " Sprite's name: " + secondName);
                Debug.Log("Wrong guess: " + wrongGuess);
                StartCoroutine(checkIfMatch());
            }
        }
        else
        {
            Debug.Log("sprList is null");
        }
    }

    IEnumerator checkIfMatch()
    {
        yield return new WaitForSeconds(1);
        if (firstName == secondName && firstIndex != secondIndex)
            {
                correctGuess++;
                btnList[firstIndex].interactable = false;
                btnList[secondIndex].interactable = false;
                Debug.Log("Correct guess: " + correctGuess);
            }
            else if (firstName != secondName && firstIndex != secondIndex)
            {
                btnList[firstIndex].image.sprite = backgroundImg;
                btnList[secondIndex].image.sprite = backgroundImg;
            }
        checkIfWin();
        firstClick = secondClick = false;
    }

    void checkIfWin()
    {
        if (correctGuess == winTarget)
        {
            Debug.Log("Win with " + wrongGuess + " attemps");
        }
    }

    void shuffleSprite(List<Sprite> list)
    {
        Sprite temp;
        for (int i = 0; i < list.Count; i++)
        {   
            temp = list[i];
            int random = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[random];
            list[random] = temp;
        }
    }
}
