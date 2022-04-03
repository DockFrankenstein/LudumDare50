using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MoveUIBehind : MonoBehaviour
    {
        Vector2 rotation;
        Quaternion rot;

        [SerializeField] Transform target;
        [SerializeField] float speed = 1f;
        [SerializeField] float amplitude = 1f;
        [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        RectTransform trans;

        private void Awake()
        {
            rotation = transform.eulerAngles;
            trans = GetComponent<RectTransform>();
        }

        //I hate in every way imaginable
        private void Update()
        {
            Vector2 difference = new Vector2(Mathf.DeltaAngle(rotation.x, target.eulerAngles.x),
                Mathf.DeltaAngle(rotation.y, target.eulerAngles.y));

            Vector2 move = difference * amplitude;

            Vector2 prevRotation = rotation;
            rotation += difference * Time.deltaTime * speed;

            if (prevRotation.x - rotation.x <= 0)
                rotation.x = target.eulerAngles.x;

            if (prevRotation.y - rotation.y <= 0)
                rotation.y = target.eulerAngles.y;

            trans.localPosition = new Vector3(-move.y, move.x, trans.localPosition.z);
        }
    }
}