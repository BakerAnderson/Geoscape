using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameLoop : MonoBehaviour
{
    public Country country;
    public List<string> countryFacts = new List<string>();
    public List<Country> allCountries = new List<Country>();
    public List<Country> selectedCountries = new List<Country>();
    public List<Country> playableCountries = new List<Country>();
    public bool selectable;
    public int guesses;
    public int guessesCounter;
    public TabHandler th;
    public GameObject confettiOne;
    public GameObject confettiTwo;

    public GameObject winScreen;
    public TextMeshProUGUI guessedCountryText;
    public Image nextButton;

    public Camera cam;

    public bool lost;

    public GameObject ex;

    public Country finalGuy;

    void Start()
    {
        GameStart();
    }

    void Update()
    {
        if (guessesCounter <= 0)
        {
            EndGame();
        }
    }

    void SelectCountry()
    {
        country = playableCountries[Random.Range(0, playableCountries.Count)];
    }

    void GameStart()
    {
        SelectCountry();
        ResetMap();
    }

    void ResetMap()
    {
        guessesCounter = guesses;
        SelectCountry();
        th.firstFact.fact = country.facts[0];
        th.secondFact.fact = country.facts[1];
        th.thirdFact.fact = country.facts[2];
        th.fourthFact.fact = country.facts[3];
        th.fifthFact.fact = country.facts[4];
    }

    public void GuessCountry(Country countryGuessed)
    {
        bool validCountry = true;
        for (int i = 0; i < selectedCountries.Count; i++)
        {
            if (selectedCountries[i] == countryGuessed)
            {
                validCountry = false;
            }
        }
        if (validCountry)
        {
            selectedCountries.Add(countryGuessed);
            if (countryGuessed == country)
            {
                country.TurnGreen();
                selectable = false;
                finalGuy = country;
                Invoke("Celebration", .2f);
                Invoke("WinScreen", 1.5f);
            }
            else
            {
                countryGuessed.TurnSelectedColor();
                guessesCounter -= 1;
                th.resetColors();
                switch (guessesCounter)
                {
                    case 4:
                        th.selected = th.secondFact;
                        th.secondFact.im.color = th.secondFact.selectedColor;
                        break;
                    case 3:
                        th.selected = th.thirdFact;
                        th.thirdFact.im.color = th.thirdFact.selectedColor;
                        break;
                    case 2:
                        th.selected = th.fourthFact;
                        th.fourthFact.im.color = th.fourthFact.selectedColor;
                        break;
                    case 1:
                        th.selected = th.fifthFact;
                        th.fifthFact.im.color = th.fifthFact.selectedColor;
                        break;
                    default:
                        lost = true;
                        selectable = false;
                        finalGuy = country;
                        Invoke("WinScreen", .5f);
                        ex.SetActive(true);
                        break;
                }
            }
        }
    }

    public void EndGame()
    {
        selectable = true;
        ResetMap();
        foreach (Country c in allCountries)
        {
            c.Reset();
        }
        selectedCountries.Clear();
        th.selected = th.firstFact;
        th.firstFact.selectable = false;
        th.secondFact.selectable = false;
        th.thirdFact.selectable = false;
        th.fourthFact.selectable = false;
        th.fifthFact.selectable = false;
    }

    void Celebration()
    {
        confettiOne.SetActive(true);
        confettiTwo.SetActive(true);
    }

    void WinScreen()
    {
        ex.SetActive(false);
        winScreen.SetActive(true);
        if (lost)
        {
            guessedCountryText.SetText("The correct country was : " + finalGuy.name);
        }
        else 
        {
            confettiOne.SetActive(false);
            confettiTwo.SetActive(false);
            guessedCountryText.SetText(finalGuy.name);
        }
        

    }
}
