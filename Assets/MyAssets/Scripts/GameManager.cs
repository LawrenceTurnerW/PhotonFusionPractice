using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

// Photon Fusionのネットワーク管理・プレイヤーのスポーン・入力収集を担当するマネージャー
public class GameManager : MonoBehaviour, INetworkRunnerCallbacks {
	[Header("Prefabs")] [SerializeField] private NetworkPrefabRef _playerPrefab;

	[Header("Spawn Points")] [SerializeField]
	private Transform[] _spawnPoints;

	private NetworkRunner _runner;

	private async void Start() {
		// NetworkRunnerのインスタンス化
		_runner = gameObject.AddComponent<NetworkRunner>();
		gameObject.AddComponent<NetworkSceneManagerDefault>();

		// セッションの開始（ホストとして）
		await _runner.StartGame(new StartGameArgs {
			GameMode = GameMode.Host,
			SessionName = "TestRoom",
			Scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex),
			SceneManager = gameObject.GetComponent<INetworkSceneManager>()
		});
	}

	// 新しいプレイヤーが参加した時
	public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) {
		if (runner.IsServer) {
			var spawnIndex = player.PlayerId % _spawnPoints.Length;
			Debug.Log($"Player {player.PlayerId} spawned at index {spawnIndex}");
			var spawnPosition = _spawnPoints[spawnIndex].position;
			Debug.Log($"Spawning player at position: {spawnPosition}");
			runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
		}
	}

	// 入力収集
	public void OnInput(NetworkRunner runner, NetworkInput input) {
		var data = new NetworkInputData();
		data.horizontalInput = Input.GetAxis("Horizontal");
		data.verticalInput = Input.GetAxis("Vertical");
		input.Set(data);
	}

	// 以下は必須だが未使用
	public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) {
	}

	public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {
	}

	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) {
	}

	public void OnConnectedToServer(NetworkRunner runner) {
	}

	public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) {
	}

	public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) {
	}

	public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) {
	}

	public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) {
	}

	public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) {
	}

	public void OnSceneLoadDone(NetworkRunner runner) {
	}

	public void OnSceneLoadStart(NetworkRunner runner) {
	}

	public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) {
	}

	public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) {
	}

	public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
	}

	public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) {
	}

	public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) {
	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
		ArraySegment<byte> data) {
	}

	public void OnDisconnectedFromServer(NetworkRunner runner) {
	}

	public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) {
	}
}