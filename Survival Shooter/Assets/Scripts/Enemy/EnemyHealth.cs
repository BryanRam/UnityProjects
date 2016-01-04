using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f; //after they die, they sink through the floor
    public int scoreValue = 10;
    public AudioClip deathClip; 


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> (); //finds the first instance of a particlesystem in the component's children
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead) //exit function if the enemy is dead
            return;

        enemyAudio.Play ();

        currentHealth -= amount;

        //find the position of where the enemy was hit, make that the hitParticle's position   
        hitParticles.transform.position = hitPoint;
        hitParticles.Play(); //let stuffing fly out

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        capsuleCollider.isTrigger = true; //make enemy intangible
        //since the collider is no longer physical

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    { //To turn off a GameObject you use SetActive("false")
        GetComponent <NavMeshAgent> ().enabled = false; //disable enemy's navmesh agent
        GetComponent <Rigidbody> ().isKinematic = true;
        /*when you move a collider in the scene, unity tries to recalculate all the static geometry
        because it assumes the level has changed leading to a recalculation. However, if you are
        translating a kinematic rigidbody object, it will be ignored 
        */

        isSinking = true;
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
