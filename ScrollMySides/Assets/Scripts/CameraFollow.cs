using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Controller2D target;
    public Vector2 focusAreaSz;

    public float lookAheadDistanceX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;
    public float verticalOffset;

    FocusArea focusArea;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped;

    public float shakeTimer;
    public float shakeAmount;

    // Use this for initialization
    void Start()
    {
        focusArea = new FocusArea(target.collider.bounds, focusAreaSz);
    }

    private void Update()
    {
        if(shakeTimer >= 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;
            Transform t = gameObject.GetComponent<Camera>().transform;
            t.position = new Vector3(t.position.x + ShakePos.x, t.position.y + ShakePos.y, t.position.z);

            shakeTimer -= Time.deltaTime;
        }

    }

    public void ShakeCamera(float shakePwr, float shakeDur)
    {
        shakeTimer = shakeDur;
        shakeAmount = shakePwr;
    }

    private void LateUpdate()
    {
        focusArea.Update(target.collider.bounds);
        Vector2 focusPos = focusArea.centre + Vector2.up * verticalOffset;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) == Mathf.Sign(focusArea.velocity.x) && Input.GetAxisRaw("Horizontal") != 0)
            {
                targetLookAheadX = lookAheadDirX * lookAheadDistanceX;
                lookAheadStopped = false;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistanceX - currentLookAheadX) / 4f;
                    lookAheadStopped = true;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPos.y = Mathf.SmoothDamp(transform.position.y, focusPos.y, ref smoothVelocityY, verticalSmoothTime);
        focusPos += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPos + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSz);
    }

    struct FocusArea
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea(Bounds targetBounds, Vector2 sz)
        {
            left = targetBounds.center.x - sz.x / 2;
            right = targetBounds.center.x + sz.x / 2;
            top = targetBounds.min.y + sz.y;
            bottom = targetBounds.min.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }
        
        public void Update(Bounds targetBounds)
        {
            float ShiftX = 0;
            if (targetBounds.min.x < left)
            {
                ShiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                ShiftX = targetBounds.max.x - right;
            }
            left += ShiftX;
            right += ShiftX;

            float ShiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                ShiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                ShiftY = targetBounds.max.y - top;
            }
            top += ShiftY;
            bottom += ShiftY;

            velocity = new Vector2(ShiftX, ShiftY);
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }
    }

	
}
