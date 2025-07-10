using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : NetworkBehaviour {
	public float moveSpeed = 8f;
	private CharacterController controller;
	private Animator animator;

	public override void Spawned() {
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();

		// スポーン時にCharacterControllerを一旦無効にする(座標ずれ防止)
		controller.enabled = false;

		if (Object.HasInputAuthority)
			if (Camera.main != null) {
				Camera.main.transform.SetParent(transform, false);
				Camera.main.transform.localPosition = new Vector3(0, 15, -10);
				Camera.main.transform.localEulerAngles = new Vector3(45, 0, 0);
			}

		controller.enabled = true;
	}

	public override void FixedUpdateNetwork() {
		if (controller == null || !controller.enabled) return;
		// サーバー側でのみ入力を処理
		if (Runner.IsServer) {
			if (GetInput(out NetworkInputData data)) {
				var moveDirection = new Vector3(data.horizontalInput, 0, data.verticalInput).normalized;

				if (moveDirection.sqrMagnitude > 0) {
					// moveDirectionの長さが0より大きい = 移動している
					animator.SetBool("IsWalking", true);
				}
				else {
					// 移動していない
					animator.SetBool("IsWalking", false);
				}

				controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);
			}
		}
	}
}