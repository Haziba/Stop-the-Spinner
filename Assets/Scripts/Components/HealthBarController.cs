using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public GameObject[] Backgrounds;

    int _health = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void UpdateHealthbar()
    {
        Debug.Log("Update health bar - " + _health);
        for (var i = 0; i < Backgrounds.Length; i++)
        {
            if (_health >= 10 * (i + 1))
                Backgrounds[i].GetComponent<Image>().fillAmount = 1;
            else if (_health <= 10 * (i))
                Backgrounds[i].GetComponent<Image>().fillAmount = 0;
            else
                Backgrounds[i].GetComponent<Image>().fillAmount = (_health % 10) / 10f;
        }
    }

    public void SetHealth(int health)
    {
        _health = health;
        UpdateHealthbar();
    }
}
