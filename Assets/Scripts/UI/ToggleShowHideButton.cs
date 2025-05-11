using UnityEngine;
using UnityEngine.EventSystems;

public class ToggleShowHideButton : MonoBehaviour, IPointerClickHandler
{
    public bool StartVisible = false;
    public GameObject Target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (StartVisible) {
            Target.SetActive(true);
        } else {
            Target.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Target.SetActive(!Target.activeSelf);
    }
}
