using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    Touch _touch;
    [SerializeField] GameObject tutorialObjects;
    [SerializeField] GameObject handObject;

    bool _isTutorialCloseAllowed=true;

    [SerializeField] private ParticleSystem confeti;
    [SerializeField] GameObject nextButton;


    private void Start()
    {
        C_GameManager.OnGameFinish += C_GameManager_OnGameFinish;
    }

    private void C_GameManager_OnGameFinish()
    {
        if (!confeti.isPlaying)
        {
            confeti.Play();
        }
        
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Moved)
            {
                if (_isTutorialCloseAllowed)
                {
                    StartCoroutine(CloseTutorialObjects());
                }
           
            }
        }

    }

    IEnumerator CloseTutorialObjects()
    {
        yield return new WaitForSeconds(1.5f);
        tutorialObjects.SetActive(false);
        handObject.SetActive(false);
        _isTutorialCloseAllowed = false;
    }


    public void LoadMainScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
