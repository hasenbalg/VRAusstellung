   M S B A M L   `   `   `  ����N  JVRAustellungManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:VRAustellungManagerVRAustellungManager  5   	   $    VRAustellungManager.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:VRAustellungManager     5   	   \ XHelixToolkit.Wpf, Version=2015.1.715.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d(helixhttp://helix-toolkit.org/wpf  5   	     � Title$  VR Ausstellung Manager��5	   	   $
��Auto��6(   $
��Auto��66    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.New��5      6c     5   
   -   5        $ ApplicationCommands.Open��5      6e     5   
   -   5        $  ApplicationCommands.SaveAs��5      6i     5   
   -   5        $ ApplicationCommands.Save��5      6e     5   
   -   5        $! local:CustomCommands.Import��5      6j     5   
   -   5        $! local:CustomCommands.Export��5      6j     5   
   -   5        $ local:CustomCommands.Exit��5      6g    J� InputBindings 5      �� 5   
   
 �� Key$ N��6    �� 	Modifiers$ Controlf�6    �� Command$ ApplicationCommands.New��61   6U   �� 5   
   $ O��6   $ Controlf�6   $ ApplicationCommands.Open��61   6V   �� 5   
   $ S��6   $ Controlf�6   $ ApplicationCommands.Save��61   6V   �� 5   
   $ S��6   $ 
Ctrl+Shiftf�6   $  ApplicationCommands.SaveAs��64   6[   .��5      �� .+�5!   
   ]� .+�5"      o� $	��Top=�6   .@�5#      m� $��_Datei��6   .@�5$      m� $
��_Neu��6    m� Command$ ApplicationCommands.New��6-    m� InputGestureText$ Ctrl+N��6O   m� 5%      $��_Oeffnen��6   $ ApplicationCommands.Open��61   $ Ctrl+O��6T   m� 5&      $��
_Speichern��6   $ ApplicationCommands.Save��63   $ Ctrl+S��6V   m� 5'      $��_Speichern unter��6   $ ApplicationCommands.Save��69   $ Ctrl+Shift+S��6\   m� 5(      $��_Importieren��6   $! local:CustomCommands.Import��66   $ Ctrl+I��6\   m� 5)      $��_Exportieren��6   $! local:CustomCommands.Export��66   $ Ctrl+E��6\   m� 5*      $��_Beenden��6   $ local:CustomCommands.Exit��61   $ Alt+F4��6U   5+      5,      �� 5-      6   5.      �� 54   
   	 �� TabStripPlacement$		 Top=�6   .@�55      �� ��56      �� 57      
 �� Orientation$
 
Horizontal=�6!   .+�58      �� $��Allgemeine Einstellungen��6$   59      .��5;      .   (VRAustellungManager.ExhibitionProperties  -   5          Name$! exhibitionPropertiesControl��5;   -   5<      �� 5=      ��5>      �� 5?      $
 
Horizontal=�6!   .+�5@      �� $��Ausstellungsstuecke��6$   5A      .��5C      �  � ColumnDefinitions 5D      �� 5E      $
��Auto �6+   �� 5F      $
��Auto �6+   .+�5H      $   VRAustellungManager.PiecesGrid  -	   5          Name$ piecesGridControl��5H   '   $��0��6B   �� 5I      -
   5         ��Name$' !piecePropertiesControlHolderPanel��5I   !   $��1��6L   5L      5O      5P      5S      5T      