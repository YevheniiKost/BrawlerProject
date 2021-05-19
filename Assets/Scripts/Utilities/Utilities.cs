using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class MyUtilities 
{
    public static class UI
    {
        public static void MoveUp(Transform window, float offcet, float duration)
        {
            window.DOBlendableLocalMoveBy(Vector3.up * offcet, duration).SetEase(Ease.InCubic);
        }

        public static void MoveDown(Transform window, float offcet, float duration)
        {
            window.DOBlendableLocalMoveBy(Vector3.up * -offcet, duration).SetEase(Ease.InCubic);
        }

        public static void MoveLeft(Transform window, float offcet, float duration)
        {
            window.DOBlendableLocalMoveBy(Vector3.right * -offcet, duration).SetEase(Ease.InCubic);
        }

        public static void MoveRight(Transform window, float offcet, float duration)
        {
            window.DOBlendableLocalMoveBy(Vector3.right * offcet, duration).SetEase(Ease.InCubic);
        }
    }
}
