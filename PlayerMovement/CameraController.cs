using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity;

    private Transform parent;
    void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Rotate();
    }

    private void Rotate() 
    {
        float mouseX = Input.GetAxis("Mouse X") * (mouseSensitivity*100) * Time.deltaTime;
        parent.Rotate(Vector3.up, mouseX);
    }
}
