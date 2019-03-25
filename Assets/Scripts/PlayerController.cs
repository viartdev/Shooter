using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float NormalSpeed = .3f;
    private float speed;



    private int Health { get; set; } = 100;
    private int Armor { get; set; } = 0;
    public int Ammo { get; set; }
    private bool LightOn { get; set; } = true;
    public Weapon weapon;


    private Vector3 playerScreenPos;
    private CircleCollider2D cColider;
    public LayerMask blockingLayer;
    private GUI myGUI;

    private float NextLightTime;
    private float PlayerSize = .6f;
    private Camera myCamera;

    void Start()
    {
        if (isLocalPlayer)
        {
            Weapon gun = new Weapon();
            gun.weaponType = Weapon.WeaponType.USP;
            gun.InitWeapon();
            AcceptWeapon(gun);
            myGUI = GameObject.Find("MainCamera").GetComponent<GUI>();
            speed = NormalSpeed;
            myCamera = Camera.main;
            playerScreenPos = new Vector3(Screen.width / 2, Screen.height / 2);
            cColider = GetComponent<CircleCollider2D>();
            

        }
    }


    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = NormalSpeed / 2;
            }
            else
            {
                speed = NormalSpeed;
            }
            float xSpeed = Input.GetAxis("Horizontal") * speed;
            float ySpeed = Input.GetAxis("Vertical") * speed;
            Vector2 previosPosition = transform.position;
            float xPos = previosPosition.x;
            float yPos = previosPosition.y;
            
            Vector2 newXPosition = new Vector2(xPos + xSpeed, yPos);
            Vector2 newYPosition = new Vector2(xPos, yPos + ySpeed);

            if (!CanMove(previosPosition, newXPosition))
            {
                xSpeed = 0;
            }
            if (!CanMove(previosPosition, newYPosition))
            {
                ySpeed = 0;
            }

            transform.position = new Vector2(xPos + xSpeed, yPos + ySpeed);

            if (Input.GetKey(KeyCode.F)&&Time.time>NextLightTime)
            {
                NextLightTime = Time.time + 0.3f;
                TurnLight();
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
            CheckHealth();
            ChangeUIStats();
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            RespawnPlayer();
        }
    }

    private void ChangeUIStats()
    {
        myGUI.ChangeUI(GUI.Stat.Health, Health.ToString());
        myGUI.ChangeUI(GUI.Stat.Armor, Armor.ToString());
        myGUI.ChangeUI(GUI.Stat.Ammo, Ammo.ToString());
        myGUI.ChangeUI(GUI.Stat.Weapon, weapon.weaponType.ToString());

    }

    private void RespawnPlayer()
    {
        Vector2 newPosition = GameObject.Find("BoardLayout").GetComponent<BoardManager>().GetRandomPosition();
        this.transform.position = newPosition;
        Health = 100;
    }



    public void GetDamage(int damage)
    {
        if (isLocalPlayer)
        {
            Health = Health - damage;
        }
    }

    public void AcceptWeapon(Weapon gun)
    {
        weapon = gun;
        Ammo = weapon.Ammo;
    }


    private bool CanMove(Vector2 startPosition, Vector2 endPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPosition, endPosition - startPosition, PlayerSize/2, blockingLayer);

        return hit.transform == null;
    }

    private void TurnLight()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; i++)
        {
            GameObject light = transform.GetChild(i).gameObject;
            if (light.tag == "PlayerLight")
            {
                if (LightOn)
                    light.SetActive(false);
                else

                    light.SetActive(true);

            }
        }

        LightOn = !LightOn;
    }

}