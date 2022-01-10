using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class GateIndicator : MonoBehaviour
{
    BallSlider[] sliders;
    void Start()
    {
        sliders = GetComponentsInChildren<BallSlider>();
        foreach (var item in sliders)
        {
            item.gameObject.SetActive(false);
        }
    }


    Dictionary<int, float> summaryTable = new Dictionary<int, float>();
    void OnTriggerEnter2D(Collider2D other)
    {
        var ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            StartCoroutine(MesurementRoutine(ball));
        }
    }

    IEnumerator MesurementRoutine(Ball ball)
    {
        ball.Pause();
        var task = DataProvider.instance.circuit.BuildAndRunAsync();
        yield return new WaitUntil(() => task.ready);

        ShowResult(task.result);

        ball.Resume();

        yield return new WaitForSeconds(QPongConfig.instance.measurmentShowTime);
        UpdateState();
    }

    public void ShowResult(CircuitMeasurementResult result)
    {
        foreach(var slider in sliders)
        {
            var ts = slider.watchingStateStr;
            if(result.maxStates.Contains(ts))
            {
                slider.SetState(SliderState.full);
            }
            else
            {
                slider.SetState(SliderState.disabled);
            }
        }
    }


    public void UpdateState()
    {
        Dictionary<int, float> summary = null;

        summary = DataProvider.instance.circuit.stateSummary;


        foreach (var slider in sliders)
        {
            var th = slider.watchingState / 100;

            var sec = (slider.watchingState - th * 100) / 10;
            var mono = slider.watchingState - th * 100 - sec * 10;

            if (summary.TryGetValue(2, out var value) && value == 0.5f)
            {
                slider.SetState(SliderState.half);
            }
            else if (th == 1)
            {
                if (summary[2] == 0)
                {
                    slider.SetState(SliderState.disabled);
                    continue;
                }
                else if (slider.state != SliderState.half)
                {
                    slider.SetState(SliderState.full);
                }
            }
            else if (summary[2] == 1)
            {
                slider.SetState(SliderState.disabled);
                continue;
            }


            if (summary[1] == 0.5f)
            {
                slider.SetState(SliderState.half);
            }
            else if (sec == 1)
            {
                if (summary[1] == 0)
                {
                    slider.SetState(SliderState.disabled);
                    continue;
                }
                else if (slider.state != SliderState.half)
                {
                    slider.SetState(SliderState.full);
                }
            }
            else if (summary[1] == 1)
            {
                slider.SetState(SliderState.disabled);
                continue;
            }

            if (summary[0] == 0.5f)
            {
                slider.SetState(SliderState.half);
            }
            else if (mono == 1)
            {
                if (summary[0] == 0)
                {
                    slider.SetState(SliderState.disabled);
                    continue;
                }
                else if (slider.state != SliderState.half)
                {
                    slider.SetState(SliderState.full);
                }
            }
            else if (summary[0] == 1)
            {
                slider.SetState(SliderState.disabled);
                continue;
            }

        }

    }
}