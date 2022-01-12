# coding: utf-8

############### cx_Freeze 用セットアップファイル ##########
# コマンドライン上で python setup.py buildとすると、exe化　#
# Mac用のAppを作成するには、buildをbdist_macとする        #
######################################################
 
import sys, os
from cx_Freeze import setup, Executable

file_path = "./Assets/StreamingAssets/FlaskServer/server.py"


base = None # "Win32GUI"

#importして使っているライブラリを記載
packages = ["qiskit.pulse.library"]

#importして使っているライブラリを記載（こちらの方が軽くなるという噂）
includes = [
    "re",
    "numpy"
]

#excludesでは、パッケージ化しないライブラリやモジュールを指定する。
"""
numpy,pandas,lxmlは非常に重いので使わないなら、除く。（合計で80MBほど）
他にも、PIL(5MB)など。
"""
excludes = [
    "pandas",
    "lxml"
]

##### 細かい設定はここまで #####

#アプリ化したい pythonファイルの指定触る必要はない
exe = Executable(
    script = file_path,
    base = base
)
 
# セットアップ
setup(name = 'main',
      options = {
          "build_exe": {
              "packages": packages, 
              "includes": includes, 
              "excludes": excludes,
          }
      },
      version = '0.1',
      description = 'converter',
      executables = [exe])