using UnityEngine;
using UnityEngine.UI;


public class Cursor : MonoBehaviour
{
    public Image image { get; private set; }

    void Awake()
    {
        image = GetComponent<Image>();
    }
}
