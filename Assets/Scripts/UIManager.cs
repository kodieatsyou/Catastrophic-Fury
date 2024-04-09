using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(Instance);
            Instance = this;
        }
    }
    public GameObject pauseInstructions;
    public GameObject mainMenu;
    public GameObject credits;
    public GameObject instructions;
    public GameObject gameOver;
    public GameObject overLay;
    public GameObject pauseScreen;
    public TMP_Text GOdestoyedObjectTally;
    public TMP_Text GOtotalMoney;
    public TMP_Text OLTotalMoney;
    public Image OLAngerBar;
    public RectTransform OLAngerBarArrow;
    public Button GOMainMenuButton;
    public Image angerExpression;
    public Sprite happyFace;
    public Sprite midFace;
    public Sprite madFace;
    public float gameOverCountTime;
    private float GOtotal = 0f;
    private float totalAnger = 0f;

    public void ShowMainMenu()
    {
        pauseScreen.SetActive(false);
        mainMenu.SetActive(true);
        overLay.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(false);
        gameOver.SetActive(false);
    }

    public void ShowCredits()
    {
        pauseScreen.SetActive(false);
        mainMenu.SetActive(false);
        overLay.SetActive(false);
        credits.SetActive(true);
        instructions.SetActive(false);
        gameOver.SetActive(false);
    }

    public void ShowInstructions()
    {
        pauseScreen.SetActive(false);
        mainMenu.SetActive(false);
        overLay.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(true);
        gameOver.SetActive(false);
    }

    public void StartGameUI()
    {
        pauseInstructions.SetActive(false);
        pauseScreen.SetActive(false);
        overLay.SetActive(true);
        mainMenu.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(false);
        gameOver.SetActive(false);
        SetAngerMeterAmount(0);
        UpdateBrokenMoneyAmount();
    }

    public void UpdateBrokenMoneyAmount()
    {
        StartCoroutine(PulseText(OLTotalMoney));
        OLTotalMoney.text = $"Total: ${GameManager.Instance.moneyBroken}";
    }

    public void SetAngerMeterAmount(float anger)
    {
        if(anger < 4)
        {
            angerExpression.sprite = happyFace;
        } else if(anger > 6)
        {
            angerExpression.sprite = madFace;
        } else
        {
            angerExpression.sprite = midFace;
        }
        totalAnger = Mathf.Clamp(anger, 0, 10);

        float normalizedPosition = totalAnger / 10f;
        Debug.Log(normalizedPosition);

        float targetX = (OLAngerBar.rectTransform.sizeDelta.x) * normalizedPosition;

        // Update the anchored position of the arrow
        Vector2 newPosition = OLAngerBarArrow.anchoredPosition;
        newPosition.x = -(OLAngerBar.rectTransform.sizeDelta.x / 2.0f) + targetX;
        OLAngerBarArrow.anchoredPosition = newPosition;
    }

    public void OnPlayButtonClick()
    {
        StartGameUI();
        GameManager.Instance.StartGame();
    }

    public void OnCreditsButtonClick()
    {
        ShowCredits();
    }

    public void OnInstructionsButtonClick()
    {
        ShowInstructions();
    }

    public void ShowGameOver()
    {
        overLay.SetActive(false);
        mainMenu.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(false);
        gameOver.SetActive(true);
        GOMainMenuButton.enabled = false;
        if(GameManager.Instance.objectsDestroyedNames.Count > 0)
        {
            StartCoroutine(TallyObjects());
        } else
        {
            GOdestoyedObjectTally.text = $"It seems you didnt destroy anything...!";
            GOtotalMoney.text = "Total Value: :(";
            GOMainMenuButton.enabled = true;
        }

    }

    IEnumerator TallyObjects()
    {
        GOdestoyedObjectTally.enabled = false;
        GOtotalMoney.enabled = false;
        yield return new WaitForSeconds(1f);
        GOdestoyedObjectTally.enabled = true;
        GOtotalMoney.enabled = true;
        for (int i = 0; i < GameManager.Instance.objectsDestroyedNames.Count; i++)
        {
            GOdestoyedObjectTally.text = $"{GameManager.Instance.objectsDestroyedNames[i]}: ${GameManager.Instance.objectsDestroyedCost[i]}!";
            yield return StartCoroutine(IncrementTotalValue(GameManager.Instance.objectsDestroyedCost[i]));
            yield return StartCoroutine(PulseText(GOtotalMoney));
            // Wait for the specified display time
            yield return new WaitForSeconds(gameOverCountTime);
        }
        GOMainMenuButton.enabled=true;
        // After finishing the loop, stop the coroutine
        StopCoroutine(TallyObjects());
    }

    IEnumerator PulseText(TMP_Text text)
    {
        float timer = 0f;
        Vector3 defaultScale = text.transform.localScale;

        while (timer < 0.1f)
        {
            // Calculate the scale based on the pulse effect
            float scale = Mathf.Lerp(1f, 0.2f, timer / 0.1f);
            text.transform.localScale = defaultScale * scale;

            // Update the timer
            timer += Time.deltaTime;

            yield return null;
        }

        // Reset the scale to its default value
        text.transform.localScale = defaultScale;
    }

    IEnumerator IncrementTotalValue(int amountToAdd)
    {
        float currentValue = GOtotal;
        float targetValue = GOtotal + amountToAdd;

        while (GOtotal < targetValue)
        {
            GOtotal++;
            GOtotalMoney.text = "Total Value: " + GOtotal;
            yield return null;
        }
    }

    public void TogglePauseScreen()
    {
        pauseScreen.SetActive(!pauseScreen.activeSelf);
        mainMenu.SetActive(false);
        overLay.SetActive(false);
        credits.SetActive(false);
        instructions.SetActive(false);
        gameOver.SetActive(false);
        pauseInstructions.SetActive(false);
    }

    public void ShowPauseInstructions()
    {
        pauseInstructions.SetActive(true);
    }

    public void CloseInstructions()
    {
        pauseInstructions.SetActive(false);
    }
}
