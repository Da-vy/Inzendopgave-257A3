   M S B A M L   `   `   `  ����M  IInzendopgave 257A3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Inzendopgave_257A3.WindowsInzendopgave_257A3.Windows  5   	   +    %Inzendopgave_257A3.Windows.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Inzendopgave_257A3.Windows     5   	     � Title$  Main Window��5   	   $	��450��6   $	��800��6*   .��5	      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ 	DataGrid1��5      $
��Left=�6&   $	��300��6A   $	��Top=�6N   $	��400��6f   $��	10,10,0,0q�6r      AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 6�   
�������
���   �   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Team��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Name+  5      $ Player��6%    65   �� *
PlayerName+  5      $ Coach��6%    64   �� *	CoachName+5      �� 5   
   -   5         ��Name$ TextBlock1_Team��5      $
��Left=�6-   $
��Wrap=�6H   $��Team:��6\   $	��Top=�6i   $��
430,35,0,0q�6�   �� 5   
   -   5        $ TextBlock2_Player��5      $
��Left=�6/   $
��Wrap=�6J   $��Player:��6^   $	��Top=�6m   $��
430,75,0,0q�6�   �� 5   
   -   5        $ TextBlock3_Coach��5      $
��Left=�6.   $
��Wrap=�6I   $��Coach:��6]   $	��Top=�6k   $��430,115,0,0q�6�   �� 5   
   -   5        $ TextBlock4_Results��5      $
��Left=�60   $
��Wrap=�6K   $��Onderhouden wedstrijden:��6_   $	��Top=�6   $��429,200,0,0q�6�   �� 5   
   -   5        $ TextBlock5_Standings��5      $
��Left=�62   $
��Wrap=�6M   $��Stand:��6a   $	��Top=�6o   $��429,246,0,0q�6�   �� 5   
   -   5        $ TextBlock6_Rankings��5      $
��Left=�61   $
��Wrap=�6L   $��Individuele Resultaten:��6`   $	��Top=�6   $��429,292,0,0q�6�   �� 5   
   -   5         ��Name$ TextBox1_Team��5      $
��Left=�6)   $��23��6D   	 �� TextWrapping$
	 Wrap=�6P   $�� ��6d   $	��Top=�6l   $	��182��6�   $��
483,32,0,0q�6�   
���   �6�   �� 5   
   -	   5        $ TextBox2_Player��5      $
��Left=�6+   $��23��6F   $
	 Wrap=�6R   $�� ��6f   $	��Top=�6n   $	��182��6�   $��
483,72,0,0q�6�   
���   �6�   �� 5   
   -
   5        $ TextBox3_Coach��5      $
��Left=�6*   $��23��6E   $
	 Wrap=�6Q   $�� ��6e   $	��Top=�6m   $	��182��6�   $��483,112,0,0q�6�   
���   �6�   �� 5!   
   -   5        
 ��Name$
 Button1_Insert��5!      $��Insert��6)   $
��Left=�6:   $	��Top=�6U   $��75��6m   $��20��6x   $��430,152,0,0q�6�   6�   �� 5"   
   -   5        $
 Button2_Update��5"      $��Update��6)   $
��Left=�6:   $	��Top=�6U   $��75��6m   $��20��6x   $��510,152,0,0q�6�   6�   �� 5#   
   -   5        $
 Button3_Delete��5#      $��Delete��6)   $
��Left=�6:   $	��Top=�6U   $��75��6m   $��20��6x   $��590,152,0,0q�6�   6�   �� 5$   
   -   5        $
 Button4_Reset��5$      $��Reset��6(   $
��Left=�68   $	��Top=�6S   $��75��6k   $��20��6v   $��
687,74,0,0q�6�   6�   �� 5%   
   -   5        $
 Button5_Help��5%      $��Help!��6'   $
��Left=�67   $	��Top=�6R   $��75��6j   $��
687,33,0,0q�6u   6�   �� 5&   
   -   5        $
 Button7_Results��5&      $��Results��6*   $
��Left=�6<   $	��Top=�6W   $��75��6o   $��590,198,0,0q�6z   6�   �� 5'   
   -   5        $
 Button6_Standings��5'      $��	Standings��6,   $
��Left=�6@   $	��Top=�6[   $��75��6s   $��590,244,0,0q�6~   6�   �� 5(   
   -   5        $
 Button8_Rankings��5(      $��Rankings��6+   $
��Left=�6>   $	��Top=�6Y   $��75��6q   $��590,290,0,0q�6|   6�   5-      5.      