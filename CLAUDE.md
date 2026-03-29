# AsyncUnity プロジェクト

Unity + Firebase を使ったプロジェクト。genie と同様のアーキテクチャを持つ。

## リポジトリ構成

```
AsyncUnity/
├── client/          # Unity プロジェクト (C#)
└── server/firebase/ # Firebase バックエンド (TypeScript)
```

---

## クライアント (Unity)

### 技術スタック

- Unity (C#)
- **UniTask** (Cysharp.Threading.Tasks) — async/await
- **MasterMemory** — 高速インメモリ DB（マスターデータ管理）
- **MessagePack** — バイナリシリアライズ
- **ExcelDataReader** — Excel ファイルの読み込み
- **Lua (LuaCSharp)** — ゲームロジックのスクリプト

### ディレクトリ構成

```
Assets/AsyncUnity/Scripts/
├── Main.cs                  # ゲームループの起点
├── Logics/                  # ゲームロジック・データ処理
├── MasterData/              # マスターデータの型定義
├── Protocols/               # API リクエスト/レスポンス定義
├── Scenes/                  # シーンクラス
├── Views/                   # View クラス・ViewComponents クラス
└── Utils/                   # ユーティリティ
```

### アーキテクチャ

genie と同じ構造：

1. `[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]` で MessagePack リゾルバを初期化
2. `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` で `MainLoopAsync()` を起動
3. `MainLoopAsync()` でマスターデータを Excel から読み込み、無限ループで各シーンを呼び出す

#### シーン規則

```csharp
public static class HogeScene
{
    public static async UniTask<Result> StartAsync(..., CancellationToken token)
    {
        // シーンをロードして処理し、Result を返す
    }
}
```

---

## サーバー (Firebase)

### 技術スタック

- Firebase Functions (TypeScript)
- Firestore
- Firebase Hosting
- Firebase Admin SDK

### 構成

```
server/firebase/
├── functions/src/index.ts   # Cloud Functions
├── firestore.rules          # セキュリティルール
└── public/                  # Hosting 静的ファイル
```

---

## 開発上の注意

- async メソッドは必ず `UniTask` を使う（`Task` は使わない）
- シーンクラスは `static class` とし、`CancellationToken` を受け取る
