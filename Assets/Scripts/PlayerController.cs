using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = .5f;
    private Vector3 playerScreenPos;
    private CircleCollider2D cColider;
    public LayerMask blockingLayer;

    private Camera myCamera;

    void Start()
    {
        if (isLocalPlayer)
        {
            myCamera = Camera.main;
            playerScreenPos = new Vector3(Screen.width / 2, Screen.height / 2);
            cColider = GetComponent<CircleCollider2D>();

        }
    }


    void Update()
    {
        if (isLocalPlayer)
        {
            float xSpeed = Input.GetAxis("Horizontal") * speed;
            float ySpeed = Input.GetAxis("Vertical") * speed;
            Vector2 previosPosition = transform.position;
            float xPos = previosPosition.x;
            float yPos = previosPosition.y;
            Vector2 newPosition = new Vector2(xPos + xSpeed, yPos + ySpeed);
            if (CanMove(previosPosition, newPosition))
            {
                transform.position = newPosition;
                // transform.Translate(xSpeed, ySpeed, 0);
            }

            Vector3 mousePos = Input.mousePosition;


            float angle =
                180 - ((Mathf.Atan((mousePos.x - playerScreenPos.x) / (mousePos.y - playerScreenPos.y)) * 180) /
                       Mathf.PI);
            if (mousePos.y >= playerScreenPos.y)
            {
                angle += 180;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle);

            myCamera.transform.position = new Vector3(xPos, yPos, -15.0f);
            
        }
    }


    private bool CanMove(Vector2 startPosition, Vector2 endPosition)
    {
        cColider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(startPosition, endPosition, blockingLayer);
        cColider.enabled = true;

        return hit.transform == null;
    }

}
