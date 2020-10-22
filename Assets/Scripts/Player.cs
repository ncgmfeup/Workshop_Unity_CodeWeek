using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float impulse = 5.5f;
    [SerializeField]
    private float speed = .05f;
    [SerializeField]
    private GameObject gameOverScreen = null;

    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private AudioClip coinSound = null;

    [SerializeField]
    private GameObject openDoor = null;
    [SerializeField]
    private GameObject finishLevelScreen = null;

    private bool canJump = true;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Horizontal"))
        {
            float horDir = Input.GetAxisRaw("Horizontal");

            if(horDir < 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }


            transform.Translate(Vector2.right * horDir * speed, Space.World);

            animator.SetBool("walking", true);
        }
        else
        {
            animator.SetBool("walking", false);
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.AddForce(Vector2.up*impulse, ForceMode2D.Impulse);

            canJump = false;

            animator.SetBool("jumping", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Platform"))
        {
            canJump = true;

            animator.SetBool("jumping", false);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if(transform.position.y > collision.transform.position.y)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                gameOverScreen.SetActive(true);

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            audioSource.clip = coinSound;
            audioSource.Play();
            Destroy(collision.gameObject);

            openDoor.SetActive(true);
        }
        else if (collision.CompareTag("Door"))
        {
            finishLevelScreen.SetActive(true);
        }
    }
}
