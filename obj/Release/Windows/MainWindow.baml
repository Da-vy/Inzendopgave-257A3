   M S B A M L   `   `   `  ���� M  IInzendopgave 257A3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Inzendopgave_257A3.WindowsInzendopgave_257A3.Windows  +    %Inzendopgave_257A3.Windows.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     :local(clr-namespace:Inzendopgave_257A3.Windows       � Title$  Main Window��$	��450��$	��800��.��� .+�&   System.Windows.Controls.DataGrid  -     Name$ 	DataGrid1��$
��Left=�$	��300��$	��Top=�$	��400��$��	10,10,0,0q�   AutoGenerateColumns .     ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 
�������
���   �   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$
 Team��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Name+  $ Player�� �� *
PlayerName+  $ Coach�� �� *	CoachName+�� -    ��Name$ TextBlock1_Team��$
��Left=�$
��Wrap=�$��Team:��$	��Top=�$��
430,35,0,0q��� -   $ TextBlock2_Player��$
��Left=�$
��Wrap=�$��Player:��$	��Top=�$��
430,75,0,0q��� -   $ TextBlock3_Coach��$
��Left=�$
��Wrap=�$��Coach:��$	��Top=�$��430,115,0,0q��� -   $ TextBlock4_Results��$
��Left=�$
��Wrap=�$��Onderhouden wedstrijden:��$	��Top=�$��429,200,0,0q��� -   $ TextBlock5_Standings��$
��Left=�$
��Wrap=�$��Stand:��$	��Top=�$��429,246,0,0q��� -   $ TextBlock6_Rankings��$
��Left=�$
��Wrap=�$��Individuele Resultaten:��$	��Top=�$��429,292,0,0q��� -    ��Name$ TextBox1_Team��$
��Left=�$��23��	 �� TextWrapping$
	 Wrap=�$�� ��$	��Top=�$	��182��$��
483,32,0,0q�
���   ��� -	   $ TextBox2_Player��$
��Left=�$��23��$
	 Wrap=�$�� ��$	��Top=�$	��182��$��
483,72,0,0q�
���   ��� -
   $ TextBox3_Coach��$
��Left=�$��23��$
	 Wrap=�$�� ��$	��Top=�$	��182��$��483,112,0,0q�
���   ��� -   
 ��Name$
 Button1_Insert��$��Insert��$
��Left=�$	��Top=�$��75��$��20��$��430,152,0,0q��� -   $
 Button2_Update��$��Update��$
��Left=�$	��Top=�$��75��$��20��$��510,152,0,0q��� -   $
 Button3_Delete��$��Delete��$
��Left=�$	��Top=�$��75��$��20��$��590,152,0,0q��� -   $
 Button4_Reset��$��Reset��$
��Left=�$	��Top=�$��75��$��20��$��
687,74,0,0q��� -   $
 Button5_Help��$��Help!��$
��Left=�$	��Top=�$��75��$��
687,33,0,0q��� -   $
 Button7_Results��$��Results��$
��Left=�$	��Top=�$��75��$��590,198,0,0q��� -   $
 Button6_Standings��$��	Standings��$
��Left=�$	��Top=�$��75��$��590,244,0,0q��� -   $
 Button8_Rankings��$��Rankings��$
��Left=�$	��Top=�$��75��$��590,290,0,0q�