------------------------------------------------------------
Module ID Overlap Checker v0.1.1
------------------------------------------------------------

このツールはDivaModManagerにてオンになっている各MODのmod_chritm_prop.farcを展開し、アイテム名や設定されているIDを一覧化する補助ツールです。

- 対象 -
・開発したモジュールなどのIDが、他のMODと重複していないか事前に調査したい方
・モジュールなどがID重複のため表示されない方

- 使い方 -
1. "Module ID Overlap Checker.ini"を開き、各項目を設定する
2. "DivaModManager.exe"を実行し、チェックしたいMODをONにする
（この際ゲームを起動する必要はありません）
3. "Module ID Overlap Checker.exe"を実行する
4. 出力されたタブ区切りの"result.txt"を、GoogleスプレッドシートやExcelに貼り付けて確認する

- 動作要件 -
・.NET 8.0
・win-x64

- 注意 -
・現時点ではID等を一覧化するだけで、重複チェックの確認はご自身で行う必要があります。
（今後のアップデートで重複結果出力処理を追加する予定です）

------------------------------------------------------------

This tool is an auxiliary tool that extracts the mod_chritm_prop.farc files from each MOD enabled in DivaModManager and lists the item names and assigned IDs.

- Target Users -
・Those who want to investigate in advance whether the IDs of the developed modules, etc., overlap with other MODs.
・Those whose modules, etc., are not displayed due to ID conflicts.

- How to Use -
1. Open "Module ID Overlap Checker.ini" and set item.
2. Run "DivaModManager.exe" and turn on the MODs you want to check.
(There is no need to start the game at this point.)
3. Run "Module ID Overlap Checker.exe".
4. Paste the outputted tab-delimited "result.txt" into Google Sheets or Excel to verify the results.

- System Requirements -
・.NET 8.0
・win-x64

- Notes -
・At this stage, the tool only lists the IDs and other details; you will need to manually check for overlaps.
(A function to output overlap results is planned for a future update.)
