using Fusion;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour {
	public float moveSpeed = 8f;
	private CharacterController controller;

	public override void Spawned() {
		controller = GetComponent<CharacterController>();

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

		if (GetInput(out NetworkInputData data)) {
			var moveDirection = new Vector3(data.horizontalInput, 0, data.verticalInput).normalized;
			controller.Move(moveDirection * moveSpeed * Runner.DeltaTime);
		}
	}
}