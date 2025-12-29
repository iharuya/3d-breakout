# 3Dブロック崩しゲーム

## Meta

このファイルを通じて進捗管理を行う。Claude・人間ともに適宜更新すること。
Claudeは毎ターン開始時にこのファイルを読み込むこと。

---

## 進捗状況

### 現在のフェーズ

**Phase 12: UIManager作成**

### スクリプト作成状況（Claude担当）

| ファイル            | 状態   | 備考 |
| ------------------- | ------ | ---- |
| GameManager.cs      | 完了   |      |
| UIManager.cs        | 未着手 |      |
| PaddleController.cs | 完了   |      |
| BallController.cs   | 完了   |      |
| BlockController.cs  | 完了   |      |
| BlockSpawner.cs     | 完了   |      |
| DeadZone.cs         | 完了   |      |

### Unity Editor作業状況（人間担当）

| 作業           | 状態   | 備考 |
| -------------- | ------ | ---- |
| シーン作成     | 完了   |      |
| カメラ設定     | 完了   |      |
| 物理マテリアル | 完了   |      |
| 壁オブジェクト | 完了 |      |
| パドル         | 完了 |      |
| ボール         | 完了 |      |
| ブロック配置   | 完了 |      |
| UI構築         | 完了 |      |

### メモ・課題

- パドルは左右移動（上下ではない）
- 操作: 矢印キー or 画面左右半分タッチで一定速度移動
- 物理マテリアル(BouncyPhysics)は壁・パドル含め全Colliderに適用すること

---

## ゲーム仕様

- **ライフ制**: 3回ミスでゲームオーバー
- **ブロック**: 単色、1ヒットで破壊
- **スコア**: 壊したブロック数を表示
- **クリア条件**: 全ブロック破壊
- **プラットフォーム**: iPhone/Android 縦画面

### 画面構成（単一シーン方式）

1. **Start Panel** - タイトル + Start Button
2. **Game Panel** - ScoreText + LivesText + MenuButton
3. **Menu Panel** - Resume Button + Start Over Button + Return Home Button
4. **Clear Panel** - Clear Text + Return Home Button + Restart Button
5. **Gameover Panel** - Gameover Text + Return Home Button + Restart Button

---

## 役割分担

### Claude

- C#スクリプト作成・編集
- フォルダ作成
- コード修正・デバッグ支援

### 人間（Unity Editor）

- シーン作成・保存
- 3D/UIオブジェクト作成・配置
- コンポーネント追加・設定
- スクリプトのアタッチ
- プレハブ化
- ビルド設定

---

## 実装フェーズ

> **注意**: 以下の数値（Position,
> Scale等）は目安。実際に動かしながら調整してOK。
> オブジェクト名も多少違っても問題なし（スクリプトとの紐付けはInspectorで行う）。

### Phase 1: フォルダ構造作成

**[Claude]** 以下のフォルダを作成:

```
Assets/Scripts/Managers/
Assets/Scripts/Game/
Assets/Prefabs/
Assets/Materials/
```

**[人間]** Unity Editorで表示確認

---

### Phase 2: シーン準備

**[人間]**

1. `File` → `New Scene` → `Basic (URP)` → `Create`
2. `File` → `Save As...` → `Assets/Scenes/GameScene.unity`

---

### Phase 3: カメラ設定

**[人間]** Main Camera:

- Position: (0, 0, -15) 程度
- Projection: Perspective
- FOV: 60

---

### Phase 4: 物理マテリアル

**[人間]** `Assets/Materials`に`Physic Material`を作成:

- Bounciness: 1
- Friction: 0
- Bounce Combine: Maximum

---

### Phase 5: 壁の作成

**[人間]** Cubeで4つの壁を作成:

- TopWall, LeftWall, RightWall: 通常のCollider
- DeadZone（下）: Is Trigger = true、MeshRenderer無効

目安サイズ:

- 上: Y=8, Scale(10,1,1)
- 左右: X=±5, Scale(1,16,1)
- 下: Y=-9, Scale(10,1,1)

---

### Phase 6: パドル

**[人間]**

1. Cube作成、名前を`Paddle`
2. Position: (0, -7, 0)、Scale: (2, 0.5, 0.5) 程度
3. Rigidbody追加: Use Gravity=OFF, Is Kinematic=ON

**[Claude]** PaddleController.cs作成後 → **[人間]** アタッチ

---

### Phase 7: ボール

**[人間]**

1. Sphere作成、名前を`Ball`
2. Position: (0, -6, 0)、Scale: (0.5, 0.5, 0.5)
3. Rigidbody: Use Gravity=OFF, Collision Detection=Continuous
4. Constraints: Freeze Position Z, Freeze Rotation全て
5. Sphere Colliderに物理マテリアル適用

**[Claude]** BallController.cs作成後 → **[人間]** アタッチ

---

### Phase 8: ブロック

**[人間]**

1. 親オブジェクト`Blocks`(Empty)作成
2. 子にCube作成、Scale: (1.5, 0.5, 0.5)程度
3. **[Claude]** BlockController.cs作成後アタッチ
4. Prefab化（Assets/Prefabsへドラッグ）
5. BlockSpawner.cs作成後 → **[人間]** 空オブジェクト`BlockSpawner`作成、アタッチ&値調整

---

### Phase 9-10: マネージャー系

**[人間]** 空オブジェクト`GameManager`作成 **[Claude]**
GameManager.cs、DeadZone.cs作成 **[人間]** それぞれアタッチ

---

### Phase 11: UI ✅完了

**[人間]** Canvas作成:

- Render Mode: Screen Space - Overlay
- Canvas Scaler: Scale With Screen Size
- Reference: 1080×1920
- Match: 0.5

各パネル:

- **Start Panel**: タイトル、Start Button
- **Game Panel**: ScoreText、LivesText、MenuButton
- **Menu Panel**: Resume Button、Start Over Button、Return Home Button
- **Clear Panel**: Clear Text、Return Home Button、Restart Button
- **Gameover Panel**: Gameover Text、Return Home Button、Restart Button

---

### Phase 12: UIManager

**[Claude]** UIManager.cs作成 **[人間]**
Canvasにアタッチ、Inspector上で各UI要素を紐付け

---

### Phase 13: ビルド設定

**[人間]** Project Settings → Player:

- Default Orientation: Portrait

---

## 参考: Hierarchy完成イメージ

```
GameScene
├── Main Camera
├── Directional Light
├── GameManager
├── Walls
│   ├── TopWall / LeftWall / RightWall
│   └── DeadZone
├── Paddle
├── Ball
├── Blocks (BlockSpawner.cs)
│   └── Block × 20個（自動生成）
└── Canvas
    ├── Start Panel
    │   └── Start Button
    ├── Game Panel
    │   ├── ScoreText
    │   ├── LivesText
    │   └── MenuButton
    ├── Menu Panel
    │   ├── Resume Button
    │   ├── Start Over Button
    │   └── Return Home Button
    ├── Clear Panel
    │   ├── Clear Text
    │   ├── Return Home Button
    │   └── Restart Button
    └── Gameover Panel
        ├── Gameover Text
        ├── Return Home Button
        └── Restart Button
```

---

## 変更履歴

| 日付       | 内容                                           |
| ---------- | ---------------------------------------------- |
| 2024-12-29 | 初版作成                                       |
| 2024-12-30 | Phase 11完了、UI構成確定、PausePanel→Menu Panel |
