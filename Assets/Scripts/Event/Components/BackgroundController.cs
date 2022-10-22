using UnityEngine;
using System.Linq;

public class BackgroundController : MonoBehaviour
{
  public void SetImage(EventImage eventImage)
  {
    HideAllImages();
    ShowImage(eventImage);
  }

  void HideAllImages()
  {
    for(var i = 0; i < transform.childCount; i++){
      transform.GetChild(i).gameObject.SetActive(false);
    }
  }

  void ShowImage(EventImage eventImage)
  {
    transform.Find(eventImage.ToString()).gameObject.SetActive(true);;
  }
}