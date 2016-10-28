using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SetActiveUIButton : MonoBehaviour
{

  public GameObject eventSys;
  public GameObject activeButton;

  private void OnEnable()
  {
    StartCoroutine(HighlightButton());
  }

  private IEnumerator HighlightButton()
  {
    eventSys.GetComponent<EventSystem>().SetSelectedGameObject(null);
    yield return null;
    eventSys.GetComponent<EventSystem>().SetSelectedGameObject(activeButton);
  }
}