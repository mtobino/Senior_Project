using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggleScript : MonoBehaviour
{
    public CinemachineVirtualCamera firstPersonCam;
    public CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] private GameInput gameInput;
    private Camera mainCamera;
    private int originalCullingMask;


    // Start with the 3rd person view by default
    private void Start()
    {
        mainCamera = Camera.main; 
        originalCullingMask = mainCamera.cullingMask; 
        SetCameraPriority(firstPersonCam, thirdPersonCam, true);
        gameInput.OnCameraToggle += GameInput_OnCameraToggle;
    }
    /**
     * Updated to Unity's input controller. This allows us to not check in every frame if the button is pressed but 
     * instead just listen for when it is pressed. 
     */
    private void GameInput_OnCameraToggle(object sender, System.EventArgs e)
    {
        ToggleCamera();
    }
    private void ToggleCamera()
    {
        if (firstPersonCam.Priority > thirdPersonCam.Priority)
        {
            SetCameraPriority(thirdPersonCam, firstPersonCam, false);
        }
        else
        {
            SetCameraPriority(firstPersonCam, thirdPersonCam, true);
        }
    }

    private void SetCameraPriority(CinemachineVirtualCamera activeCam, CinemachineVirtualCamera inactiveCam, bool isFirstPersonActive)
    {
        activeCam.Priority = 10; 
        inactiveCam.Priority = 1;

        if (isFirstPersonActive)
        {
            // Hide the player model in first-person
            mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("whatIsPlayerModel"));
        }
        else
        {
            // Show the player model in third-person (reset to original culling mask)
            mainCamera.cullingMask = originalCullingMask;
        }
    }
}
