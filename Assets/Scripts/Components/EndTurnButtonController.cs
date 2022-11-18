using System;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonController : MonoBehaviour
{
  public GameObject Button;
  public event EventHandler OnClicked;

  public void Start()
  {
    Button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(ButtonClicked);
  }

  void ButtonClicked()
  {
    OnClicked?.Invoke(null, null);
  }

  public void DisableButton()
  {
    Button.GetComponent<UnityEngine.UI.Button>().enabled = false;
    var colour = Button.GetComponent<Image>().color;
    colour.a = 0.5f;
    Button.GetComponent<Image>().color = colour;
  }

  public void EnableButton()
  {
    Button.GetComponent<UnityEngine.UI.Button>().enabled = true;
    var colour = Button.GetComponent<Image>().color;
    colour.a = 1f;
    Button.GetComponent<Image>().color = colour;
  }
}