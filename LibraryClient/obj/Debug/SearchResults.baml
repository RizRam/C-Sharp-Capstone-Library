   M S B A M L   `   `   `  ����H  DLibraryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LibraryClientLibraryClient  5   	   !    LibraryClient.SearchResults   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LibraryClient     5   	     � Title$  Search Results��5   	   $	��500��6    $	��750��6-    � 	Resources 5	         NoCopiesAvailable �� 
TargetType �� Triggers �� Binding �� Path �� Value �� Property �� Value��%�   &	        �� 5
   
   " ��6*    5      �� 5       6   �� $ CopiesAvailable� 06G   .a�5      ��  � ��
 �  ��63   5      5      5      .��5      � .+�5   
   �� -   5        	 ��Name$	 CloseButton��5      $��Close��6&   $
��Left=�66   $��10,425,0,9.667q�6Q   $��75��6i   
������ 6�   �� 5   
   -   5        
 ��Name$
 SearchLabel��5      $��Label��6%   $
��Left=�65   $��23��6P   $��16,7,0,0q�6\   $	��Top=�6n   $	��267��6�   �� 5   
   -   5         ��Name$ ResultsListView��5      $	��347��6,   $��16,58,10.333,0q�69   $	��Top=�6Q    �� SelectionMode$ Single=�6i    �� View 5      �� 5      .P�5      �� $��ID��6%    �� Width$ 40��61    �� DisplayMemberBinding 6<   �� *BookId+�� 5      $��Title��6%   $	 175��64    6@   �� *Title+�� 5      $��
First Name��6%   $ 80��69    6D   �� *Author.Person.FirstName+�� 5      $��	Last Name��6%   $ 90��68    6C   �� *Author.Person.LastName+�� 5      $��	Publisher��6%   $	 150��68    6D   �� *	Publisher+�� 5      $
��Year��6%   $ 60��63    6>   �� *YearPublished+�� 5      $��Copies Available��6%   $	 100��6?    �� CellTemplate 5      �� 5       .R�5!   "   �� #��[  6,   $��Right=�6x   ��6W   �� *CopiesAvailable+6x   5"      5$      5%      5'      �� 5(   
   -   5        $
 UserTypeLabel��5(      $��Label��6'   $
��Left=�67   $��
626,18,0,0q�6R   $	��Top=�6f   $��96��6~   �� 5)   
   -   5        $	 ViewDetailsButton��5)      $��View Details��6,   $��604,417,10.333,15.667q�6C   {�.  6�   5*      5+      