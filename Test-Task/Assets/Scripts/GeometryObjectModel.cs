using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;

public class GeometryObjectModel : MonoBehaviour
{
    public GeometryObjectData data;

    int clickCount = 0;

    Color color;
    DateTimeOffset _lastTime;

    MeshRenderer mr;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        // Вызываем ColorChange через каждые observableTime.
        this.UpdateAsObservable()
            .Timestamp()
            .Where(x => x.Timestamp > _lastTime.AddSeconds(MainScript.Instance.gameData.observableTime))
            .Subscribe(x =>
            {
                _lastTime = x.Timestamp;
                ColorChange();
            }
            );
    }

    void ColorChange()
    {
        // Делаем выборку потенциальных цветов.
        List<Color> potentialColors = data.clicksData
            .Where(x => clickCount >= x.minClicksCount && clickCount <= x.maxClicksCount)
            .Select(x => x.Color)
            .ToList();

        // Выбираем цвет.
        switch (potentialColors.Count)
        {
            case 0:
                break;
            case 1:
                color = potentialColors[0];
                mr.material.color = color;
                return;
            default:
                Color newColor = color;

                // Выбираем случайный цвет, который не повторяется.
                while (newColor == color)
                    newColor = potentialColors[UnityEngine.Random.Range(0, potentialColors.Count)];

                color = newColor;
                mr.material.color = color;
                break;
        }
    }

    public void OnClick()
    {
        clickCount++;
        Debug.LogFormat("User clicked on {0} with {1} clicks.", gameObject.name, clickCount);
    }
}
