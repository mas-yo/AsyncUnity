# clientのソフトウェアアーキテクチャ

関数型の考え方を取り入れた以下の設計とする。


## ゲームの進行

- MainSceneというシーンを１つ用意し、これをゲームの進行の起点とする。
- MainSceneに`Main` というGameObjectを配置し、`Main.cs` をアタッチする。
- Main.Start() から Main.MainLoopAsync() を呼び出す。
- Main.MainLoopAsync() をゲームの進行の起点とする。
- ゲームの状態を表す struct を定義し、Main.MainLoopAsync() 内で使用する。
- 後述する各シーンや機能クラスの StartAsync() を呼び出し、ゲームの状態を受け渡しながらゲームを進行させる。
- ただしクライアント時間、サーバーセッション情報は通信の度に更新されるため、これらはゲームの状態に含めない。

## シーン

ゲームの進行に応じて複数のシーンを用意する。
例えば HogeScene というシーンには、必ず以下を配置する。
- `Hoge` というGameObjectを配置し、`Hoge.cs` をアタッチする。
- `Hoge` class は Monobehaviourを継承する。

### シーンの始まりと終わり
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。
- シーンの戻り値として `public record struct Result` を定義する。

#### StartAsync() の処理
- 必要に応じて引数を取って良い。ただし record か record struct のみとする。
- シーンをロードし、`Hoge` GameObjectを取得する。
- `Hoge` class のインスタンスメソッドを呼び出してシーンの処理を行う。
- シーンの処理が終わったら、Resultを返す。


## 各種機能クラス

シーンに依存しない機能（例：ログイン、リソースのダウンロード）のクラスを用意する。

- `static class`とする
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。
- 処理結果を表す構造体として `public record struct Result` を定義する。

### StartAsync() の処理
- 必要に応じて引数を取って良い。ただし record か record struct のみとする。
- この関数は副作用を持ってはならない。
- 処理が終わったら、Resultを返す。


