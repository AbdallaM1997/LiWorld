using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class ThridPersonInput : MonoBehaviour
{
    public VariableJoystick leftJoystick;
    public Button attackbutton;
    public Image healthImage;
    public Button healthButton;
    public FixedTouchField touchField;
    public AudioClip footSteps;
    public float coolDown = 5f;
    public bool isAttack = false;
    public float Speed = 5f;
    public int noOfClicks;


    public Transform Obstruction;
    float zoomSpeed = 2f;
    float rotationSpeed = 0.2f;
    float rotation = 5f;
    float mouseX, mouseY;

    protected Rigidbody playerRigi;
    protected PlayerController controller;

    protected float cameraAngleX;
    protected float cameraAngleY;
    protected float cameraAngleSpeedX = 2f;
    protected float cameraAngleSpeedY = 0.4f;

    public Transform Target, Player;
    private float currentCoolDown;
    private float lastClickedTime = 0;
    private float maxComboDelay = 1;
    public bool isMoveing;
    Vector3 playerMovement;
    Transform camTransform;
    // Start is called before the first frame update
    #region Singleton 
    public static ThridPersonInput instance;
    void Awake()
    {
        instance = this;
        currentCoolDown = coolDown;
        playerRigi = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        cameraAngleY = 2f;
        Player = GetComponent<Transform>();
        Target = transform.GetChild(1).GetComponent<Transform>();
        Obstruction = Target;
    }
    #endregion
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            //Moving();
            if (!PlayerController.instance.isDie)
            {
                playerMovement =  PlayerMovement();

                playerMovement = RotateWithVeiw();


                Move();

            }
        }
    }
    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            //if (!EnemyController.instance.looking)
            if (!PlayerController.instance.isDie)
                CamController();
            ViewObstructed();
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay)
        {
            isAttack = false;
            noOfClicks = 0;
            attackbutton.interactable = true;
            attackbutton.onClick.RemoveAllListeners();
        }

        if (currentCoolDown < coolDown)
        {
            currentCoolDown += Time.deltaTime;
            healthImage.fillAmount = currentCoolDown / coolDown;
            healthButton.interactable = false;
        }
        else
        {
            if (controller.currentHealth == controller.health)
            {
                healthButton.interactable = false;
            }
            else
            {
                healthButton.interactable = true;
            }
        }
        
        if (isMoveing)
        {
            if (!PlayerController.instance.isDie)
                controller.PlayAudio(footSteps);
            else
                controller.StopAudio();
        }
        else
        {
            controller.StopAudio();
        }
    }

    private Vector3 RotateWithVeiw()
    {
        if(camTransform != null)
        {
            Vector3 dir = camTransform.TransformDirection(playerMovement);
            dir.Set(dir.x, 0, dir.z);
            return dir.normalized * playerMovement.magnitude;
        }
        else
        {
            camTransform = Camera.main.transform;
            return playerMovement;
        }
    }

    private Vector3 PlayerMovement()
    {

        Vector3 dir = Vector3.zero;

        dir.x = leftJoystick.Horizontal;
        dir.z = leftJoystick.Vertical;


        if (dir.magnitude > 1)
            dir.Normalize();

        return dir;

        //float hor = leftJoystick.Horizontal;
        //float ver = leftJoystick.Vertical;
        //playerMovement = new Vector3(-hor, 0, -ver) * Speed * Time.deltaTime;

        //Vector3 playerMovement = new Vector3(hor * Speed, playerRigi.velocity.y, ver * Speed);
        //playerRigi.velocity = playerMovement;
    }

    private void Move()
    {
        transform.Translate(playerMovement * Speed * Time.deltaTime , Space.World);

        if (playerMovement.magnitude > 0)
        {
            controller.Running(playerMovement.magnitude);
            isMoveing = true;
            Quaternion newDirction = Quaternion.LookRotation(playerMovement.normalized, Vector3.up);
            Player.rotation = Quaternion.Slerp(Player.rotation, newDirction, Time.deltaTime * rotation);
            //Player.rotation = Quaternion.AngleAxis(cameraAngleX + Vector3.SignedAngle(Vector3.forward, playerMovement.normalized + Vector3.forward * 0.0000000000001f, Vector3.up), Vector3.up);
        }
        else
        {
            controller.Running(playerMovement.magnitude);
            isMoveing = false;
        }
    }

    public void LookAt(Transform eneime)
    {
        transform.LookAt(eneime);
    }

    private void CamController()
    {
        mouseX += touchField.TouchDist.x * rotationSpeed;
        mouseY -= touchField.TouchDist.y * rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -35, 60);


        if (EnemyController.instance.GetLooking())
        {
            Camera.main.transform.LookAt(EnemyController.instance.lookTarget);
            Debug.LogWarning(EnemyController.instance.lookTarget.parent.name);
        }
        else
            Camera.main.transform.LookAt(Target);

        if (!isMoveing)
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            Player.rotation = Quaternion.Euler(0, mouseX, 0);
        }
        else
        {
            Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
        }
    }

    private void ViewObstructed()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Target.position - Camera.main.transform.position, out hit, 4.5f))
        {
            if (hit.collider.gameObject.tag != "Player" && hit.collider.gameObject.tag != "Enemy" && hit.collider.gameObject.tag != "Boss")
            {
                Obstruction = hit.transform;
                if (Obstruction.gameObject.GetComponent<MeshRenderer>() != null)
                    Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

                if (Vector3.Distance(Obstruction.position, Camera.main.transform.position) >= 3f && Vector3.Distance(Camera.main.transform.position, Target.position) >= 1.5f)
                    Camera.main.transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
            }
            else
            {
                if (Obstruction.gameObject.GetComponent<MeshRenderer>() != null)
                    Obstruction.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                if (Vector3.Distance(Camera.main.transform.position, Target.position) < 4.5f)
                    Camera.main.transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
            }
        }
    }

    private void Moving()
    {
        if (!controller.isDie)
        {
            var input = new Vector3(leftJoystick.Horizontal, 0, leftJoystick.Vertical);
            var vel = Quaternion.AngleAxis(cameraAngleX + 180, Vector3.up) * input.normalized * Speed;

            playerRigi.velocity = new Vector3(vel.x, playerRigi.velocity.y, vel.z);
            transform.rotation = Quaternion.AngleAxis(cameraAngleX + 180 + Vector3.SignedAngle(Vector3.forward, input.normalized + Vector3.forward * 0.001f, Vector3.up), Vector3.up);

            cameraAngleX += touchField.TouchDist.x * cameraAngleSpeedX * Time.deltaTime;

            cameraAngleY += touchField.TouchDist.y * cameraAngleSpeedY * Time.deltaTime;

            if (!isAttack)
            {
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * new Vector3(0, 2f, 2f), 1.5f);
                //Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * new Vector3(0, 2f, 3f);
                Camera.main.transform.rotation = Quaternion.LookRotation(this.transform.position + Vector3.up * cameraAngleY - Camera.main.transform.position, Vector3.up);

                CameraShaker.Instance.RestPositionOffset = Camera.main.transform.localPosition;
                CameraShaker.Instance.RestRotationOffset = new Vector3(Camera.main.transform.rotation.x, Camera.main.transform.localRotation.y, Camera.main.transform.localRotation.z);
                //CameraShaker.Instance.RestRotationOffset = Camera.main.transform.localRotation.eulerAngles;

            }
            if (playerRigi.velocity.magnitude > 3f)
            {
                controller.Running(playerRigi.velocity.magnitude);
                isMoveing = true;
            }
            else
            {
                controller.Running(playerRigi.velocity.magnitude);
                isMoveing = false;
            }

        }
        else
        {
            isMoveing = false;
        }
    }

    public void HealthUP()
    {
        print("Heal");
        controller.HealthPowerUp();
        currentCoolDown = 0f;
    }

    public void Attack()
    {
        isAttack = true;
        //Record time of last button click
        lastClickedTime = Time.time;
        //print(noOfClicks);
        controller.Attcking(noOfClicks+1);
        //controller.PlayAudio(sowrdslash);
        if (noOfClicks <= 1) noOfClicks++;
        else if (noOfClicks == 2)
        {
            attackbutton.interactable = false;
            controller.StopAudio();
            //noOfClicks = 0;
            //isAttack = false;
        }
        //limit/clamp no of clicks between 0 and 3 because you have combo for 3 clicks
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
    }

}
