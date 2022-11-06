using UnityEngine;

public class InitiatorController : MonoBehaviour
{
    public GameObject Instantiate(GameObject prefab)
    {
        return Object.Instantiate(prefab);
    }

    public void Destroy(GameObject obj)
    {
        Object.Destroy(obj);
    }
}
