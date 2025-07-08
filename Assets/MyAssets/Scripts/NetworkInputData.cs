using Fusion;

// プレイヤーのキーボードやゲームパッドの入力をネットワークで送るためのデータ形式を定義します。
// INetworkInput インターフェースを継承します。
public struct NetworkInputData : INetworkInput {
	// 水平方向（左右、A/Dキー）の入力を入れる変数
	public float horizontalInput;

	// 垂直方向（前後、W/Sキー）の入力を入れる変数
	public float verticalInput;
}