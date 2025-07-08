using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour {
	public float moveSpeed = 8f;
	private CharacterController controller;

	public override void Spawned() {
		// 必要なコンポーネントをここで取得しておく
		controller = GetComponent<CharacterController>();

		// もしこのオブジェクトが自分自身なら、カメラを自分の子オブジェクトにして、追従するように
		if (Object.HasInputAuthority) {
			Camera.main.transform.SetParent(transform, false); // ワールド座標を維持したまま親子関係を設定
			Camera.main.transform.localPosition = new Vector3(0, 15, -10);
			Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
		}
	}

	public override void FixedUpdateNetwork() {
		// Fusion のネットワークTICKごとに、全プレイヤーで実行される物理更新ループ

		// プレイヤーからの入力を取得し、キャラクターを移動させる
		if (GetInput(out NetworkInputData data)) {
			// 入力値から移動方向を計算し、正規化(normalize)して速度が一定になるように
			var moveDirection = new Vector3(data.horizontalInput, 0, data.verticalInput).normalized;

			// CharacterController を使って移動させる
			// Runner.DeltaTime はネットワークの1TICKあたりの時間
			controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);
		}
	}
}