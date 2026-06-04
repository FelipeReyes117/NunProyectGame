using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
	[SerializeField] private float speed;

    private myControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new myControls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();

        controls.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };

        controls.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;    
        };
        
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * speed * Time.deltaTime);
    }
}
