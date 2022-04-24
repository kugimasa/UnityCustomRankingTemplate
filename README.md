# UnityCustomRankingTemplate
NCMBをバックエンドとして用いたUnityのランキング機能のテンプレートアセットです。

Unity1Weekをはじめとしたゲームジャムでのランキング機能としてご活用ください。
より簡単な設定でランキング導入したい場合は、**@naichilab**さんの[unity-simple-ranking](https://github.com/naichilab/unity-simple-ranking)がおすすめです。

![Sample](https://user-images.githubusercontent.com/40158101/164978642-945155bd-f800-4aa7-9fa8-a24be57bc2d0.jpg)

## アセット構成
- Prefabs
   - RankingCanvas.prefab: ランキングパネル本体を含んだCanvas
   - RankingRecord.prefab: データレコード用のPrefab
- Scripts
   - [RankingManager.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingManager.cs): ランキングのデータ操作を行うためのクラス
   - [RankingRecord.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingRecord.cs): データレコード用のクラス
   - [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs): ランキング用のキーなどを管理するstaticクラス

## 特徴
- 自身のランキングデータの色をカスタマイズできます。
- 順位に応じて、バッジの色をカスタマイズできます。
- スコアが同じユーザは同じ順位になります。
<img width="300" alt="スクリーンショット 2022-04-24 22 14 05" src="https://user-images.githubusercontent.com/40158101/164980862-c4da44a7-af07-424d-8e95-15bf0e2a702d.png">


## 使用方法
1. [ニフクラ mobile backend クイックスタート](https://mbaas.nifcloud.com/doc/current/introduction/quickstart_unity.html)に沿って「APIキーの設定とSDKの初期化」まで設定を行います。
2. NCBMのアプリケーション管理画面の「データストア」から新しいクラスを作成します。(クラス名は任意です)
![Setting](https://user-images.githubusercontent.com/40158101/164979966-da5e5986-5f19-47c5-aba2-9313948998b8.png)
3. 「クラスの編集」から、フィールドを追加します。
![Class](https://user-images.githubusercontent.com/40158101/164980238-83465e27-d698-450b-a4df-e06441beaebe.png)
設定したフィールド名は [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs) に記載されたキーと対応しています。追加でデータを保存したい場合は、RankingUtils.csにも追記が必要です。
- UniqueUserId: ユーザの一意ID(自身のデータの色を変更するために必要です)
- HighScore: ハイスコアデータ
- UserName: ユーザー名
4. [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs) の`NCMBStorageKey`を 2. で設定したクラス名にします。 
https://github.com/kugimasa/UnityCustomRankingTemplate/blob/2dc3af6eb44456d7253c683e9bce672eafb01533/Assets/Scripts/RankingUtils.cs#L9-L10
5. [RankingCanvas.prefab](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Prefabs/RankingCanvas.prefab)をシーンに配置します。
6. 以下のメソッドをデータを保存したいタイミングで呼び出します。
https://github.com/kugimasa/UnityCustomRankingTemplate/blob/2dc3af6eb44456d7253c683e9bce672eafb01533/Assets/Scripts/RankingManager.cs#L57-L60
