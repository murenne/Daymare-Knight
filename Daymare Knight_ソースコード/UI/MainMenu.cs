using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameButton;
    Button continueButton;
    Button exitButton;

    PlayableDirector director;

    void Awake() 
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        exitButton = transform.GetChild(3).GetComponent<Button>();

        newGameButton.onClick.AddListener(PlayTimeline);
        continueButton.onClick.AddListener(ContinueGame);
        exitButton.onClick.AddListener(ExitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    void PlayTimeline()
    {
        director.Play();
    }

    void NewGame(PlayableDirector director)
    {
        PlayerPrefs.DeleteAll();
        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        SceneController.Instance.TransitionToLoadGame();
    }


    void   ExitGame()
    {
        Application.Quit();
    }


}
