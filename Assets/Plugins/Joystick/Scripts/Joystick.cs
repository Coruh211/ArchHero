using UnityEngine;
using UnityEngine.EventSystems;

namespace TPSShooter
{
    public class Joystick : MonoBehaviour
    {
        private enum JoystickUsedType
        {
            None,
            Touch,
            Mouse,
            Keyboard
        }

        [SerializeField]
        private GameObject circle, dot;
        private Vector2 touchPosition;
        private Vector2 moveDirection;
        public float maxRadius;
        [HideInInspector]
        public bool IsTouched;
        [HideInInspector]
        public float Horizontal, Vertical;
        private float speedKoef = 0.02f;
        private BaseInput _input;
        private float _lastAxisUsedTime;

        void Start()
        {
            circle.SetActive(false);
            dot.SetActive(false);
        }

        void Update()
        {
            if (null == _input)
            {
                _input = EventSystem.current.currentInputModule.input;
            }

            var touch = GetTouch();
            var type = (JoystickUsedType)touch.fingerId;

            if (type == JoystickUsedType.Keyboard)
            {
                IsTouched = true;
                touchPosition = touch.position;
                Horizontal = touchPosition.x;
                Vertical = touchPosition.y;
            }
            else
            if (type == JoystickUsedType.Mouse)
            {
                touchPosition = touch.position;

                if (!IsTouched)
                {
                    IsTouched = true;
                    circle.SetActive(true);
                    dot.SetActive(true);
                    circle.transform.position = touchPosition;
                    dot.transform.position = touchPosition;
                }
                
                MovePlayer();
            }
            else
            {
                IsTouched = false;
                circle.SetActive(false);
                dot.SetActive(false);
            }
        }

        private void MovePlayer()
        {
            dot.transform.position = touchPosition;
            dot.transform.localPosition = Vector2.ClampMagnitude(dot.transform.localPosition, maxRadius);
            moveDirection = (dot.transform.position - circle.transform.position) * speedKoef;
            Horizontal = moveDirection.x;
            Vertical = moveDirection.y;
        }

        private Touch GetTouch()
        {
            Touch touch = new Touch();

            if (_input.GetMouseButton(0))
            {
                touch.position = _input.mousePosition;
                
                touch.fingerId = (int)JoystickUsedType.Mouse;
                return touch;
            }


            if (!Mathf.Approximately(Mathf.Abs(_input.GetAxisRaw("Horizontal")) + Mathf.Abs(_input.GetAxisRaw("Vertical")), 0))
            {
                _lastAxisUsedTime = Time.time;

                touch.position =
                    new Vector2(
                        _input.GetAxisRaw("Horizontal") * speedKoef * maxRadius,
                        _input.GetAxisRaw("Vertical") * speedKoef * maxRadius);

                touch.fingerId = (int)JoystickUsedType.Keyboard;
                return touch;
            }

            touch.fingerId = (int)JoystickUsedType.None;
            return touch;
        }
    }
}