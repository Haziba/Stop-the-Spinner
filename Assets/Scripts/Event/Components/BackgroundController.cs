using UnityEngine;
using System.Linq;

public class BackgroundController : MonoBehaviour
{
  public void SetImage(BackgroundName backgroundName)
  {
    HideAllImages();
    ShowImage(backgroundName);
  }

  void HideAllImages()
  {
    for(var i = 0; i < transform.childCount; i++){
      transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowImage(BackgroundName backgroundName)
  {
    transform.Find(backgroundName.ToString()).gameObject.SetActive(true);;
  }
}