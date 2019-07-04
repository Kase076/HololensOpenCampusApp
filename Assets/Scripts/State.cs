using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HFSM{
// 階層型ステートマシンの基底クラス
public class State<S, M>
{
	public void Update()
	{
		this.Execute();
	}

    // この状態が制御するモデル
    protected M _model;
    // 現在の子状態を表す識別子
    protected S _current_child_state_id;
    // 現在の子状態
    protected State<S, M> _current_child_state;

    // -x-x-x- Properties -x-x-x-

    // 親の状態を設定または取得する
    public State<S, M> Parent { get; set; }

    // 子状態が取りえる状態を持つテーブル
	public IDictionary<S, State<S, M>> ChildStateTable { get; set; } = new Dictionary<S, State<S, M>>();

    // 現在の子状態を取得します。
    public State<S, M> ChildState { get { return this._current_child_state; } }

    // -x-x-x- Constructors -x-x-x-

    // 制御対象のモデルを指定してオブジェクトを初期化する
    public State(M model)
    {
        this._model = model;
    }

    // -x-x-x- Methods -x-x-x-

    // 遷移時に一度だけ呼ばれる
	public virtual void Enter(){}

    // この状態中毎フレーム呼び出される
	public virtual void Execute(){}

    // この状態を抜ける時に一度だけ呼ばれる
	public virtual void Exit(){}

    // 子状態を追加する
    public void AddChildState(S state_id, State<S, M> state)
    {
		state.Parent = this;
		//ChildStateTable.Add(state_id, state);
		this.ChildStateTable.Add(state_id, state);
    }
	
    // 次の状態に自己遷移する
    public void ChangeState(S next_status)
    {
        if (this.Parent == null)
        {
            //throw new InvalidOperationException("Not set parent.");
        }

        this.Parent.ChangeChildState(next_status);
    }

    // 子要素の状態を変更する
    public void ChangeChildState(S next_status)
    {
        // 次の遷移が存在するかどうかを確認する
        if (!this.ChildStateTable.ContainsKey(next_status))
        {
            //throw new InvalidOperationException("Can not transit state. " + next_status.ToString());
        }

        // 末端までの子状態を集めて末端の子要素から順に退場動作を実行していく
        var childs = new List<State<S, M>>();
        State<S, M> tempState = this._current_child_state;
        while (tempState != null)
        {
            childs.Insert(0, tempState);
            tempState = tempState.ChildState;
        }

        foreach (State<S, M> c in childs)
        {
            c.Exit();
        }

        this._current_child_state_id = next_status;
        this._current_child_state = this.ChildStateTable[next_status];
        this._current_child_state.Enter();

        // 状態を変更したら状態を1回実行する（不要であればコメントアウト）
        this._current_child_state.Execute();
    }
}
}