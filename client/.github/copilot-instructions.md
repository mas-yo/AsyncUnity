# clientのソフトウェアアーキテクチャ

関数型の考え方を取り入れた以下の設計とする。


## ゲームの進行

- MainSceneというシーンを１つ用意し、これをゲームの進行の起点とする。
- MainSceneに`Main` というGameObjectを配置し、`Main.cs` をアタッチする。
- Main.Start() から Main.StartAsync() を呼び出す。
- Main.StartAsync() をゲームの進行の起点とする。
- ゲームの状態を表す struct を定義し、Main.StartAsync() 内で定義する。
- 後述する各シーンや機能クラスの StartAsync() を呼び出し、ゲームの状態を受け渡しながらゲームを進行させる。


## シーン

ゲームの進行に応じて複数のシーンを用意する。
例えば HogeScene というシーンには、必ず以下を配置する。
- `Hoge` というGameObjectを配置し、`Hoge.cs` をアタッチする。
- `Hoge` class は Monobehaviourを継承する。

### Hoge classの中身

### シーンの始まりと終わり
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。
- シーンの戻り値として `public struct Result` を定義する。

#### StartAsync() の処理
- 必要に応じて引数を取って良い。ただしstructのみとする。
- シーンをロードし、`Hoge` GameObjectを取得する。
- `Hoge` class のインスタンスメソッドを呼び出してシーンの処理を行う。
- シーンの処理が終わったら、シーンをアンロードし、Resultを返す。


#### Result struct について
- 処理の結果を表すstructとして定義する。

- `StartAsync()` 内で `Hoge` GameObjectを取得しUnity提供の機能を使っても良い。


## 各種機能クラス

シーンに依存しない機能（例：ログイン、リソースのダウンロード）のクラスを用意する。

- `static class`とする
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。この関数は副作用を持ってはならない。
- シーンの戻り値として `public struct Result` を定義する。



