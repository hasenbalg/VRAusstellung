   M S B A M L   `   `   `  ����N  JVRAustellungManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VRAustellungManagerVRAustellungManager  5      9!clr-namespace:VRAustellungManagerVRAustellungManager  5      .    (VRAustellungManager.PiecePropertiesAudio   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3local!clr-namespace:VRAustellungManager     5      0we!clr-namespace:VRAustellungManager     5      .��5
      �� .+�5   
   �� $	��Art��6   �� 5   
   -   5          ��Name$  pieceComboBox��5      6*   �� 5   
   -   5         ��Name$ pieceIdTextBox��5   $   {�.  �� 5   
   $
��Name��6   �� 5   
   -   5        $ pieceNameTextBox��5      6,   �� 5   
   $��Beschreibung��6   �� 5   
   -   5        $ pieceDescriptionTextBox��5      63   �� 5   
   -   5         ��Name$ pieceFileSelectButton��5   )   $��Datei waehlen��6   6H   �� 5   
   -   5         ��Name$! pieceFileSelectURLTextBlock��5      �� 5   
   $	��300��6   .+�5      � -   5         �Name$ pieceAVPreview��5       � RowDefinitions 5      �� 5      $��* �6$   �� 5      $
��Auto �6$   .+�5      r� -   5        $��mePlayer��5   7    r� LoadedBehavior$ Manual=�6   67   �� 5      $��1��6   .+�5      �� -	   5        $��	lblStatus��5      $��Not playing...��6-   $��Center=�6F   $��5q�6j   � 5      $��Center=�6    .+�5       �� -
   5        $��btnPlay��5    !   .��6F   Play6L   �� 5!      -   5        $��btnPause��5!   !   $	��5,0q�61   .��6U   Pause6\   �� 5"      -   5        $��btnStop��5"   !   .��6F   Stop6L   5#      5$      5%      5&      5'      5(      