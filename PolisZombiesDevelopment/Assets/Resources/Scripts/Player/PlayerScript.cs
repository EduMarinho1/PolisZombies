using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    //For debuging purposes. Can delete this
    private float tookDamageTime;
    private float firstTime;
    private float secondTime;

    // Handling miscelaneous
    public GameObject spawnerManager;
    public bool hasGoldenKnife;
    public bool tookExpDamage;
    Image reloadingSprite;

    // Handling colas
    public List<string> playerColas;
    public int criticalChance; // criticalChance/100
    public float criticalMultiplier;
    private float regenMultiplierTime; // The less, the faster is to regen
    private float reloadMultiplier;
    private int moneyFactor;
    public GameObject cola1;
    public GameObject cola2;
    public GameObject cola3;

    // Handling misc
    private bool tookExplosionDamage = false;
    private bool canMove = true;
    public bool beingPulled;
    public Sprite playerSprite;
    public Sprite slowedPlayerSprite;
    public float moveSpeed;
    private bool slowed = false;
    private float slowedSpeed;
    private float normalSpeed;
    Vector2 movement;
    Vector2 mousePos;
    public Rigidbody2D rb;
    public Camera cam;
    public int money = 0;

    // Handling shots
    public Transform firePoint;
    private float bulletForce = 20;
    public float timeBetweenShots = 1f;
    private float timeSinceLastShot = 0f;

    // Handling health
    public GameObject healthManager;
    public int maxHealth = 3;
    public int health = 3;
    public float regenerationDelay = 3f;
    public float lastDamageTime = -1f;
    public bool healingState = false;

    // Handling HUD
    public GameObject hudSecondWeaponDisplayPrefab;
    public GameObject hudEquippedWeaponDisplayPrefab;
    public GameObject hudASBulletPrefab;
    public GameObject hudASScopePrefab;
    public GameObject hudASBarrelPrefab;
    public GameObject hudASMagazinePrefab;
    public GameObject hudASBagPrefab;
    public GameObject hudAEBulletPrefab;
    public GameObject hudAEScopePrefab;
    public GameObject hudAEBarrelPrefab;
    public GameObject hudAEMagazinePrefab;
    public GameObject hudAEBagPrefab;

    // Handling weapons and ammo
    public GameObject equippedWeaponPrefab;
    public GameObject secondWeaponPrefab;
    public int totalPlayerAmmo;
    public int totalPlayerMagazineAmmo;
    public int playerAmmo;
    public int secondTotalPlayerAmmo;
    public int secondTotalPlayerMagazineAmmo;
    public int secondPlayerAmmo;
    public float timeSinceLastChangeOfWeapon;
    public float timeBetweenChangeOfWeapon = 0.8f;
    public GameObject saveWeaponWhenChange; // Its from the current equipped weapon
    public int saveTotalPlayerAmmo; // Its from the current equipped weapon
    public int savePlayerAmmo; // Its from the current equipped weapon
    public int saveTotalPlayerMagazineAmmo; // Its from the current equipped weapon
    public int reloadTime;
    private bool reloading;

    //Handling sprinting
    public bool isSprinting;
    public Image StaminaBar;
    public float stamina;
    public float maxStamina;
    public float runCost;
    public float staminaRechargeRate;
    public float sprintRegenDelay;
    public float sprintRegenTimeRate;

    void Start ()
    {
        sprintRegenTimeRate = 4f;
        staminaRechargeRate = 20f;
        sprintRegenDelay = 2f;
        runCost = 60f;
        maxStamina = 100f;
        stamina = maxStamina;
        criticalChance = 10;
        criticalMultiplier = 3;
        StaminaBar = GameObject.Find("SprintBar").GetComponent<Image>();
        reloadingSprite = GameObject.Find("ReloadingHUD").GetComponent<Image>();
        Debug.Log("reloading hud" + GameObject.Find("ReloadingHUD"));
        Color reloadingSpriteColor = reloadingSprite.color;
        reloadingSpriteColor.a = 0f;
        reloadingSprite.color = reloadingSpriteColor;
        tookExpDamage = false;
        GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 8f;
        hasGoldenKnife = false;
        spawnerManager = GameObject.Find("SpawnerManager");
        moneyFactor = 1;
        reloadMultiplier = 1;
        regenMultiplierTime = 1f;
        playerColas = new List<string> {};
        beingPulled = false;
        normalSpeed = 4.5f;
        slowedSpeed = normalSpeed * 0.8f;
        moveSpeed = normalSpeed;
        maxHealth = 3;
        health = maxHealth;
        money = 100;
        //totalPlayerAmmo = equippedWeaponPrefab.GetComponent<WeaponScript>().totalAmmo;
        //totalPlayerMagazineAmmo = equippedWeaponPrefab.GetComponent<WeaponScript>().totalMagazineAmmo;
        //playerAmmo = equippedWeaponPrefab.GetComponent<WeaponScript>().totalMagazineAmmo;
        //timeBetweenShots = equippedWeaponPrefab.GetComponent<WeaponScript>().GetTimeBetweenShots();
        //reloadTime = equippedWeaponPrefab.GetComponent<WeaponScript>().reloadTime;
        ////////
        totalPlayerAmmo = 0;
        playerAmmo = 0;
        totalPlayerMagazineAmmo = 0;
        secondPlayerAmmo = 0;
        secondTotalPlayerAmmo = 0;
        secondTotalPlayerMagazineAmmo = 0;
        ///////
        reloading = false;

        StartCoroutine(SprintRegen());

        updateHUDWeaponsAndAccessories();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close the application
            Application.Quit();
        }
        // Handling Shots
        timeSinceLastShot = timeSinceLastShot + Time.deltaTime;

        if(Input.GetButton("Fire1") && timeSinceLastShot >= timeBetweenShots && playerAmmo > 0 && !reloading)
        {
            Debug.Log("timebetweenShots player " + timeBetweenShots);
            equippedWeaponPrefab.GetComponent<WeaponScript>().Shot(ref playerAmmo, firePoint);
            timeSinceLastShot = 0;
            StartCoroutine(WaitToReload());            
        }

        // When changing weapons
        timeSinceLastChangeOfWeapon = timeSinceLastChangeOfWeapon + Time.deltaTime;

        if(Input.GetKey(KeyCode.Y) && timeSinceLastChangeOfWeapon >= timeBetweenChangeOfWeapon && secondWeaponPrefab.name != "EmptyGun" && !reloading)
        {
            saveWeaponWhenChange = equippedWeaponPrefab;
            saveTotalPlayerAmmo = totalPlayerAmmo;
            savePlayerAmmo = playerAmmo;
            saveTotalPlayerMagazineAmmo = totalPlayerMagazineAmmo;

            equippedWeaponPrefab = secondWeaponPrefab;
            totalPlayerAmmo = secondTotalPlayerAmmo;
            playerAmmo = secondPlayerAmmo;
            totalPlayerMagazineAmmo = secondTotalPlayerMagazineAmmo;

            secondWeaponPrefab = saveWeaponWhenChange;
            secondTotalPlayerAmmo = saveTotalPlayerAmmo;
            secondPlayerAmmo = savePlayerAmmo;
            secondTotalPlayerMagazineAmmo = saveTotalPlayerMagazineAmmo;

            reloadTime = equippedWeaponPrefab.GetComponent<WeaponScript>().reloadTime;
            timeBetweenShots = equippedWeaponPrefab.GetComponent<WeaponScript>().timeBetweenShots;

            updateHUDWeaponsAndAccessories();


            timeSinceLastChangeOfWeapon = 0;
        }

        // Reloading weapon
        if (Input.GetKey(KeyCode.R) && playerAmmo < totalPlayerMagazineAmmo && totalPlayerAmmo > 0 && !reloading)
        {
            Debug.Log("Total player magazine ammo: Player Script " + totalPlayerMagazineAmmo);
            StartCoroutine(Reloading());
        }

        // Handling health
        if(health < maxHealth && lastDamageTime >= 0 && Time.time - lastDamageTime >= regenerationDelay * regenMultiplierTime && !healingState)
        {
            healingState = true;
            Debug.Log("regenerating = true" + Time.time);
            firstTime = Time.time;
            StartCoroutine(Regenerating());
        }

        // Handling misc
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private IEnumerator WaitToReload()
    {
        yield return new WaitForSeconds(0.1f);
        if(playerAmmo == 0 && totalPlayerAmmo > 0 && !reloading)
        {
            StartCoroutine(Reloading());
        }
    }

    private IEnumerator Reloading()
    {
        Color reloadingSpriteColor = reloadingSprite.color;
        reloadingSpriteColor.a = 1f;
        reloadingSprite.color = reloadingSpriteColor;
        reloading = true;
        yield return new WaitForSeconds(reloadTime * reloadMultiplier);
        equippedWeaponPrefab.GetComponent<WeaponScript>().Reload(ref playerAmmo, ref totalPlayerAmmo);
        reloadingSpriteColor.a = 0f;
        reloadingSprite.color = reloadingSpriteColor;
        reloading = false;
    }

    // Handling health
    private IEnumerator Regenerating()
    {
        yield return new WaitForSeconds(regenerationDelay * regenMultiplierTime);

        while(health < maxHealth && healingState == true && Time.time - lastDamageTime >= regenerationDelay * regenMultiplierTime)
        {
            secondTime = Time.time;
            Debug.Log("damaged at " + tookDamageTime + " before regen " + firstTime + " after regen " + secondTime);
            health = health + 1;
            healthManager.GetComponent<HUDHealthScript>().Heal(1);
            yield return new WaitForSeconds(regenerationDelay * regenMultiplierTime);
        }

        healingState = false;
    }

    //Sprint regen
    private IEnumerator SprintRegen()
    {
        while (true)
        {
            //while(stamina < maxStamina && !isSprinting) 
            //{
                stamina = stamina + staminaRechargeRate;
                if(stamina > maxStamina) {stamina = maxStamina;}
                StaminaBar.fillAmount = stamina / maxStamina;
                yield return new WaitForSeconds(sprintRegenTimeRate);
            //}

            //yield return new WaitForSeconds(sprintRegenDelay);
        }           
    }

    void FixedUpdate()
    {
        if(canMove && Input.GetKey(KeyCode.Mouse1) && stamina > 0) //Movement with sprint
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime * 1.5f);
            stamina = stamina - runCost * Time.deltaTime;
            if(stamina < 0) {stamina = 0;}
            StaminaBar.fillAmount = stamina / maxStamina;
        }
        // Handling movement and camera position
        else if(canMove) //Movement without sprint
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    // Handling health
    public void TakeDamage(int i)
    {
        StopCoroutine(Regenerating());
        tookDamageTime = Time.time;
        lastDamageTime = Time.time;
        //StopCoroutine(Regenerating());
        healthManager.GetComponent<HUDHealthScript>().TakeDamage(i);
        health = health - i;
        Debug.Log(health);
        gameObject.GetComponent<AudioSource>().Play();

        if (health <= 0)
        {
            if(spawnerManager.GetComponent<WavesManagerScript>().thisWave == 21) {GameOverDataScript.setEndGameData(1);}
            else {GameOverDataScript.setEndGameData(0);}
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void Slow()
    {
        Debug.Log("slow called");
        if (slowed == false) {StartCoroutine(SlowPlayer());}
    }

    IEnumerator SlowPlayer()
    {
        slowed = true;
        Debug.Log("slowed");
        moveSpeed = slowedSpeed;
        GetComponent<SpriteRenderer>().sprite = slowedPlayerSprite;
        yield return new WaitForSeconds(3);
        moveSpeed = normalSpeed;
        slowed = false;
        GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

    public void TemporarilyRemoveMovement(float moveRestraintTime)
    {
        StartCoroutine(TemporarilyRemoveMovementRoutine(moveRestraintTime));
    }

    IEnumerator TemporarilyRemoveMovementRoutine(float moveRestraintTime)
    {
        canMove = false;
        yield return new WaitForSeconds(moveRestraintTime);
        canMove = true;
    }

    public void updateHUDWeaponsAndAccessories()
    {
        hudEquippedWeaponDisplayPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<SpriteRenderer>().sprite);
        hudSecondWeaponDisplayPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<SpriteRenderer>().sprite);
        hudASBulletPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<WeaponAcessoriesScript>().bulletPrefabAccessory.GetComponent<SpriteRenderer>().sprite);
        hudASScopePrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<WeaponAcessoriesScript>().scopePrefab.GetComponent<SpriteRenderer>().sprite);
        hudASBarrelPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<WeaponAcessoriesScript>().barrelPrefab.GetComponent<SpriteRenderer>().sprite);
        hudASMagazinePrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<WeaponAcessoriesScript>().magazinePrefab.GetComponent<SpriteRenderer>().sprite);
        hudASBagPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(secondWeaponPrefab.GetComponent<WeaponAcessoriesScript>().bagPrefab.GetComponent<SpriteRenderer>().sprite);
        hudAEBulletPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().bulletPrefabAccessory.GetComponent<SpriteRenderer>().sprite);
        hudAEScopePrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().scopePrefab.GetComponent<SpriteRenderer>().sprite);
        hudAEBarrelPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().barrelPrefab.GetComponent<SpriteRenderer>().sprite);
        hudAEMagazinePrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().magazinePrefab.GetComponent<SpriteRenderer>().sprite);
        hudAEBagPrefab.GetComponent<HUDSpriteDisplayScript>().changeSpriteDisplay(equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().bagPrefab.GetComponent<SpriteRenderer>().sprite);
    }

    public void AddCola(string cola)
    {
        Debug.Log("cola = " +  cola);
        if(cola == "Jug")
        {
            maxHealth = maxHealth + 1;
            GameObject.Find("HealthManager").GetComponent<HUDHealthScript>().playerMaxHealth =  GameObject.Find("HealthManager").GetComponent<HUDHealthScript>().playerMaxHealth * 2;
            healingState = true;
            StartCoroutine(Regenerating());
        }
        
        if(cola == "Stamina") {normalSpeed = normalSpeed * 1.25f; moveSpeed = normalSpeed;}
        if(cola == "Reload") { reloadMultiplier = 0.5f; }
        if(cola == "Pina") { moneyFactor = 2; }

        if(cola == "HeadShot") { } // No need for code here

        if(cola == "Regen") { regenMultiplierTime = 0.5f;}

        Debug.Log("added" + cola);

        if(cola == "DeathPerception") {GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize = 11f;}

        playerColas.Add(cola);

        if(playerColas.Count == 1) {cola1.GetComponent<HUDColaDisplayScript>().changeColaSprite(cola); Debug.Log("changing cola1");}
        if(playerColas.Count == 2) {cola2.GetComponent<HUDColaDisplayScript>().changeColaSprite(cola); Debug.Log("changing cola2");}
        if(playerColas.Count == 3) {cola3.GetComponent<HUDColaDisplayScript>().changeColaSprite(cola); Debug.Log("changing cola3");}
    }

    public bool HasCola(string cola)
    {
        return playerColas.Contains(cola);
    }

    public void AddMoney(int amount)
    {
        money = money + amount * moneyFactor;
    }

    public void TakeExplosionDamage(int damageAmount)
    {
        if(tookExpDamage == false)
        {
            tookExpDamage = true;
            TakeDamage(damageAmount);
            StartCoroutine(WaitForDamageAgain());
        }
    }

    IEnumerator WaitForDamageAgain()
    {
        yield return new WaitForSeconds(0.05f);
        tookExpDamage = false;
    }
}