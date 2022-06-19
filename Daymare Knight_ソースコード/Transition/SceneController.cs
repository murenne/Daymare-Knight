using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class SceneController : Singleton<SceneController>,IEndGameObserver
{
    public GameObject PlayerPrefab;
    GameObject player;
    public SceneFade sceneFadePrefab;
    bool isFadeFinish;
    NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }


    void Start() 
    {
        GameManager.Instance.AddObserver(this);
        isFadeFinish = true;
    }


    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch(transitionPoint.transitionType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name,transitionPoint.destinationTag));
            break;

            case TransitionPoint.TransitionType.DifferentScene:
                StartCoroutine(Transition(transitionPoint.sceneName,transitionPoint.destinationTag));
            break;
        }
    }

    IEnumerator Transition(string sceneName,TransitionDestination.DestinationTag destinationTag)
    {
        SceneFade fade = Instantiate(sceneFadePrefab);
        yield return StartCoroutine(fade.FadeOut(2f));
        SaveManagement.Instance.SavePlayerData();
        InventoryManagement.Instance.SaveData();//IEnumerator LoadLevel(string scene)中，在savemanagement。instance。saveplayerdata后一行也要加上

        if(SceneManager.GetActiveScene().name != sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(PlayerPrefab,GetDestination(destinationTag).transform.position,GetDestination(destinationTag).transform.rotation);

            SaveManagement.Instance.LoadPlayerData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;//跳出协程
        }
        else
        {
            player = GameManager.Instance.playerStates.gameObject;
            agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            player.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position,GetDestination(destinationTag).transform.rotation);
            agent.enabled = true;
            yield return null;
            
        }
        
       
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for( int i = 0;i <= entrances.Length ; i++)
        {
            if(entrances[i].destinationTag == destinationTag)
            {
                return entrances[i];
            }
        }
        
        return null;
    }

    public void TransitionToMainMenu()
    {
         StartCoroutine(LoadMain());
    }

    public void TransitionToLoadGame()
    {
        StartCoroutine(LoadLevel(SaveManagement.Instance.SceneName));
    }

    public void TransitionToFirstLevel()
    {
        StartCoroutine(LoadLevel("Wild"));
    }

    IEnumerator LoadLevel(string scene)
    {
        SceneFade fade = Instantiate(sceneFadePrefab);

        if(scene != "")
        {
            yield return StartCoroutine(fade.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(PlayerPrefab,GameManager.Instance.GetEntrance().position,GameManager.Instance.GetEntrance().rotation);

            SaveManagement.Instance.SavePlayerData();
            InventoryManagement.Instance.SaveData();
            yield return StartCoroutine(fade.FadeIn(2f));
            yield break;
        }
       
    }

     IEnumerator LoadMain()
     {
         SceneFade fade = Instantiate(sceneFadePrefab);
         yield return StartCoroutine(fade.FadeOut(2f));
         yield return SceneManager.LoadSceneAsync("MainMenu");
         yield return StartCoroutine(fade.FadeIn(2f));
         yield break;
     }

    public void EndNotify()
    {
        if(isFadeFinish)
        {
            isFadeFinish = false;
            StartCoroutine(LoadMain());
        }
        Destroy(gameObject);
        
    }
}
