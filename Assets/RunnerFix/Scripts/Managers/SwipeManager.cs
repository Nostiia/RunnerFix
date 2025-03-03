using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RunnerFix.Scripts
{
    public static class SwipeManager
    {
        private static Vector2 _touchStartPos;
        private static bool _swipeDetected = false;
        private static float swipeThreshold = 20f;

        public static bool DetectSwipeUp()
        {
            return DetectSwipe(Vector2.up);
        }

        public static bool DetectSwipeDown()
        {
            return DetectSwipe(Vector2.down);
        }

        public static bool DetectSwipeLeft()
        {
            return DetectSwipe(Vector2.left);
        }

        public static bool DetectSwipeRight()
        {
            return DetectSwipe(Vector2.right);
        }
        private static bool DetectSwipe(Vector2 direction)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    _touchStartPos = touch.position;
                    _swipeDetected = false;
                }
                else if (touch.phase == TouchPhase.Ended && !_swipeDetected)
                {
                    Vector2 swipeDelta = touch.position - _touchStartPos;

                    if (swipeDelta.magnitude > swipeThreshold)
                    {
                        Vector2 swipeDirection = swipeDelta.normalized;

                        if (Vector2.Dot(swipeDirection, direction) > 0.7f) // Ensures clear directional detection
                        {
                            _swipeDetected = true;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }

}
