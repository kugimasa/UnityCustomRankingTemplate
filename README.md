# UnityCustomRankingTemplate
NCMBをバックエンドとして用いたUnityのランキング機能のテンプレートアセットです。

Unity1Weekをはじめとしたゲームジャムでのランキング機能としてご活用ください。
より簡単な設定でランキング導入したい場合は、**@naichilab**さんの[unity-simple-ranking](https://github.com/naichilab/unity-simple-ranking)がおすすめです。
(いつもありがとうございます！)

![Sample](https://user-images.githubusercontent.com/40158101/165917150-f6296367-bd4b-413b-93e7-be762aece4df.jpg)

## 動作環境
- Unity Version: 以下のバージョンにて動作確認済みです。
   - 2020.3.8f1(LTS) 
   - 2020.3.21f1(LTS)
   - 2021.3.1f1(LTS)
   
- TextMeshProを使用しています。
- 事前にNCMBの設定が必要です。(詳しくは使用方法を参照してください)

## アセット構成
- Prefabs
  - RankingCanvas.prefab: 以下のPrefabが入っているCanvas
     - RankingPanel.prefab: ランキングデータが表示されるパネル
     - RankingFetcher.prefab: ランキングデータを取得するボタン
     - ScoreSender.prefab: スコアをデータベースに送信するボタン
     - NameForm.prefab: ユーザ名設定用の入力フォーム 
  - RankingRecord.prefab: データレコード用のPrefab
- Scripts
   - [RankingManager.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingManager.cs): ランキングのデータ操作を行うためのクラス
   - [ScoreSender.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/ScoreSender.cs): スコアをデータベースに送信するためのクラス
   - [NameForm.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/NameForm.cs): ユーザ名設定用のクラス
   - [RankingRecord.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingRecord.cs): データレコード用のクラス
   - [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs): ランキング用のキーなどを管理するstaticクラス

## 特徴
- 自身のランキングデータの色をカスタマイズできます。
- 順位に応じて、バッジの素材、色、サイズをカスタマイズできます。
- スコアが同じユーザは同じ順位になります。
- ユーザが自身の名前を自由に変えることができます。
<img width="500" alt="スクリーンショット 2022-04-24 22 14 05" src="https://user-images.githubusercontent.com/40158101/165919290-77302eb9-9e6c-4514-ad7f-3b5140921b5b.png">


## 使用方法
### バックエンド(NCMB)の設定
1. [ニフクラ mobile backend クイックスタート](https://mbaas.nifcloud.com/doc/current/introduction/quickstart_unity.html)に沿って「APIキーの設定とSDKの初期化」まで設定を行います。
2. NCMBのアプリケーション管理画面の「データストア」から新しいクラスを作成します。(クラス名は任意です)
![Setting](https://user-images.githubusercontent.com/40158101/164979966-da5e5986-5f19-47c5-aba2-9313948998b8.png)
3. 「クラスの編集」から、フィールドを追加します。
![Class](https://user-images.githubusercontent.com/40158101/164980238-83465e27-d698-450b-a4df-e06441beaebe.png)
設定したフィールド名は [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs) に記載されたキーと対応しています。追加でデータを保存したい場合は、RankingUtils.csにも追記が必要です。
- UniqueUserId: ユーザの一意ID(自身のデータの色を変更するために必要です)
- HighScore: ハイスコアデータ
- UserName: ユーザー名

### アセットの設定
1. [Releases](https://github.com/kugimasa/UnityCustomRankingTemplate/releases)よりunitypackageをダウンロードし、Unityにインポートします。
2. [RankingUtils.cs](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Scripts/RankingUtils.cs) の`NCMBStorageKey`を 2. で設定したクラス名にします。 
```cs
// NCMBのデータストアキー 
 internal static readonly string NCMBStorageKey = "UnityCustomRanking"; 
```
3. [RankingCanvas.prefab](https://github.com/kugimasa/UnityCustomRankingTemplate/blob/main/Assets/Prefabs/RankingCanvas.prefab)をシーンに配置します。

#### RankingFetcher
RankingPanelを開き、取得したランキングデータを表示します。

#### NameForm
入力フォームにユーザ名を入力し、Saveボタンを押すことでユーザ名を更新します。
データベースも更新されます。確認するには「FETCH RANKING」を押してください。

#### ScoreSender
インスペクターで設定されたスコアデータをデータベースに送信します。
スコア更新(現状のスコアよりハイスコア)がない場合は上書きされません。

<img width="500" alt="image" src="https://user-images.githubusercontent.com/40158101/165920742-aff4837f-4c56-4f37-a047-e6f10dd019c7.png">

## その他
RankingCanvasはあくまで実装例なので自由にカスタマイズしてください。
質問や不具合報告などお気軽にご連絡ください！
