using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public GameObject[] Backgrounds;
    public GameObject ArmourCover;
    public GameObject ArmourText;

    int _health = 5;
    int _armour = 0;

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

    void UpdateArmour()
    {
        if (_armour > 0)
        {
            ArmourText.GetComponent<TextMeshProUGUI>().text = _armour.ToString();
            ArmourCover.SetActive(true);
        }
        else
            ArmourCover.SetActive(false);
    }

    public void SetHealthAndArmour(int health, int armour)
    {
        _health = health;
        _armour = armour;
        UpdateHealthbar();
        UpdateArmour();
    }
}
