   M S B A M L   `   `   `  ����H  DLibraryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LibraryClientLibraryClient  5   	   !    LibraryClient.CheckInWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LibraryClient     5   	     � Title$  CheckInWindow��5   	   $	��300��6   $��463.333��6,   .��5	      � .+�5
   
   �� -   5         ��Name$ CheckOutListView��5
      $��25,65,21.333,70.667q�6-    �� SelectionMode$ Single=�6�   ��6J   ��  �� View 5      �� 5      .P�5      �� $��ID��6%    �� Width$ 40��6`    �� DisplayMemberBinding 61   �� *CheckOutLogId+6`   �� 5      $��Title��6%   $	 175��6`    64   �� *
Book.Title+6`   �� 5      $��Check Out Date��6%   $	 100��6�    6=   �� *CheckOutDate+ �� StringFormat$ 
MM-dd-yyyy��6�   �� 5      $��LibraryCard ID��6%    6=   �� *Cardholder.LibraryCardId+5      5      �� 5   
   -   5         ��Name$ SearchTextBox��5      $��23��6)   $��25,25,21.333,0q�65    �� TextWrapping$
 Wrap=�6M   $��TextBox��6a   $	��Top=�6p   6�   �� 5   
   -   5        	 ��Name$	 CheckInButton��5      $��Check In��6(   $
��Left=�6;   $��205,220,0,0q�6V   $	��Top=�6k   $��75��6�   6�   �� 5   
   -   5        
 ��Name$
 
ErrorLabel��5      $��Label��6$   $
��Left=�64   $��
25,220,0,0q�6O   $	��Top=�6c   $	��130��6{   5      5      