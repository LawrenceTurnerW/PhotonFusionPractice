using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour {
	public float moveSpeed = 8f;
	private CharacterController controller;

	public override void Spawned() {
		controller = GetComponent<CharacterController>();

		// ★★★ 正しい解決策 ★★★
		// スポーン時にCharacterControllerを一旦無効にする
		controller.enabled = false;

		// 【重要】手動での座標設定は完全に削除します。
		// このSpawned()が呼ばれる時点で、FusionのNetworkTransformが
		// オブジェクトを正しいスポーン地点に配置しようとしています。
		// その処理を邪魔しないようにします。

		// CharacterControllerを再度有効にする
		controller.enabled = true;
		// ★★★ ここまでが修正箇所 ★★★

		if (Object.HasInputAuthority)
			if (Camera.main != null) {
				Camera.main.transform.SetParent(transform, false);
				Camera.main.transform.localPosition = new Vector3(0, 15, -10);
				Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
			}
	}

	public override void FixedUpdateNetwork() {
		if (controller == null || !controller.enabled) return;

		if (GetInput(out NetworkInputData data)) {
			var moveDirection = new Vector3(data.horizontalInput, 0, data.verticalInput).normalized;
			controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);
		}
	}
}