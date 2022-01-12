using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using QiskitPlugin.Utility;
using QiskitPlugin;

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
        var task = DataProvider.instance.circuitManager.BuildAndRunAsync();
        yield return new WaitUntil(() => task.ready);

        ShowResult(task.result);

        ball.Resume();

        yield return new WaitForSeconds(QPongConfig.instance.measurmentShowTime);
        UpdateState();
    }

    public void ShowResult(CircuitMeasurementResult result)
    {
        foreach (var slider in sliders)
        {
            var ts = slider.watchingStateStr;
            if (result.maxStates.Contains(ts))
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
        StartCoroutine(UpdateRoutine());
    }

    IEnumerator UpdateRoutine()
    {
        Dictionary<string, float> summary = null;
        while (DataProvider.instance.circuitManager.updatedToHead)
        {
            yield return null;
            summary = DataProvider.instance.circuitManager.stateSummary;
        }
        if(summary==null)
        {
            summary=DataProvider.instance.circuitManager.stateSummary;
        }


        foreach (var slider in sliders)
        {
            if (summary.TryGetValue(slider.watchingStateStr, out var val))
            {
                if (val == 1)
                {
                    slider.SetState(SliderState.full);
                }
                else
                {
                    slider.SetState(SliderState.half);
                }
            }
            else
            {
                slider.SetState(SliderState.disabled);
            }
        }
    }
}