using System;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingHandler : MonoBehaviour
{
    [SerializeField] private List<BottomButton> _bottomButtons;

    public void Initialize()
    {
        foreach (var button in _bottomButtons)
        {
            button.Initialize(OnButtonClicked);
        }
    }

    public void Dispose()
    {
        foreach (var button in _bottomButtons)
        {
            button.Dispose();
        }
    }

    private void OnButtonClicked(string contentName)
    {
    }
}
