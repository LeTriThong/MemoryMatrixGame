   M S B A M L   `   `   `  ���� V  RLeTriThong_MemoryMatrixGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:LeTriThong_MemoryMatrixGameLeTriThong_MemoryMatrixGame  ,    &LeTriThong_MemoryMatrixGame.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ;local)clr-namespace:LeTriThong_MemoryMatrixGame     -     � Title$  Memory Matrix��$	��600��$	��800��.���� -   $��canvas������  �� ImageSource$ background.png�� j� Stretch$ UniformToFill=�.+��� -   $��stackpanelLives��
������� �� Left$	 480��
 �� Top$ 0��$
��Auto��$��40�� �� Orientation$ 
Horizontal=�$y�Hidden=�.+��� -   $��rtgSeriesNum1��$��2��
���   �$��16��$��16��$��10q��� -   $��rtgSeriesNum2��$��2��
���   �$��16��$��16��$��5q��� -   $��rtgSeriesNum3��$��2��
���   �$��16��$��16��$��10q��� -   $��lblLevelTitle��$��80��$��40��$��Center=�$	 340��$ 0��$��TILES��$��20(�
���E��
�������$
��Bold"�$��Center=�$y�Hidden=��� -   $��lblScoreTitle��$��80��$��40��$��Center=�$��SCORE��$��20(�$	 600��$ 0��
���E��
�������$
��Bold"�$��Center=�$y�Hidden=��� -	   $��lblCurrentScore��$	��100��$��40��$��Center=�$�� ��$��20(�$	 680��$ 0��
���E��
�������$
��Bold"�$��Right=�$��5q�$y�Hidden=��� -
   $��lblBonusScore��$	��100��$��40��$��Center=�$�� ��$��20(�$	 680��$ 40��
�������$
��Bold"�$��Right=�$y�Hidden=��� -   $��lblCurrentLevel��$��40��$��40��$��Center=�$�� ��$��20(�$	 420��$ 0��
���E��
�������$
��Bold"�$��Center=�$y�Hidden=��� -   $��lblGameTitle��$��./Fonts/#Museo Sans 100)�$��Memory Matrix��$ 0��$ 60��$	��170��$	��794��$��80(�$��Center=�$��Center=�
��������� -   $��lblLevelResult��$��./Fonts/#Museo Sans 100)�$�� ��$ 0��$	 220��$��60��$	��794��$��35(�$��Center=�$��Center=�
��������� -   $��lblScoreResult��$��./Fonts/#Museo Sans 100)�$�� ��$ 0��$	 280��$��60��$	��794��$��35(�$��Center=�$��Center=�
��������� -   $��btnPlay��$	��200��$��80��$	 300��$	 350��$��Center=����#FB6246$
��Play��
�������$��40(�$%��./Fonts/#DIN 1451 Mittelschrift)�