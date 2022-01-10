using UnityEngine;
using UnityEngine.UI;

public class BallSlider : MonoBehaviour
{
    [SerializeField] Color fullColor = Color.cyan;
    [SerializeField] Color halfColor = Color.red;
    [SerializeField] string _watchingState;
    public string watchingStateStr{get{return _watchingState;}}
    public int watchingState { get { return int.Parse(_watchingState); } }
    public Image image { get; private set; }
    public SliderState state { get; private set; }
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetState(SliderState state)
    {
        if (state == SliderState.full)
        {
            gameObject.SetActive(true);

            image.color = fullColor;
        }
        else if (state == SliderState.half)
        {
            gameObject.SetActive(true);

            image.color = halfColor;
        }
        else if (state == SliderState.disabled)
        {
            gameObject.SetActive(false);
        }

    }
}