---
applyTo: "Assets/AsyncUnity/Scripts/Views/**/*.cs"
---

# ビュークラスの生成ルール

`Assets/AsyncUnity/Scripts/Views/` 以下に新しいビューを追加するとき、必ず以下のルールに従って **2つのファイル** を生成すること。

---

## ファイル構成

| ファイル名 | 役割 |
|---|---|
| `XxxView.cs` | ビューのロジック（純粋な C# クラス） |
| `XxxViewComponents.cs` | Unity コンポーネントの参照（MonoBehaviour） |

---

## XxxViewComponents.cs のルール

- `MonoBehaviour` を継承する
- namespace は `AsyncUnity.Views`
- UI 要素はすべて `[SerializeField] public` フィールドとして公開する
- ボタンは `UnityEngine.UI.Button`、テキストは `UnityEngine.UI.Text` を使う

```csharp
using UnityEngine;
using UnityEngine.UI;

namespace AsyncUnity.Views
{
    public class XxxViewComponents : MonoBehaviour
    {
        [SerializeField]
        public Button XxxButton;

        [SerializeField]
        public Text XxxText;
    }
}
```

---

## XxxView.cs のルール

- 純粋な C# クラス（`MonoBehaviour` 不可）
- namespace は `AsyncUnity.Views`
- `_components` フィールドは `private readonly`
- コンストラクタで `XxxViewComponents` を受け取る
- 戻り値の型を持つ場合は `public struct Result` をクラス内に定義する
- `Show()` / `Hide()` で `gameObject.SetActive` を切り替える
- ボタン押下の待機は `UniTaskUtil.WaitAndCancel<T>` を使い、キャンセルトークンを伝播させる

```csharp
using System.Threading;
using Cysharp.Threading.Tasks;
using AsyncUnity.Utils;

namespace AsyncUnity.Views
{
    public class XxxView
    {
        public struct Result
        {
            // 戻り値フィールドをここに定義
        }

        private readonly XxxViewComponents _components;

        public XxxView(XxxViewComponents components)
        {
            _components = components;
        }

        public void Show()
        {
            _components.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _components.gameObject.SetActive(false);
        }

        public async UniTask<Result> OnClickButtonsAsync(CancellationToken token)
        {
            return await UniTaskUtil.WaitAndCancel<Result>(token,
                t => _components.XxxButton.OnClickAsync(t).ContinueWith(() => new Result { /* ... */ })
            );
        }
    }
}
```

---

## UniTaskUtil.WaitAndCancel の使い分け

| ケース | 使い方 |
|---|---|
| ボタンが **1つ** | `button.OnClickAsync(token).ContinueWith(() => new Result {...})` を直接 await |
| ボタンが **2〜複数・固定数** | `UniTaskUtil.WaitAndCancel<Result>(token, func1, func2, ...)` |
| ボタンが **動的な配列** | `UniTaskUtil.WaitAndCancel<T>(token, params Func<CancellationToken, UniTask<T>>[] funcs)` |

ボタンを複数待機するときは **必ず** `WaitAndCancel` を使い、先に押されたボタンで残りのタスクをキャンセルすること。

---

## Show() に引数が必要なケース

ダイアログのようにテキストを外部から渡す場合は `Show()` の引数でセットする。

```csharp
public void Show(string title, string message)
{
    _components.TitleText.text = title;
    _components.MessageText.text = message;
    _components.gameObject.SetActive(true);
}
```

---

## 動的にボタンを生成するケース（リスト系ビュー）

`Object.Instantiate` でプレハブからボタンを生成し、`finally` ブロックで `Object.Destroy` してクリーンアップする。

```csharp
public async UniTask<Result> OnClickItemButtonAsync(CancellationToken token)
{
    var buttons = new List<Button>();
    try
    {
        var tasks = new Func<CancellationToken, UniTask<long>>[items.Length];
        for (var i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var button = Object.Instantiate(_components.EntryPrefab, _components.ButtonsParent)
                               .GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = item.ToString();
            tasks[i] = t => button.OnClickAsync(t).ContinueWith(() => item.Id);
            buttons.Add(button);
        }
        var resultId = await UniTaskUtil.WaitAndCancel(token, tasks);
        return new Result { Id = resultId };
    }
    finally
    {
        Hide();
        foreach (var button in buttons)
            Object.Destroy(button.gameObject);
    }
}
```

---

## 実装済みビュー一覧（参考）

| クラス名 | 概要 |
|---|---|
| `PauseView` | 2ボタン（Resume / ToTitle）、`WaitAndCancel` で待機 |
| `FooterMenuView` | 3ボタン（Quest / PresentBox / Option）を個別メソッドで公開 |
| `DebugStartView` | 2ボタンを個別の `UniTask` メソッドで公開 |
| `QuestListView` | 動的ボタン生成、`WaitAndCancel` で待機 |
| `PresentBoxView` | 動的ボタン生成、`WaitAndCancel` で待機 |
| `LoadingView` | ローディング表示、コールバック経由でロジックを実行 |
| `GeneralDialogView` | 2ボタン（OK / Cancel）、`struct Result { bool IsOk }` を返す |
