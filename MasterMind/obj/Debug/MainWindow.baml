   M S B A M L   `   `   `  ����E  AMasterMind, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MasterMind
MasterMind  5   	       MasterMind.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:MasterMind     5   	     � Title$  
MasterMind��5   	    � SizeToContent$ WidthAndHeight=�5	      $	��400��5
      $	��300��5      .��5      ]� ��. .+�5      ��  �� Orientation$ 
Horizontal=�6   $��5 0 0 5q�5      $��Bottom=�5       � 	Resources 5       �� 
TargetType �� Property �� Value��%5  '��       '�� M     �� 5      " ��6   .�5      ��  � ��$ 0 0 5 0q�6    5      �� 5      " ��6   .�5      ��  � ��$ 25��6   �� 5       � ��$ 0 0 5 0q�6    �� 5       �@��	MaxLength$ 1��6#   �� 5       �@��TextAlignment$ Center=�6'   5      5      .+�5      �� ��6   �� *
ValueOne+�� 5      ��6   �� *
ValueTwo+�� 5      ��6   �� *
ValueThree+�� 5      ��6   �� *	ValueFour+�� 5       $��Guess��6   ��5!      �� *AttemptGameCommand+{�5"      �� *CanGuessGame+�� 5#      $��New game��6    � ToolTip$] WThis will verify your attempt towards the solution, it also activates the Guess button.��5%      ��5$      �� *CreateGameCommand+5%      5&      �� 5'      $	��Top=�6   $
��Auto=�5(      .��5)      �� $��10q�6   ��5*      �� *
Attempts+ �� View 5+      �� 5,      .P�5-      �� $��1��6   	 �� Width$	 40��6"   
 �� DisplayMemberBinding
 6-   ��  �� Path$ ValueOne��� 5.      $��2��6   $	 40��6"   
 6-   �� $ ValueTwo��� 5/      $��3��6   $	 40��6"   
 6-   �� $ 
ValueThree��� 50      $��4��6   $	 40��6"   
 6-   �� $ 	ValueFour��� 51      $��Hints��6   $		 100��6&    �� CellTemplate 52   	   �� 53   
   .R�54      �� ��6   �� $ Hints�55      57   	   58      5:      5;      5<      5=      