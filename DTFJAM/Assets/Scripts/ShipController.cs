using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ShipController : MonoBehaviour, IDestroyable
{
    [Header("Base pararms")]
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float speedLimit = 6f;
    [SerializeField] private float dashDistance = 4.5f;

    public bool IsDashActive {get; set;} = true;
    public bool IsDead {get; set;} 

    public Rigidbody2D Rb {get; private set;}
    private Vector2 accelerationDir;

    [Header("References"), Space(10)]
    [SerializeField] private SpriteRenderer dash;
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private ParticleSystem dead;
    [SerializeField] private BoxCollider2D dashDetector;
    [SerializeField] private CameraFollow cameraFollow;
    private SpriteRenderer shipSprite;
    public IEnumerator dashCooldown;

    private void Start()
    {
        Rb = this.GetComponent<Rigidbody2D>();
        shipSprite = this.GetComponent<SpriteRenderer>();
        accelerationDir = new Vector2(0, acceleration);
    }

    private void FixedUpdate()
    {
        if(IsDead)
            return;
        
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(Vector3.forward * rotationSpeed);
        }

        if(Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(Vector3.back * rotationSpeed);
        }

        if(Input.GetKey(KeyCode.UpArrow) && Rb.velocity.magnitude < speedLimit)
        {
            Rb.AddRelativeForce(accelerationDir, ForceMode2D.Force);
        } 

        if((Input.GetKey(KeyCode.DownArrow) && Rb.velocity.magnitude > 0.1f) || Rb.velocity.magnitude >= speedLimit)
        {
            Rb.AddForce(-Rb.velocity, ForceMode2D.Force);
        }

        if(IsDashActive)
        {
            var color = dash.color;
            dash.color = new Color(color.r, color.g, color.b, (Mathf.Sin(Time.time * 6f) + 1) / 4);
        }  
    }
    private void Update()
    {
        if(IsDead)
            return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            DestroyWithRestart();
        }
         
        if(Input.GetKeyDown(KeyCode.Space) && IsDashActive)
        {
            AudioManager.current.PlaySFX(AudioManager.current.dash);
            dashDetector.enabled = true;
            Rb.transform.localPosition += transform.TransformDirection(Vector3.up * dashDistance);
            dash.color = new Color(1, 1, 1, 0);
            dashCooldown = DashCooldown();
            StartCoroutine(dashCooldown);
            StartCoroutine(CamVFXDash());
            IsDashActive = false;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            trail.Play();
            StartCoroutine(AudioManager.current.engine.Fade(0.1f, 0.5f));
        } 

        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            trail.Stop();
            StartCoroutine(AudioManager.current.engine.Fade(0.1f, 0f));
        }
    }

    private IEnumerator InitializeRestart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void Destroy()
    {
        IsDead = true;

        Rb.velocity = Vector3.zero;
        Rb.angularVelocity = 0;

        dash.enabled = false;
        shipSprite.enabled = false;
        cameraFollow.enabled = false;

        trail.Stop();

        dead.Play();

        AudioManager.current.PlaySFX(AudioManager.current.death);

        StartCoroutine(AudioManager.current.engine.Fade(0.1f, 0f));
        StartCoroutine(AudioManager.current.music.Fade(0.25f, 0f));
    }

    public void DestroyWithRestart()
    {
        Destroy();
        StartCoroutine(InitializeRestart());
    }

    private IEnumerator CamVFXDash()
    {
        cameraFollow.Offset = new Vector3(0, 0, CameraFollow.Z_OFFSET + 5);
        yield return new WaitForSeconds(0.15f);
        cameraFollow.Offset = new Vector3(0, 0, CameraFollow.Z_OFFSET);
    }
    
    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.05f);
        dashDetector.enabled = false;
        yield return new WaitForSeconds(3f);
        IsDashActive = true;
    }

    private void ApplyRotation(Vector3 rotation)
    {
        this.transform.localEulerAngles += rotation;
        Rb.angularVelocity = 0;
    }

}
