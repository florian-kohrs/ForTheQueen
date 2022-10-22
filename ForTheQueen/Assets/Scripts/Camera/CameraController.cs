using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector2 minCameraPos;

    protected Vector2 maxCameraPos;

    protected Vector2 screenSize;

    protected const float PROGRESS_TO_BORDER_TO_START_MOUSE_MOVEMENT = 0.15f;

    public float minCameraSpeed = 1;

    public float maxCameraSpeed = 5;

    void Start()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        maxCameraPos = new Vector2(HexagonWorld.GetWorldTotalWidth, HexagonWorld.GetWorldTotalHeigth);
    }

    void Update()
    {
        if (!GameManager.CanCameraMove)
            return;

        transform.position += GetCurrentCameraSpeed() * Time.deltaTime;
        ConfineCameraInMapBounds();
    }

    protected void ConfineCameraInMapBounds()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minCameraPos.x, maxCameraPos.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minCameraPos.y, maxCameraPos.y)
            );
    }

    protected Vector3 GetCurrentCameraSpeed()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        return new Vector3(
            GetAxisSpeed((int)screenSize.x, (int)mouseScreenPos.x), 
            0, 
            GetAxisSpeed((int)screenSize.y, (int)mouseScreenPos.y));
    }

    protected float GetAxisSpeed(int axisSize, int axisPos)
    {
        float progress = axisPos / (float)axisSize;
        progress -= 0.5f;
        progress *= 2;
        ///progress is now between -1 and 1 where -1 means left border and 1 means right border. 0 is center
        if(Mathf.Abs(progress) >= (1- PROGRESS_TO_BORDER_TO_START_MOUSE_MOVEMENT))
        {
            ///0 if the mouse is just starting to move, 1 is the mouse is at the border completly
            float progressFromStartMoveToBorder = Mathf.InverseLerp(1 - PROGRESS_TO_BORDER_TO_START_MOUSE_MOVEMENT, 1, Mathf.Abs(progress));
            return Mathf.Lerp(minCameraSpeed, maxCameraSpeed, progressFromStartMoveToBorder) * Mathf.Sign(progress);
        }
        else
        {
            return 0;
        }    
        
    }

}
