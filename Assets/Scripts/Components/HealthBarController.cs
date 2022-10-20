using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public GameObject Background;

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
        Debug.Log(_health);
        Background.GetComponent<Image>().fillAmount = _health / 10f;
    }

    public void SetHealth(int health)
    {
        _health = health;
        UpdateHealthbar();
    }
}
