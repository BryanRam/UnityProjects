using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20; //damage bullets are inflicting
    public float timeBetweenBullets = 0.15f; //cooldown time
    public float range = 100f; //range of the bullets


    float timer; //keeps the cooldown time intact
    Ray shootRay; //use to raycast out and figure out whatever we hit with those bullets
    RaycastHit shootHit; //return to us info on whatever it is we hit

    Ray ricochetRay;
    RaycastHit ricochetHit;

    int shootableMask; //layer for only hitting shootable things
    ParticleSystem gunParticles; 
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f; //how long these effects will be viewable


    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable"); //returns the number of the shootable layer and assigns it to shootableMask
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop (); //if the particles are still playing, stop them and play again
        gunParticles.Play ();

        gunLine.enabled = true; //turn on line renderer element
        gunLine.SetPosition (0, transform.position); //start position of the line

        shootRay.origin = transform.position;//start at the tip of the gun
        shootRay.direction = transform.forward; //we generally treat the z axis as forward

        /*Fire a ray forward 100 units. If it hits something, whatever it hits will come 
         *back to us and that's the other end of the line, thus the line is drawn. If it doesn't
         *hit something, we still want to fire. It's just way out there, so draw a really long line
         */
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask)) //fire the ray forward
        {/*if the raycast hits something, output results in a variable shootHit 
          *store in enemyHealth 
          */
            ricochetRay.origin = shootHit.transform.position;
            if (shootHit.transform.tag != "Enemy")
            {
               
                ricochetRay.direction = Vector3.Reflect(shootHit.transform.position, shootHit.normal);
                gunLine.SetPosition(1, shootHit.point); //2nd or end point of the line
                if (Physics.Raycast(ricochetRay, out ricochetHit, range, shootableMask))
                {
                   
                    EnemyHealth enemyHealth1 = ricochetHit.collider.GetComponent<EnemyHealth>();
                    if (enemyHealth1 != null)//if you hit something without an EnemyHealth script, enemyHealth will be null
                    {
                        enemyHealth1.TakeDamage(damagePerShot, ricochetHit.point); //subtract damage, and show exactly where enemy was hit
                    }
                    gunLine.SetPosition(2, ricochetHit.point); //2nd or end point of the line
                }
            }

            else
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)//if you hit something without an EnemyHealth script, enemyHealth will be null
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point); //subtract damage, and show exactly where enemy was hit
                }
                gunLine.SetPosition(1, shootHit.point); //2nd or end point of the line
                gunLine.SetPosition(2, shootHit.point);
            }
        }
        else //if you don't hit anything, end point of line = start of ray + direction (0,0,1) *  100f
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
            gunLine.SetPosition(2, shootRay.origin + shootRay.direction * range);
        }
    }
}
