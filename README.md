# MazeMaker
Maze maker for Unity

# 特徴
Unityで簡単に立体迷路を自動生成できる。

# 使用方法
1. 迷路を生成する範囲となるオブジェクト(Cubeなど)を作成する。
    1. サイズは1x1x1より大きい任意サイズ。
    1. Mesh Rendererのチェックを外す、または透明マテリアルを設定すると良い
1. 迷路範囲のオブジェクト内側に、迷路の1単位になるGameObjectを生成する。
    1. サイズは1x1x1
		1. MazeMakerGrowInObjスクリプトをアタッチする。
1. 実行すると、範囲オブジェクトに沿って立体迷路を自動生成する。

# その他
- MazeMakerGrowInObjのGrow On Startチェックを外すと、起動時に迷路生成を開始しない。Grow()を呼び出すことで任意のタイミングで迷路生成を開始できる。
