using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player Type
    public ECharacter playerType;

    // Component
    Animator anim;
    Rigidbody rigid;
    CapsuleCollider capsuleCollider;
    MeshRenderer[] meshs;

    public Camera followCamera;
    public Stat hpStat;
    public Stat staminaStat;

    // Item, value
    public bool isDead;

    public float hp;
    public float stamina;
    public int magazine;

    public float armor;
    public float speed;
    public float bomb;

    public float maxHp;
    public float maxStamina;
    public float maxArmor;
    public float maxSpeed;
    public float maxBomb;


    public float curDamage = 0;

    public float weaponValue;

    // Input Key
    float hKey; 
    float vKey;
    bool wDown; // Walk = left shift
    bool eDown; // Eat = E
    bool aDown; // Attack = fire1(마우스 왼쪽)
    bool bDown; // Boom = fire2
    bool rDown; // Reload = R
    bool sDown; // Skill = Jump(SpaceBar)

    //bool isSkill;

    // Moving
    Vector3 moveVec;
    bool isBorder; // 현재 벽과 닿았는지

    // Delay
    readonly float attackMotion = 0.4f;
    bool attackReady = true;
    float attackDelay;

    // Weapons
    public GameObject[] weapons;
    public GameObject nearWeapon;
    public Weapon playerWeapon;
    int weaponIndex;

    // Grade
    public GameObject bombObj;
    const float bombDelay = 3.0f;
    float bombReady = 0f;

    // Enemy
    bool isDamaged;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        followCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        hpStat = GameObject.FindGameObjectWithTag("Hp").GetComponent<Stat>();
        staminaStat = GameObject.FindGameObjectWithTag("Stamina").GetComponent<Stat>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        hpStat.Init(maxHp, hp);
        staminaStat.Init(maxStamina, stamina);
    }

    void Start()
    {
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
    }

    void Update()
    {
        if (isDead) return;
        InputKey();
        Move();
        GetWeapon();
        Attack();
        Reload();
        Bomb();
        Skill();
    }

    void InputKey()
    {
        hKey = Input.GetAxisRaw("Horizontal");
        vKey = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        eDown = Input.GetButtonDown("Eat");
        aDown = Input.GetButton("Fire1");
        bDown = Input.GetButton("Fire2");
        rDown = Input.GetButtonDown("Reload");
        sDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        if (attackDelay < attackMotion) return;

        moveVec = new Vector3(hKey, 0, vKey).normalized;

        if (!isBorder)
            transform.position += moveVec * Time.deltaTime * speed * (wDown ? 0.5f : 1f);

        staminaStat.lerpSpeed = (wDown == true ? 2f : 1f); 

        transform.LookAt(transform.position + moveVec);

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        if(aDown)
        {
            // Bird로직만 감지 제외
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("PlayerSkill"));

            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100, layerMask))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }

    }

    void GetWeapon()
    {
        if (eDown && nearWeapon != null)
        {
            if (playerWeapon != null)
                playerWeapon.gameObject.SetActive(false);

            playerWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            playerWeapon.gameObject.SetActive(true);

            if (playerType != ECharacter.Police)
            {
                weapons[0].GetComponent<Weapon>().PlusDamage(weaponValue);
            }
            else weapons[0].GetComponent<Weapon>().PlusGun(weaponValue);

            AudioManager.instance.PlaySound(EAudio.GetItem);
            Destroy(nearWeapon);
        }
    }

    void Attack()
    {
        attackDelay += Time.deltaTime;
        attackReady = playerWeapon.rate < attackDelay;

        if(aDown && attackReady)
        {
            playerWeapon.Use();

            anim.SetTrigger(playerType != ECharacter.Police ? "doSwing" : "doShot");
            attackDelay = 0;
        }

    }

    void Reload()
    {
        if (playerWeapon == null || playerType != ECharacter.Police || magazine == 0) return;
        if(rDown && attackReady)
        {
            AudioManager.instance.PlaySound(EAudio.Reload);
            int reBullet = magazine < playerWeapon.maxMagazine ? magazine : playerWeapon.maxMagazine;
            playerWeapon.curMagazine = reBullet;
            magazine -= reBullet;
        }
    }

    void Bomb()
    {
        bombReady += Time.deltaTime;

        if (bomb == 0) return;

        if(bDown && bombDelay < bombReady)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 50))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;

                GameObject instanceBomb = Instantiate(bombObj, transform.position, transform.rotation);
                Rigidbody rigidBomb = instanceBomb.GetComponent<Rigidbody>();
                rigidBomb.AddForce(nextVec, ForceMode.Impulse);
                rigidBomb.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                bomb--;
            }

            bombReady = 0f;
        }
    }

    void Skill()
    {
        if (staminaStat.CurrentValue != maxStamina || !attackReady) return;

        if(sDown)
        {
            SkillManager.Instance.SpecialSkill(playerType, transform);
            staminaStat.CurrentValue = 0;
            staminaStat.StartCoroutine("Gauge", 1);
        }
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Item")
        {
            nearWeapon = other.gameObject;
            weaponIndex = (int)other.GetComponent<Item>().value;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
        {
            nearWeapon = null;
            weaponIndex = -1;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            switch(item.type)
            {
                case EItem.Armor:
                    armor += item.value;
                    if (armor > maxArmor) armor = maxArmor;
                    break;
                case EItem.Portion:
                    hp += item.value;
                    if (hp > maxHp) hp = maxHp;
                    hpStat.PlusHp();
                    break;
                case EItem.Shoes:
                    speed += item.value;
                    if (speed > maxSpeed) speed = maxSpeed;
                    break;
                case EItem.Bomb:
                    bomb += item.value;
                    if (bomb > maxBomb) bomb = maxBomb;
                    break;
                default:
                    return;
            }
            AudioManager.instance.PlaySound(EAudio.GetItem);
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyWeapon")
        {
            if(!isDamaged)
            {
                EnemyWeapon enemyWeapon = other.GetComponent<EnemyWeapon>();
                //hp -= enemyWeapon.damage > armor ? enemyWeapon.damage - armor : 1;
                curDamage = enemyWeapon.damage > armor ? enemyWeapon.damage - armor : 1;
                hp -= curDamage;

                hpStat.CurrentValue -= curDamage;
                StartCoroutine(OnDamaged());
            }
        }
        else if(other.tag == "Floor")
        {
            GameManager.instance.spawnOn = true;
        }
    }

    IEnumerator OnDamaged()
    {
        isDamaged = true;

        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.gray;
        }
        if(hp > 0)
        {
            AudioManager.instance.PlaySound((EAudio)((int)playerType + 7));
            yield return new WaitForSeconds(1.5f); // 무적시간
            isDamaged = false;

            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.white;
            }
        }
        else // 플레이어 사망
        {
            isDead = true;
            rigid.useGravity = false;
            capsuleCollider.enabled = false;
            anim.SetTrigger("doDead");
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.black;
            }
            GameManager.instance.Dead();
        }
    }
}
