using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EEnemy type;

    public ECharacter playertype;

    public float maxHp;
    public float curHp;
    public float speed;
    public bool isNav;
    public bool isAttack;
    public bool isSturn;
    public bool isDead;

    public bool redBoss;

    // Item
    public GameObject[] Items;


    public float sturnDelay = 2.0f;
    
    public BoxCollider meleeArea;
    public GameObject melee;

    // Component
    NavMeshAgent nav;
    Transform target;

    Rigidbody rigid;
    BoxCollider boxCollider;
    Animator anim;
    MeshRenderer[] meshs;


    public float AttackDelay;
    public float curDelay;
    public bool AttackReady;

    int attackPattern = -1;

    // Rader
    public float curRange;
    public float areaRange;
    const float castingRange = 15f;

    public BossPos fireBall;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();


        anim = GetComponent<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    private void OnEnable()
    {
        GameManager.instance.curEnemy += 1;
        curHp = maxHp;

        if (isDead)
        {
            rigid.freezeRotation = true;
            gameObject.layer = 11;
            nav.enabled = true;
            NavPlayer();
            isDead = false;
            meleeArea.gameObject.SetActive(true);

            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }

    void Start()
    {
        // 플레이어를 Instantiate 사용하여 생성하므로 테그로 플레이어를 찾기
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        playertype = target.GetComponent<Player>().playerType;

        curRange = areaRange;

        // nav 위치 초기화
        nav.Warp(transform.position);
        nav.speed = speed;
        NavPlayer();

        if (type == EEnemy.EBasic) PlusDamage(GameManager.instance.zombieDaamge);
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.enabled) // 네비 활성화인지
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isNav;
        }
    }

    void NavPlayer()
    {
        isNav = true;
    }

    // 추가한 로직, 적이 물리충돌로인한 방향 전환 실패 방지
    void FreezeRotation()
    {
        if(isNav)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        Rader();
        FreezeRotation();
    }

    void Rader()
    {
        if (curHp < 0) return;

        curDelay += Time.deltaTime;
        if (curDelay < AttackDelay || isSturn) return;

        if (type == EEnemy.EBoss)
        {
            attackPattern = Random.Range(1, 11);
        }
        else if (type == EEnemy.EMiddle)
        {
            attackPattern = Random.Range(1, 9);
        }

        if (attackPattern >= 9)
        {
            curRange = castingRange;
        }
        else curRange = areaRange;

        RaycastHit[] rayHits =
            Physics.SphereCastAll(transform.position,
                            curRange,
                            transform.forward,
                            0f,
                            LayerMask.GetMask("Player"));

        

        if (rayHits.Length > 0 && attackPattern < 5 ) StartCoroutine(Attack());
        else if (rayHits.Length > 0 && attackPattern < 9) StartCoroutine(Swing());
        else if (rayHits.Length > 0 && attackPattern < 11) StartCoroutine(Shot());
    }

    IEnumerator Attack()
    {
        curDelay = 0;
        isNav = false;
        isAttack = true;

        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(2f);
        meleeArea.enabled = false;

        anim.SetBool("isAttack", false);
        isAttack = false;
        isNav = true;
    }

    IEnumerator Swing()
    {
        curDelay = 0;
        isNav = false;
        isAttack = true;

        anim.SetBool("isSwing", true);

        yield return new WaitForSeconds(0.25f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        anim.SetBool("isSwing", false);
        isAttack = false;
        isNav = true;
    }

    IEnumerator Shot()
    {
        curDelay = 0;
        isNav = false;
        isAttack = true;

        anim.SetBool("isShot", true);

        yield return new WaitForSeconds(1.1f);
        fireBall.FireBall();

        yield return new WaitForSeconds(1f);

        anim.SetBool("isShot", false);
        isAttack = false;
        isNav = true;
    }

    IEnumerator StopEnemy()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        isNav = false;
        nav.speed = 0;
        isSturn = true;
        anim.SetTrigger("doSturn");


        yield return new WaitForSeconds(sturnDelay);
        isNav = true;
        isSturn = false;

        if (type != EEnemy.EBasic)
        {
            nav.speed = speed / 2.0f;
            yield return new WaitForSeconds(1f);
        }
    }

    public void Sturn()
    {
        if (curHp > 0)
        {
            StopCoroutine(StopEnemy());
            StartCoroutine(StopEnemy());
            nav.speed = speed;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isDead && other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;

            StartCoroutine(OnDamage(reactVec, weapon.bouncingPower));
        }
        else if(!isDead && other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, bullet.bouncingPower));
        }
        else if(!isDead && other.tag == "PlayerSkill")
        {
            SkillItem item = other.GetComponent<SkillItem>();
            if(item.eSkill != ESkill.ESpawn)
            {
                curHp -= item.damage;
                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec, 10));
            }
        }
    }

    public void Bomb(Vector3 vec)
    {
        curHp -= 50;
        Vector3 rigidVec = transform.position - vec;
        StartCoroutine(OnDamage(rigidVec, 50.0f));
    }


    void DropItem(Vector3 vec)
    {
        if (Random.Range(1, 11) < 3)
        {
            int RandomItem = Random.Range(0, 10);
            int ItemNum = 0;
            if (RandomItem < 2)
                ItemNum = (int)playertype;
            else if (RandomItem == 2)
                ItemNum = 3;
            else if (RandomItem < 6)
                ItemNum = 4;
            else if ( RandomItem < 8)
                ItemNum = 5;
            else
                ItemNum = 6;

            GameObject i = Instantiate(Items[ItemNum], transform.position, transform.rotation);
            i.GetComponent<Rigidbody>().AddForce(vec * 8, ForceMode.Impulse);
        }
    }

    public void PlusDamage(int num)
    {
        melee.GetComponent<EnemyWeapon>().damage = num;
    }

    IEnumerator OnDamage(Vector3 vec, float power)
    {
        if(type !=  EEnemy.EBasic)
        {
            anim.SetTrigger("doDamage");
        }


        foreach(MeshRenderer mesh in meshs)
                mesh.material.color = Color.yellow;

        if(curHp > 0)
        {
            yield return new WaitForSeconds(1f);
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;
            gameObject.layer = 12;
            isNav = false;
            nav.enabled = false;
            isDead = true;

            anim.SetTrigger("doDie");
            meleeArea.gameObject.SetActive(false);

            if (type == EEnemy.EBasic) AudioManager.instance.PlaySound(EAudio.Die_zombie);
            else if (type == EEnemy.EBoss)
            {
                AudioManager.instance.PlaySound(EAudio.Boss_Die);
                GameManager.instance.isBoss = false;
                GameManager.instance.GameClear();
                yield return new WaitForSeconds(1f);
            }
            else if (type == EEnemy.EMiddle)
            {
                if (redBoss) AudioManager.instance.PlaySound(EAudio.ESlimeDie);
                else AudioManager.instance.PlaySound(EAudio.EQuillDie);

                GameManager.instance.CheckMidleBoss();
                yield return new WaitForSeconds(1f);
            }

            vec = vec.normalized;
            vec += Vector3.up;

            rigid.freezeRotation = false;
            GameManager.instance.curEnemy -= 1;

            if (type == EEnemy.EBasic) DropItem(vec);

            if (power == 50.0) //수류탄일때
            {
                rigid.AddForce(vec * 10, ForceMode.Impulse);
                rigid.AddTorque(vec * 10, ForceMode.Impulse);
            }
            else 
                rigid.AddForce(vec * power/3f, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
