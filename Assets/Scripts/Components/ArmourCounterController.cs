using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourCounterController : MonoBehaviour
{
  public GameObject Text;

  public void Init(int armour)
  {
    SetArmour(armour);
  }
  
  public void SetArmour(int armour)
  {
    Text.GetComponent<UnityEngine.UI.Text>().text = armour.ToString();
  }
}
