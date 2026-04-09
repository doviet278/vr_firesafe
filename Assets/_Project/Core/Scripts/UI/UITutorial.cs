using UnityEngine;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialFire;
    [SerializeField] private GameObject tutorialF;
    [SerializeField] private GameObject tutorialQ;

    public void ShowTutorialFire()
    {
        tutorialFire.SetActive(true);
    }

    public void HideTutorialFire()
    {
        tutorialFire.SetActive(false);
    }

    public void ShowTutorialF()
    {
        tutorialF.SetActive(true);
    }

    public void HideTutorialF()
    {
        tutorialF.SetActive(false);
    }

    public void ShowTutorialQ()
    {
        tutorialQ.SetActive(true);
    }

    public void HideTutorialQ()
    {
        tutorialQ.SetActive(false);
    }
}
