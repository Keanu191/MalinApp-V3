   M S B A M L   `   `   `  ����@  <Q4GUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:Q4GUIQ4GUI  5   	       Q4GUI.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:Q4GUI     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � -   5         � ColumnDefinitions 5
   
   �� 5      $��0* �6   �� 5      $	��17* �6   �� 5      $
��783* �6   .+�5   
   �� -   5         ��Name$ ListBoxReadOnly��5      $��9,50,438,161q�6+   {�. $��2��6R   ��5      �� 5      .R�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $��5q�69   ��6$   �� *Key+69   �� 5      $��5q�6;   ��6$   �� *Value+6;   5      5      5      �� 5   
   $��	0,389,0,0q�6   $��3��6'   .@�5      �� -   5         ��Name$ statusBarText��5      5      �� 5   
   -   5        $ ListBoxSelectable��5      $��414,50,33,161q�6-   {�. $��2��6U   ��5      �� 5      .R�5      �� $ 
Horizontal=�6!   .+�5       �� $��5q�69   ��6$   �� *Key+69   �� 5!      $��5q�6;   ��6$   �� *Value+6;   5"      5#      5%      �� 5&   
   $��Filter Staff by name: ��6   $
��Left=�61   $��414,278,0,0q�6L   $	��Top=�6a   $��2��6y   �� 5'   
   -   5        $��searchInput��5'      $
��Left=�6%   $��548,282,0,0q�6@    �� TextWrapping$
 Wrap=�6U   $	��Top=�6i   $	��120��6�   $��2��6�   �� 5(   
   $��Filter Staff by ID:��6   $
��Left=�6.   $��414,328,0,0q�6I   $	��Top=�6^   $��2��6v   �� 5)   
   -   5        $��searchID��5)      $
��Left=�6"   $��548,332,0,0q�6=   $
 Wrap=�6R   $	��Top=�6f   $	��120��6~   $��2��6�   6�   5*      5+      