using System;
using UnityEngine;

public class SpriteFromEnumController : MonoBehaviour
{
    public string EnumName;
    private Type _enumType;
    private object _currentEnumValue;
    
    public SpriteRenderer SpriteRenderer;

    void Start()
    {
        _enumType = Type.GetType(EnumName);
        if (_enumType == null || !_enumType.IsEnum)
        {
            Debug.LogError($"Could not find enum type: {EnumName}");
            return;
        }

        var enumValues = Enum.GetValues(_enumType);
        _currentEnumValue = enumValues.GetValue(0); // default: first enum value

        Show(_currentEnumValue.ToString());
    }

    void Show(string childName)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child.name == childName);
            if (child.name == childName)
            {
                SpriteRenderer = child.GetComponent<SpriteRenderer>();
            }
        }
    }

    public void ShowEnumValue(string enumValueName)
    {
        if (_enumType == null) return;

        try
        {
            var value = Enum.Parse(_enumType, enumValueName, true);
            Show(value.ToString());
        }
        catch
        {
            Debug.LogError($"Invalid enum value: {enumValueName} for type {_enumType.Name}");
        }
    }
}
