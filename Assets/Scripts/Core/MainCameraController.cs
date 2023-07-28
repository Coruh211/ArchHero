using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private float timeMove;
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private Vector3 cameraRotation;

        private Transform _target;

        private void Start()
        {
            FindTarget();
            SetTransform();
        }

        private void FindTarget()
        {
            _target = FindObjectOfType<LevelController>().transform;
        }

        private void SetTransform()
        {
            transform.DOMove(_target.position + cameraOffset, timeMove);
            transform.DORotate(cameraRotation, timeMove);
        }
    }
}

    