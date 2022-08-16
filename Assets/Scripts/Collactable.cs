using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

public class Collactable : MonoBehaviour
{
    public static Collactable instance;
    public float movementDelay = 0.25f;
    public List<GameObject> collectedMoney = new List<GameObject>();
    public int moneyValue=0;
    public TextMeshProUGUI moneyValueText;
    private InputManager _inputManager;
    private Transform _parent;

    [Space(30)]
    [Header("Mesh & Materials")]
    [Space(10)]
    public Mesh goldMesh;
    public Mesh diamondMesh;
    public Material goldMaterial, diamondMaterial;

    [Header("Particle Effects")]
    [Space(10)]
    public ParticleSystem cashDestroyedParticle;
    public ParticleSystem goldDestroyedParticle;
    public ParticleSystem diamondDestroyedParticle;

    [Header("Finish")]
    [Space(10)]
    public GameObject finishTarget;
    public GameObject cashPrefab, playerPrefab;
    private Animator anim;
    public float targetToMoveDelay, finishDelay, cameraLerpTime;


    private void Awake()
    {
        _parent = transform;
        _inputManager = new InputManager();
        if (instance==null)
        {
            instance = this;
            moneyValue = 0;
        }
    }

    private void Update()
    {
        if (GameManager.instance.gameState==GameManager.GameState.playing)
        {
            if (_inputManager.MouseHold)
            {
                MoveListElements();
            }
            else
            {
                MoveOrigin();
            }
        }
       
        if (GameManager.instance.gameState == GameManager.GameState.finished && GameManager.instance.finish)
        {
            GameManager.instance.finish = false;
            Camera.main.transform.parent = null;
            //Camera.main.GetComponent<CameraFollow>().offset = new Vector3(0, 1, -6);
            Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            anim = playerPrefab.GetComponent<Animator>();
            anim.SetTrigger("finish");
            playerPrefab.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            StartCoroutine(GameFinished());
        }
    }

    public void StackMoney(GameObject other,int index)
    {
        other.gameObject.name = "Money" + collectedMoney.Count;
        moneyValue++;
        //moneyValueText.text = moneyValue.ToString();
        other.transform.parent = _parent;
        Vector3 newPos = collectedMoney[index].transform.localPosition;
        if (index == 0)
        {
            newPos.z += .0005f;
        }
        newPos.z += .3f;
        newPos.y = .1f;
        other.transform.localPosition = newPos;
        collectedMoney.Add(other);
        StartCoroutine(MakeObjectsBigger());
    }

    public void MoveListElements()
    {
        for (int i = 1; i < collectedMoney.Count; i++)
        {
            Vector3 pos = collectedMoney[i].transform.position;
            pos.x = collectedMoney[i - 1].transform.position.x;
            collectedMoney[i].transform.position = Vector3.Lerp(collectedMoney[i].transform.position, pos, 20 * Time.deltaTime);
        }
    }
    public void MoveOrigin()
    {
        for (int i = 1; i < collectedMoney.Count; i++)
        {
            Vector3 pos = collectedMoney[i].transform.position;
            pos.x = collectedMoney[0].transform.position.x;
            collectedMoney[i].transform.position = Vector3.Lerp(collectedMoney[i].transform.position, pos, 4 * Time.deltaTime);

        }
    }
    public IEnumerator MakeObjectsBigger()
    {    
        for (int i = collectedMoney.Count - 1; i > 0; i--)
        {
            var index = i;
            Vector3 scale = new Vector3(1, 1, 1);
            if (collectedMoney[index].gameObject.tag == "Money"|| collectedMoney[index].gameObject.tag == "Gold" || collectedMoney[index].gameObject.tag == "Diamond")
            {
                scale *= 1.5f;
                collectedMoney[index].transform.DOScale(scale, 0.1f).OnComplete(() =>
                 collectedMoney[index].transform.DOScale(new Vector3(1, 1, 1), 0.1f));
                yield return new WaitForSeconds(0.05f);
            }

        }
    }
    public IEnumerator UpgradeCollectable(GameObject gameObject)
    {
        if (gameObject.CompareTag("Money"))
        {
            moneyValue++;
            gameObject.tag = "Gold";
            gameObject.GetComponent<MeshFilter>().mesh = goldMesh;
            gameObject.GetComponent<MeshRenderer>().material = goldMaterial;
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(0.003f, 0.005f, 0.001f);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            gameObject.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
            gameObject.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 1, 1);

            yield return new WaitForSeconds(0.05f);

        }
        else if (gameObject.CompareTag("Gold") || gameObject.CompareTag("Diamond"))
        {
            moneyValue++;
            gameObject.tag = "Diamond";
            gameObject.GetComponent<MeshFilter>().mesh = diamondMesh;
            gameObject.GetComponent<MeshRenderer>().material = diamondMaterial;
            //gameObject.GetComponent<BoxCollider>().size = new Vector3(0.02f, 0.02f, 0.01f);
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            gameObject.transform.localScale = new Vector3(.8f, .8f, .8f);
            gameObject.transform.DOPunchScale(new Vector3(1f, 1f, 1f), 1f, 1, 1);

            yield return new WaitForSeconds(0.05f);
        }
    }
    public IEnumerator ATMKeepMoney(GameObject other,int index)
    {
        if (index == 0)
            index = 1;
        if (index==collectedMoney.Count-1)
        {
            GameObject gameObject = collectedMoney[collectedMoney.Count - 1];
            Vector3 scale = gameObject.transform.localScale;
            Vector3 doScale = scale * .4f;
            collectedMoney.Remove(gameObject);         
            gameObject.transform.DOScale(doScale, 0.02f).OnComplete(() =>
                     gameObject.transform.DOScale(scale, 0.02f)).OnComplete(() => { Destroy(gameObject); });
        }
        else
        {
            for (int i = collectedMoney.Count-1; i >= index; i--)
            {
                GameObject gameObject = collectedMoney[collectedMoney.Count-1];
                Vector3 scale = gameObject.transform.localScale;
                Vector3 doScale = scale * .4f;
                collectedMoney.Remove(gameObject);
                gameObject.transform.DOScale(doScale, 0.06f).OnComplete(() =>
                         gameObject.transform.DOScale(scale, 0.06f)).OnComplete(() => { Destroy(gameObject); });
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
    public void DestroyMoney(GameObject other,int index,GameObject obstacle)
    {
        if (index==0)
        {
            index = 1;
        }
        int countList = collectedMoney.Count;
        for (int i =index; i <countList ; i++)
        {
            GameObject gameObject = collectedMoney[i];
            if (i==collectedMoney.Count-1)
            {
                Instantiate(cashDestroyedParticle, obstacle.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            }
            if (gameObject.CompareTag("Money"))
            {
                moneyValue--;
                //moneyValueText.text = moneyValue.ToString();
            }
            else if (gameObject.CompareTag("Gold"))
            {
                moneyValue -= 2;
                //moneyValueText.text = moneyValue.ToString();
            }
            else if (gameObject.CompareTag("Diamond"))
            {
                moneyValue -= 3;
                //moneyValueText.text = moneyValue.ToString();
            }
            Destroy(gameObject);
        }
        collectedMoney.RemoveRange(index, collectedMoney.Count - index);
    }
    public void DisperseCollectibles(GameObject other,int index,GameObject obsatcle)
    {
        if (index==0)
        {
            index = 1;
        }
        for (int i = collectedMoney.Count-1; i > index; i--)
        {
            GameObject gameObject = collectedMoney[i];
            collectedMoney.Remove(gameObject);
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject.GetComponent<Collision>());
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.transform.parent = null;
            Vector3 target = new Vector3(UnityEngine.Random.Range(-2, 2), 0.1f, obsatcle.transform.position.z + UnityEngine.Random.Range(2, 20));
            //Vector3 targerUpPos = target - new Vector3(0, -3, (target.z - gameObject.transform.position.z) / 2);
            gameObject.transform.DOJump(target, 2, 1, 1);
            if (gameObject.CompareTag("Money"))
            {
                moneyValue--;
                //moneyValueText.text = moneyValue.ToString();
                gameObject.tag = "CollectableMoney";
            }
            else if (gameObject.CompareTag("Gold"))
            {
                moneyValue -= 2;
               // moneyValueText.text = moneyValue.ToString();
                gameObject.tag = "CollectableGold";
            }
            else if (gameObject.CompareTag("Diamond"))
            {
                moneyValue -= 3;
               // moneyValueText.text = moneyValue.ToString();
                gameObject.tag = "CollectableDiamond";
            }
        }

    }
    public void FinishCollecterMoney(GameObject gameObject)
    {
        gameObject.tag = "Untagged";
        gameObject.transform.parent = null;
        collectedMoney.Remove(gameObject);
        gameObject.transform.DOMove(finishTarget.transform.position, targetToMoveDelay);
    }
    IEnumerator GameFinished()
    {
        playerPrefab.transform.parent = null;
        for (int i = 0; i < moneyValue; i++)
        {
            GameObject go = Instantiate(cashPrefab, new Vector3(0, -3.61f + (i * 0.4f), 205), Quaternion.Euler(new Vector3(-90, 90, 0)));
            go.tag = "Untagged";
            playerPrefab.transform.position = new Vector3(0, go.transform.position.y + .06f, 204.73f);
            yield return new WaitForSeconds(finishDelay);
        }
    }
}
