   M S B A M L   `   `   `  ����M  IInzendopgave 257A3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Inzendopgave_257A3.WindowsInzendopgave_257A3.Windows  5   	   0    *Inzendopgave_257A3.Windows.StandingsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Inzendopgave_257A3.Windows     5   	     � Title$  	Standings��5   	   $	��450��6   $	��800��6(   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ DataGrid1_Standings��5
      $
��Left=�60   $	��300��6K   $��	10,10,0,0q�6X   $	��Top=�6k   $	��600��6�      AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�   
�������   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Team��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Team+  5      $ Gespeeld��6%    67   �� *
Gespeeld+  5      $ Gewonnen��6%    67   �� *
Gewonnen+  5      $ Verloren��6%    67   �� *
Verloren+  5      $ 
Gelijkspel��6%    69   �� *
Gelijkspel+  5      $ 	Doelsaldo��6%    68   �� *	Doelsaldo+  5      $ Punten��6%    65   �� *Punten+5      5      5      