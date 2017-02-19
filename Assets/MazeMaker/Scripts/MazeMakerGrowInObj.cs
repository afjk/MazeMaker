using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMakerGrowInObj : MonoBehaviour {

    public bool growOnStart = true;

    private GameObject roadObject = null;
    public GameObject coverObject = null;
    public GameObject preRoadObject = null;
    public float scale = 1.0f;

    private GameObject mMazeBase;

    public bool makeMazeComplete = false;

    private bool isActive = false;

    public delegate void MazeMakerEventHandler(object sender);
    public event MazeMakerEventHandler MakeComplete;

    // Use this for initialization
    void Start()
    {
        scale = this.gameObject.transform.localScale.x;
        roadObject = this.gameObject;
        MakeRayList();

        if ( isStart() )
        {
            if (coverObject == null)
            {
                Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f));

                foreach (Collider item in colliders)
                {
                    if (item.gameObject != this.gameObject)
                    {
                        coverObject = item.gameObject;
                        break;
                    }
                }

            }

        }

        if (growOnStart)
        {
            Grow();
        }
    }

    private List<Ray> mRayList = new List<Ray>();

    private void MakeRayList()
    {
        mRayList.Add(new Ray(transform.position, this.transform.forward));
        mRayList.Add(new Ray(transform.position, -this.transform.forward));
        mRayList.Add(new Ray(transform.position, this.transform.right));
        mRayList.Add(new Ray(transform.position, -this.transform.right));
        mRayList.Add(new Ray(transform.position, this.transform.up));
        mRayList.Add(new Ray(transform.position, -this.transform.up));
    }

    public void Grow()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            isActive = false;

            // 進行可能な方向を得る
            Vector3 direction = GetGoDirection();

            if (direction == Vector3.zero)
            {
                // 進行可能な方向がない場合、戻る。
                GoBack();
            }
            else
            {
                // 進行可能な方向に2セル進む
                GoAhead(direction);
            }
        }
    }

    bool InCoverObj(Vector3 pos)
    {
        bool result = false;

        if (coverObject)
        {
            Collider[] colliders = Physics.OverlapBox(pos, new Vector3(0.1f, 0.1f, 0.1f));
            foreach (Collider item in colliders)
            {
                if (item.gameObject == coverObject)
                {
                    result = true;
                    break;
                }
            }
        }
        else
        {
            result = true;
        }
        return result;
    }

    Vector3 GetGoDirection()
    {
        Vector3 result = Vector3.zero;

        List<Ray> rayList = new List<Ray>();
        RaycastHit hit;
        float distance = 2.0f * scale;
        float radious = 1.0f * scale;

        if( mRayList.Count > 0 )
        {
            foreach (Ray ray in mRayList)
            {
                if (!Physics.SphereCast(ray, radious, out hit, distance))
                {
                    if (InCoverObj(transform.position + ray.direction * distance))
                    {
                        rayList.Add(ray);
                    }
                }
            }
        }
        if (rayList.Count > 0)
        {
            int randIndex = Random.Range(0, rayList.Count);
            result = rayList[randIndex].direction;
        }
        Debug.Log(result);
        return result;
    }

    bool isStart()
    {
        if (preRoadObject == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void GoBack()
    {
        Debug.Log("GoBack:in");
        // 2セル戻る
        // スタート地点なら生成終了
        if (isStart())
        {
            Debug.Log("Maze Generate Complete!");
            makeMazeComplete = true;
            // 完了通知
            OnMakeComplete();
            return;
        }
        else
        {
            // 戻る
            if (preRoadObject != null)
            {
                preRoadObject.GetComponent<MazeMakerGrowInObj>().Grow();
            }
        }
    }

    void GoAhead(Vector3 direction)
    {
        GameObject copySpaceCube1 = Object.Instantiate(roadObject) as GameObject;
        copySpaceCube1.transform.position = this.transform.position + direction * 1.0f * scale;
        copySpaceCube1.name = roadObject.name;

        Destroy(copySpaceCube1.GetComponent<MazeMakerGrowInObj>());

        GameObject copySpaceCube2 = Object.Instantiate(roadObject) as GameObject;
        copySpaceCube2.transform.position = this.transform.position + direction * 2.0f * scale;
        copySpaceCube2.name = roadObject.name;

        copySpaceCube2.GetComponent<MazeMakerGrowInObj>().preRoadObject = this.gameObject;
        copySpaceCube2.GetComponent<MazeMakerGrowInObj>().Grow();

        if( this.transform.parent != null )
        {
            copySpaceCube1.transform.parent = this.transform.parent;
            copySpaceCube2.transform.parent = this.transform.parent;
        }
    }

    /* 迷路生成完了通知 */
    public virtual void OnMakeComplete()
    {
        if (MakeComplete != null)
        {
            MakeComplete(this);
        }
    }
}
