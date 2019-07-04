using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using HFSM;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;

//ステートリスト
public enum FireState
{
    //FloorFireState
    FloorFire,          //Parent
    Grow,
    Spread,
    Extinguishing,
    //WallFireState
    WallFire,           //Parent
    OnWall,
    OnCeiling,
    OnWallSpread,
    OnWallExtinguishing,
    //ExtinguishedState
    Extinguished,       //Parent
    DestroyedFire,
}

public class FireAI : /*AIModel<FireState, FireAI>,*/ MonoBehaviour
{
    private float time;
    private float fire_size = 1.0f;
    private List<GameObject> ChildFireList = new List<GameObject>();
    private Rigidbody _rigidbody;
    private ParticleSystem particle;
    //private Vector3 default_size;

    private State<FireState, FireAI> RootState;

    private GameObject ParentFire;

    private int Nearwall;
    public int NearWall
    {
        get { return Nearwall; }
        set { Nearwall = value; }
    }
    private int Celling;
    public int celling
    {
        get { return Celling; }
        set { Celling = value; }
    }

    public bool DestroyedFlag = false;  

    public Vector3 nextArea;

    [SerializeField]
    private GameObject fireball;

    // Use this for initialization
    void Start()
    {
        RootState = new State<FireState, FireAI>(this);
        NearWall = 0;
        celling = 0;
        _rigidbody = this.GetComponent<Rigidbody>();
        particle = this.GetComponentInChildren<ParticleSystem>();
        //default_size = this.transform.localScale;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        this.RootState.ChildState.Update();
        if (this.NearWall != 1 && this.celling != 1)
        {
            if (_rigidbody.velocity.magnitude >= 10.0f)
            {
                this.RootState.ChangeChildState(FireState.Extinguished);
            }
        }
        //this.transform.localScale = default_size * fire_size;
        time++;
    }

    void FixedUpdate()
    {
        //Debug.Log(fire_size);
    }

    public void Initialize()
    {
        // 状態の組み立て
        //FloorFireステートの組み立て
        this.RootState.AddChildState(FireState.FloorFire, new FloorFire(this));
        this.RootState.ChildStateTable[FireState.FloorFire].AddChildState(FireState.Grow, new GrowState(this));
        this.RootState.ChildStateTable[FireState.FloorFire].AddChildState(FireState.Spread, new SpreadState(this));
        this.RootState.ChildStateTable[FireState.FloorFire].AddChildState(FireState.Extinguishing, new ExtinguishingState(this));
        //WallFireステートの組み立て
        this.RootState.AddChildState(FireState.WallFire, new WallFire(this));
        this.RootState.ChildStateTable[FireState.WallFire].AddChildState(FireState.OnWall, new OnWallState(this));
        this.RootState.ChildStateTable[FireState.WallFire].AddChildState(FireState.OnCeiling, new OnCeilingState(this));
        this.RootState.ChildStateTable[FireState.WallFire].AddChildState(FireState.OnWallSpread, new OnWallSpreadState(this));
        this.RootState.ChildStateTable[FireState.WallFire].AddChildState(FireState.OnWallExtinguishing, new OnWallExtinguishingState(this));
        //Extinguishedステートの組み立て
        this.RootState.AddChildState(FireState.Extinguished, new Extinguished(this));
        this.RootState.ChildStateTable[FireState.Extinguished].AddChildState(FireState.DestroyedFire, new DestroyedFireState(this));

        this.RootState.ChangeChildState(FireState.FloorFire); // 初期状態を指定
    }

    public void FireInstance()
    {
        var new_fire = GameObject.Instantiate(fireball);
        Vector3 tmp = this.transform.position;
        float x = tmp.x + Random.Range(-0.6f, 0.6f);
        float y = tmp.y + 0.1f;
        float z = tmp.z + Random.Range(-0.6f, 0.6f);
        new_fire.transform.position = new Vector3(x, y, z);
        FireMeta.FireList.Add(new_fire);
        ChildFireList.Add(new_fire);
        FireAI child_fire_ai = new_fire.GetComponent<FireAI>();
        child_fire_ai.ParentFire = this.gameObject;
        //ネットワーク処理
        //NetworkServer.Spawn(new_fire);
    }

    public void OnWallFireInstance()
    {
        var new_fire = GameObject.Instantiate(fireball);
        _rigidbody = new_fire.GetComponent<Rigidbody>();
        //_rigidbody.useGravity = false;
        _rigidbody.isKinematic = true;
        Vector3 tmp = this.transform.position;
        float x = tmp.x + Random.Range(-0.3f, 0.3f);
        float y = tmp.y + Random.Range(0.3f, 0.5f);
        float z = tmp.z + Random.Range(-0.3f, 0.3f);
        new_fire.transform.position = new Vector3(x, y, z);
        FireMeta.FireList.Add(new_fire);
        ChildFireList.Add(new_fire);
        FireAI child_fire_ai = new_fire.GetComponent<FireAI>();
        child_fire_ai.ParentFire = this.gameObject;
        //ネットワーク処理
        //NetworkServer.Spawn(new_fire);
    }

    public void OnCellingFireInstance()
    {
        var new_fire = GameObject.Instantiate(fireball);
        _rigidbody = new_fire.GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        //_rigidbody.useGravity = false;
        Vector3 tmp = this.transform.position;
        float x = tmp.x + Random.Range(-0.1f, 0.1f);
        float y = tmp.y + Random.Range(0.05f, 0.2f);
        float z = tmp.z + Random.Range(-0.1f, 0.1f);
        new_fire.transform.position = new Vector3(x, y, z);
        FireMeta.FireList.Add(new_fire);
        ChildFireList.Add(new_fire);
        FireAI child_fire_ai = new_fire.GetComponent<FireAI>();
        child_fire_ai.ParentFire = this.gameObject;
        //ネットワーク処理
        //NetworkServer.Spawn(new_fire);
    }

    //public void OnInputClicked(InputClickedEventData eventData)
    //{
    //    //fire_size = fire_size * 0.5f;
    //    //fire_size = 0;
    //}

    /// <summary>
    /// Floor Fire State
    /// </summary>
    public class FloorFire : State<FireState, FireAI>
    {
        public FloorFire(FireAI owner) : base(owner) { }

        public override void Enter()
        {
            this._model.time = 0;
            Debug.Log("State FloorFire Called");
            this.ChangeChildState(FireState.Grow);
        }

        public override void Execute()
        {
            if (this._model.NearWall == 1)
            {
                this.ChangeState(FireState.WallFire);
                Debug.Log("Change OnWall State");
            }
            this.ChildState.Execute();
        }

        public override void Exit()
        {
            Debug.Log("State FloorFire Exited");
            this._model.time = 0;
        }
    }

    public class GrowState : State<FireState, FireAI>
    {
        public GrowState(FireAI owner) : base(owner) { }

        float current_fire_size;

        public override void Enter()
        {
            this._model.time = 0;
            current_fire_size = this._model.fire_size;
            Debug.Log("Grow");
        }

        public override void Execute()
        {
            if (this._model.fire_size <= 30.0f)
            {
                this._model.fire_size = this._model.fire_size + 0.01f;
            }
            if (current_fire_size > this._model.fire_size)
            {
                Debug.Log("Start Extinguishing");
                this.ChangeState(FireState.Extinguishing);
            }
            if (this._model.time >= 800.0f)
            {
                this.ChangeState(FireState.Spread);
            }
            if (this._model.celling == 1)
            {
                this.Parent.ChangeState(FireState.WallFire);
            }
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }

    public class SpreadState : State<FireState, FireAI>
    {
        public SpreadState(FireAI owner) : base(owner) { }

        public override void Enter()
        {
            //Instantiate (this._model.fireball);
            if (this._model.ChildFireList.Count < 1)
            {
                this._model.FireInstance();
            }
        }

        public override void Execute()
        {
            this.ChangeState(FireState.Grow);
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }

    public class ExtinguishingState : State<FireState, FireAI>
    {
        public ExtinguishingState(FireAI owner) : base(owner) { }

        float current_fire_size;

        public override void Enter()
        {
            this._model.time = 0;
            Debug.Log("Extinguishing");
        }

        public override void Execute()
        {
            if (this._model.time > 50.0f)
            {
                this.ChangeState(FireState.Grow);
            }
            if (this._model.fire_size <= 1.0f)
            {
                this.Parent.ChangeState(FireState.Extinguished);
            }
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }


    /// <summary>
    /// Wall Fire State.
    /// </summary>
    public class WallFire : State<FireState, FireAI>
    {
        public WallFire(FireAI owner) : base(owner) { }

        public override void Enter()
        {
            this._model.time = 0;
            Debug.Log("State WallFire Called");
            if (this._model.celling == 1)
            {
                ChangeChildState(FireState.OnCeiling);
            }
            else
            {
                this.ChangeChildState(FireState.OnWall);
            }
        }

        public override void Execute()
        {
            this.ChildState.Execute();
        }

        public override void Exit()
        {
            Debug.Log("State WallFire Exited");
            this._model.time = 0;
        }
    }

    public class OnWallState : State<FireState, FireAI>
    {
        public OnWallState(FireAI owner) : base(owner) { }

        float current_fire_size;

        public override void Enter()
        {
            this._model.time = 0;
            current_fire_size = this._model.fire_size;
            Debug.Log("Grow on Wall");
        }

        public override void Execute()
        {
            if (this._model.celling == 1)
            {
                this.ChangeState(FireState.OnCeiling);
            }
            if (this._model.fire_size <= 30.0f)
            {
                this._model.fire_size = this._model.fire_size + 0.01f;
            }
            if (current_fire_size > this._model.fire_size)
            {
                Debug.Log("Start Extinguishing");
                this.ChangeState(FireState.OnWallExtinguishing);
            }
            if (this._model.time >= 500.0f)
            {
                this.ChangeState(FireState.OnWallSpread);
            }
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }

    public class OnCeilingState : State<FireState, FireAI>
    {
        public OnCeilingState(FireAI owner) : base(owner) { }

        public override void Enter()
        {

        }

        public override void Execute()
        {
            Debug.Log("Warning On Celling");
        }

        public override void Exit()
        {

        }
    }

    public class OnWallSpreadState : State<FireState, FireAI>
    {
        public OnWallSpreadState(FireAI owner) : base(owner) { }

        public override void Enter()
        {
            if (this._model.ChildFireList.Count < 2)
            {
                if (this._model.celling == 0)
                {
                    this._model.OnWallFireInstance();
                }
                if (this._model.celling == 1)
                {
                    this._model.OnWallFireInstance();
                }
            }
        }

        public override void Execute()
        {
            this.ChangeState(FireState.OnWall);
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }

    public class OnWallExtinguishingState : State<FireState, FireAI>
    {
        public OnWallExtinguishingState(FireAI owner) : base(owner) { }

        float current_fire_size;


        public override void Enter()
        {
            this._model.time = 0;
            Debug.Log("Extinguishing on Wall");
        }

        public override void Execute()
        {
            if (this._model.time > 50.0f)
            {
                this.ChangeState(FireState.OnWall);
            }
            if (this._model.fire_size <= 1.0f)
            {
                this.Parent.ChangeState(FireState.Extinguished);
            }
        }

        public override void Exit()
        {
            this._model.time = 0;
        }
    }


    /// <summary>
    /// Extinguished State.
    /// </summary>
    public enum ExtinguishedFire { }

    public class Extinguished : State<FireState, FireAI>
    {
        public Extinguished(FireAI owner) : base(owner) { }

        public override void Enter()
        {
            if (this._model.ParentFire != null)
            {
                FireAI parent_fire_ai = this._model.ParentFire.GetComponent<FireAI>();
                parent_fire_ai.ChildFireList.Remove(this._model.gameObject);
            }
            this.ChangeChildState(FireState.DestroyedFire);
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    public class DestroyedFireState : State<FireState, FireAI>
    {
        public DestroyedFireState(FireAI owner) : base(owner) { }

        private float time;

        public override void Enter()
        {
            this._model.CmdDestroyFire();
        }

        public override void Execute()
        {

        }

        public override void Exit()
        {

        }
    }

    public void TriggerThis()
    {
        //fire_size = fire_size * 0.5f;
        fire_size = 0;
    }

    //[Command]
    public void CmdDestroyFire()
    {
        DestroyedFlag = true;
        particle.Stop();
        //Destroy(this.gameObject);
        //NetworkServer.Destroy(this.gameObject);
    }

}
