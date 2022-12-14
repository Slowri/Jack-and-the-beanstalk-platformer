using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class Slingshot : MonoBehaviour
{
    public int Stones;
    public int maxStones;

    public GameObject Player;
    public GameObject Projectile;
    public GameObject SpecialBean;
    public float projectileForce;
    public Transform projectilePoint;

    public bool normalBullet;

    Vector2 direction;


    private bool isAttacking;
    public bool isCharging;
    public bool isWalking;
    public bool carryingGoose;

    public bool canShoot;

    private float chargeTime;
    private float totalTime;

    private GameObject AM;
    private GameObject GM;


    //UI


    //Trajectory
    public GameObject point;
    GameObject[] points;
    public int numberOfPoints;
    public float spaceBetweenPoints;


    void Start()
    {
        AM = GameObject.Find("AnimationManager");
        GM = GameObject.Find("GM");
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Instantiate(point, projectilePoint.position, Quaternion.identity);
            points[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = GameObject.FindGameObjectWithTag("Player").GetComponent<playerAttack>().isAttacking;

        //UI
        //stoneText.text = "Stones: " + Stones;


        transform.position = Player.transform.position;
        //Slingshot rotation
        Vector2 slingshotPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - slingshotPos;
        transform.right = direction;

        if(Input.GetKeyDown(KeyCode.E))
        {
            if(normalBullet)
            {
                normalBullet = false;
            } else
            {
                normalBullet = true;
            }
        }


        //Slingshot charge
        if (Stones > 0)
        {
            if(canShoot)
            {
                if (!isAttacking)
                {
                    if (Input.GetMouseButton(1))
                    {
                        isCharging = true;
                        AM.GetComponent<AnimationManager>().isCharging = true;
                        if (normalBullet)
                        {
                            chargeTime += Time.deltaTime;
                            projectileForce = chargeTime * 3.5f + 2;
                            projectileForce = Mathf.Clamp(projectileForce, 2, 10);
                        }
                        else
                        {
                            chargeTime += Time.deltaTime;   
                            projectileForce = chargeTime * 1.5f + 2;
                            projectileForce = Mathf.Clamp(projectileForce, 2, 4);
                        }
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        GM.GetComponent<PlayerData>().removePebbles(1);
                        isCharging = false;
                        AM.GetComponent<AnimationManager>().isCharging = false; 
                        Shoot();
                        chargeTime = 0.0f;
                        projectileForce = chargeTime;
                    }
                }
            }
        }




        //Trajectory
        for (int i = 0; i < numberOfPoints; i++)
        {
            if (projectileForce > 0)
            {
                points[i].gameObject.SetActive(true);
                points[i].transform.position = PointPosition(i * spaceBetweenPoints);
            }
            else
            {
                points[i].gameObject.SetActive(false);
            }
        }
    }

    public void Shoot()
    {
        if (normalBullet == true)
        {
            GameObject newProjectile = Instantiate(Projectile, projectilePoint.position, projectilePoint.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileForce;
            newProjectile.GetComponent<Projectile>().force = projectileForce;
            Stones--;
        }
        else
        {
            GameObject newProjectile = Instantiate(SpecialBean, projectilePoint.position, projectilePoint.rotation);
            newProjectile.GetComponent<Rigidbody2D>().velocity = transform.right * projectileForce;
            Stones--;
        }
    }



    Vector2 PointPosition(float t)
    {
        Vector2 pos = (Vector2)projectilePoint.transform.position + (direction.normalized * projectileForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return pos;
    }
}