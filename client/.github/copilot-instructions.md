# clientのソフトウェアアーキテクチャ

関数型の考え方取り入れた設計にします。

## シーン

ゲームの進行に応じて複数のシーンを用意する。
例えば HogeScene というシーンには、必ず以下を配置します。
- `Hoge` というGameObjectを配置し、`Hoge.cs` をアタッチする。
- `Hoge` class は Monobehaviourを継承。

### Hoge classの中身

- シーンの戻り値として `public struct Result` を定義する。
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。この関数は副作用を持ってはならない。ただしUnity内部の状態を変化させるのはOK
- `StartAsync()` 内で `Hoge` GameObjectを取得しUnity提供の機能を使っても良い。


## 各種機能クラス

例えばログイン、リソースのダウンロードなどの機能単位のクラスを用意する。

- `static class`とする
- `public static async UniTask<Result> StartAsync()` を処理の起点とする。この関数は副作用を持ってはならない。



