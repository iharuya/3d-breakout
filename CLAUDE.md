# 3Dブロック崩しゲーム

## 概要

Unity 6.0 (URP) で作成した3Dブロック崩しゲーム。見た目は3D、動きは2D（XY平面）。
iPhone/Android 縦画面向け。

## 完了済み

### スクリプト

| ファイル | 役割 |
|----------|------|
| GameManager.cs | ゲーム状態・スコア・ライフ管理 |
| UIManager.cs | UI表示・ボタン/キー入力ハンドリング |
| PaddleController.cs | パドル左右移動（キー/タッチ/マウス） |
| BallController.cs | ボール発射・速度維持 |
| BlockController.cs | ブロック破壊処理 |
| BlockSpawner.cs | ブロック自動生成 |
| DeadZone.cs | ボール落下検出 |

### Unity Editor設定

- シーン: GameScene.unity
- カメラ: Perspective, FOV 60
- 物理マテリアル: BouncyPhysics (Bounciness=1)
- 壁: TopWall, LeftWall, RightWall, DeadZone
- UI: Canvas (1080×1920, Scale With Screen Size)
  - Start Panel, Game Panel, Menu Panel, Clear Panel, Gameover Panel

### ゲーム仕様

- ライフ: 3
- ブロック: 単色、1ヒット破壊
- クリア条件: 全ブロック破壊
- 操作: 矢印キー / 画面左右タッチ / マウス

## 現在のフェーズ

ゲーム骨格完成 → **装飾フェーズへ**
