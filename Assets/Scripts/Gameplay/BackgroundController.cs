using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.Core;
using UnityEngine.UI;
using DG.Tweening;


namespace Gameplay.Main
{
    public class BackgroundController : MonoBehaviour, IBackgroundController
    {
        [SerializeField] private Image backgroundColor;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color[] color;

        public void ChangeColorByLevel(uint level)
        {
            ChangeColor(color[level % color.Length]);
        }

        public void ChangeColor(Color newColor)
        {
            backgroundColor.DOColor(newColor, 0.3f);
        }

        public void ResetToDefaultColor()
        {
            ChangeColor(defaultColor);
        }
    }
}