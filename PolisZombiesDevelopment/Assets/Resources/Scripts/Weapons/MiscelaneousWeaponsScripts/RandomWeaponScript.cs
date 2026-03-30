using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomWeaponScript : MonoBehaviour
{
    public bool startAsAvailableBox;
    public Sprite closedBoxSprite;
    public Sprite openBoxSprite;
    public Sprite teddyOpenBoxSprite;
    public Sprite teddyNoBoxSprite;
    public AudioClip purchaseSound;
    public AudioClip teddyBearLaughSound;
    private float timeBetweenBuy = 1f;
    private float timeSinceLastBuy = 3f;
    public int price;
    private string[] guns = {"Pistol", "Revolver", "AK147", "FR456", "ShotGun", "Olimpea", "NeyGun", "Brazuca", "SniperLotus", "N14"};
    private bool availableToGet;
    private bool availableToBuy;
    private GameObject gunChildGameObject;
    public GameObject purchaseChildGameObject;
    private string gun;

    void Start()
    {
        availableToGet = false;
        gunChildGameObject = gameObject.transform.GetChild(0).gameObject;
        SetAlpha(gunChildGameObject.GetComponent<SpriteRenderer>(), 0f);
        purchaseChildGameObject = gameObject.transform.GetChild(1).gameObject;
        gameObject.GetComponent<AudioSource>().clip = purchaseSound;

        if(startAsAvailableBox)
        {
            GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
            SetAlpha(purchaseChildGameObject.GetComponent<SpriteRenderer>(), 1.0f);
            availableToBuy = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = teddyNoBoxSprite;
            SetAlpha(purchaseChildGameObject.GetComponent<SpriteRenderer>(), 0f);
            availableToBuy = false;
        }
    }

    void Update()
    {
        timeSinceLastBuy = timeSinceLastBuy + Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerScript playerScript = other.gameObject.GetComponent<PlayerScript>();
        //WeaponScript equippedWeaponScript = playerScript.equippedWeaponPrefab.GetComponent<WeaponScript>();      //////////
        WeaponScript secondWeaponScript = playerScript.secondWeaponPrefab.GetComponent<WeaponScript>();

        //Player purchases box
        if(other.gameObject.tag == "Player" && timeSinceLastBuy >= timeBetweenBuy && Input.GetKey("space") 
        && playerScript.money >= price && availableToBuy == true)
        {
            availableToBuy = false;
            playerScript.money = playerScript.money - price;
            timeSinceLastBuy = 0;
            Debug.Log("Bought Bubious Box");
            gun = guns[Random.Range(0, guns.Length)];
            
            while(gun == playerScript.equippedWeaponPrefab.name 
            || gun == playerScript.secondWeaponPrefab.name)
            {
                gun = guns[Random.Range(0, guns.Length)];
            }

            Debug.Log(gun + "asdf");
            gunChildGameObject.GetComponent<SpriteRenderer>().sprite = GameObject.Find(gun).GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(ShowWeaponToPlayer());
        }

        // Player gets weapon
        else if ((availableToGet == true) && Input.GetKey("space"))
        {
            SetAlpha(gunChildGameObject.GetComponent<SpriteRenderer>(), 0f);
            timeSinceLastBuy = 0;
            availableToGet = false;
            availableToBuy = true;
            GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
            
            // If player have no weapons
            if(playerScript.equippedWeaponPrefab.name == "EmptyGun")
            {
                playerScript.equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().DetachAccessories();
                //
                //equipInPrimaryWeapon(playerScript, equippedWeaponScript, gun);    //////////
                equipInPrimaryWeapon(playerScript, gun);
                //
            }

            // Equip it in secondary weapon
            else if(playerScript.secondWeaponPrefab.name == "EmptyGun")
            {
                if(gun == "Pistol") { playerScript.secondWeaponPrefab = GameObject.Find("Pistol"); }

                if(gun == "Revolver") { playerScript.secondWeaponPrefab = GameObject.Find("Revolver"); }

                if(gun == "AK147") { playerScript.secondWeaponPrefab = GameObject.Find("AK147"); }

                if(gun == "FR456") { playerScript.secondWeaponPrefab = GameObject.Find("FR456"); }

                if(gun == "ShotGun") { playerScript.secondWeaponPrefab = GameObject.Find("ShotGun"); }

                if(gun == "Olimpea") { playerScript.secondWeaponPrefab = GameObject.Find("Olimpea"); }

                if(gun == "NeyGun") { playerScript.secondWeaponPrefab = GameObject.Find("NeyGun"); }

                if(gun == "Brazuca") { playerScript.secondWeaponPrefab = GameObject.Find("Brazuca"); }

                if(gun == "SniperLotus") { playerScript.secondWeaponPrefab = GameObject.Find("SniperLotus"); }

                if(gun == "N14") { playerScript.secondWeaponPrefab = GameObject.Find("N14"); }

                secondWeaponScript = playerScript.secondWeaponPrefab.GetComponent<WeaponScript>();
                playerScript.secondTotalPlayerAmmo = secondWeaponScript.totalAmmo;
                playerScript.secondPlayerAmmo = secondWeaponScript.totalMagazineAmmo;
                playerScript.secondTotalPlayerMagazineAmmo = secondWeaponScript.totalMagazineAmmo;
                playerScript.updateHUDWeaponsAndAccessories();
            }

            // If player have 2 weapons
            else
            {
                playerScript.equippedWeaponPrefab.GetComponent<WeaponAcessoriesScript>().DetachAccessories();
                //
                //equipInPrimaryWeapon(playerScript, equippedWeaponScript, gun);    ////////
                equipInPrimaryWeapon(playerScript, gun);
                //
            }
        }
    }

    IEnumerator ShowWeaponToPlayer()
    {
        gameObject.GetComponent<AudioSource>().Play();
        float clipDuration = gameObject.GetComponent<AudioSource>().clip.length;
        //change sprite to box open
        yield return new WaitForSeconds(clipDuration - 3f);

        if(Random.Range(0, 10) == 0) //turn into teddy box
        {
            //Changing current box
            GetComponent<SpriteRenderer>().sprite = teddyOpenBoxSprite;
            SetAlpha(purchaseChildGameObject.GetComponent<SpriteRenderer>(), 0f);
            availableToBuy = false;
            availableToGet = false;

            //Getting another random box
            GameObject[] allTeddyBoxes = GameObject.FindGameObjectsWithTag("TeddyNoBox");
            List<GameObject> allTeddyBoxesList = new List<GameObject>(allTeddyBoxes);
            allTeddyBoxesList.Remove(gameObject);
            GameObject boxToSwitch = allTeddyBoxesList[Random.Range(0, allTeddyBoxes.Length - 1)];

            //Changing that ranom box
            boxToSwitch.GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
            SetAlpha(boxToSwitch.GetComponent<RandomWeaponScript>().purchaseChildGameObject.GetComponent<SpriteRenderer>(), 1.0f);
            boxToSwitch.GetComponent<RandomWeaponScript>().availableToBuy = true;
            boxToSwitch.GetComponent<RandomWeaponScript>().availableToGet = false;
            
            gameObject.GetComponent<AudioSource>().clip = teddyBearLaughSound;
            gameObject.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(gameObject.GetComponent<AudioSource>().clip.length - 2);
            GetComponent<SpriteRenderer>().sprite = teddyNoBoxSprite;
            yield return new WaitForSeconds(3);
            gameObject.GetComponent<AudioSource>().clip = purchaseSound;
        }

        else
        {
            GetComponent<SpriteRenderer>().sprite = openBoxSprite;
            SetAlpha(gunChildGameObject.GetComponent<SpriteRenderer>(), 1.0f);
            availableToGet = true;
            yield return new WaitForSeconds(3);
            if (availableToGet == true)
            {
                availableToGet = false;
                availableToBuy = true;
                GetComponent<SpriteRenderer>().sprite = closedBoxSprite;
                SetAlpha(gunChildGameObject.GetComponent<SpriteRenderer>(), 0f);
            }
        }

    }

    //private void equipInPrimaryWeapon(PlayerScript playerScript, WeaponScript equippedWeaponScript, string gun)
    private void equipInPrimaryWeapon(PlayerScript playerScript, string gun)
    {
        if(gun == "Pistol") { playerScript.equippedWeaponPrefab = GameObject.Find("Pistol"); }

        if(gun == "Revolver") { playerScript.equippedWeaponPrefab = GameObject.Find("Revolver"); }

        if(gun == "AK147") { playerScript.equippedWeaponPrefab = GameObject.Find("AK147"); }

        if(gun == "FR456") { playerScript.equippedWeaponPrefab = GameObject.Find("FR456"); }

        if(gun == "ShotGun") { playerScript.equippedWeaponPrefab = GameObject.Find("ShotGun"); }

        if(gun == "Olimpea") { playerScript.equippedWeaponPrefab = GameObject.Find("Olimpea"); }

        if(gun == "NeyGun") { playerScript.equippedWeaponPrefab = GameObject.Find("NeyGun"); }

        if(gun == "Brazuca") { playerScript.equippedWeaponPrefab = GameObject.Find("Brazuca"); }

        if(gun == "SniperLotus") { playerScript.equippedWeaponPrefab = GameObject.Find("SniperLotus"); }

        if(gun == "N14") { playerScript.equippedWeaponPrefab = GameObject.Find("N14"); }

        Debug.Log(playerScript.equippedWeaponPrefab + "AAAA");

        WeaponScript equippedWeaponScript = playerScript.equippedWeaponPrefab.GetComponent<WeaponScript>();

        Debug.Log(equippedWeaponScript + "AAAA");

        if(equippedWeaponScript == null) {Debug.Log("ewsNNN");}
        playerScript.totalPlayerAmmo = equippedWeaponScript.totalAmmo;
        playerScript.playerAmmo = equippedWeaponScript.totalMagazineAmmo;
        playerScript.totalPlayerMagazineAmmo = equippedWeaponScript.totalMagazineAmmo;
        playerScript.reloadTime = equippedWeaponScript.reloadTime;
        playerScript.timeBetweenShots = equippedWeaponScript.timeBetweenShots;

        playerScript.updateHUDWeaponsAndAccessories();
    }

    private void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.tag == "Player")
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
        GameObject purchaseLabel = purchaseLabelTransform.gameObject;
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "purchase box (" + price + ")";
    }
}

private void OnTriggerExit2D(Collider2D other) 
{
    if(other.gameObject.tag == "Player")
    {
        GameObject canvas = GameObject.Find("Canvas");
        Transform purchaseLabelTransform = canvas.transform.Find("PurchaseLabel");
        GameObject purchaseLabel = purchaseLabelTransform.gameObject;
        purchaseLabel.GetComponent<TextMeshProUGUI>().text = "";
    }
}
}

