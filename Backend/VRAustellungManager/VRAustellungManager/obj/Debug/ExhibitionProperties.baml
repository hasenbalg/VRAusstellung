   M S B A M L   `   `   `  ����N  JVRAustellungManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VRAustellungManagerVRAustellungManager  5      .    (VRAustellungManager.ExhibitionProperties   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:VRAustellungManager     5      X TXceed.Wpf.Toolkit, Version=3.2.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      .��5	      �� .+�5
   
   �   � ColumnDefinitions  5   
   �� 5      $
��Auto �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��Titel��6   $��0��6    $��0��6-   �� 5   
   -   5         ��Name$ exhibTitleTextBox��5      $��0��6-   $��1��6:   6J   �� 5   
   $��Beschreibung��6   $��1��6'   $��0��64   �� 5   
   -   5        $ exhibDescriptionTextBox��5      $��1��64   $��1��6A   �� 5   
   $��Groesse��6   $��0��6"   $��2��62   �� 5   
   $��1��6   $��2��6%   �� 5      -   5        $ exhibWidthTextBox��5      65    x �� 5!      -   5        $ exhibHeightTextBox��5!      66   5"      5#      � 5$        5%   
   �� 5&      $��* �6   �� 5'      $��* �6#   �� 5(      $��* �6#   .+�5*      #  Xceed.Wpf.Toolkit.ColorPicker  -   5          Name$ 
firstColor��5*      $��0��63   6C     5+      -   5        $ secondColor��5+      $��1��64   6D     5,      -   5        $ 
thirdColor��5,      $��2��63   6C   5-      5.      5/      