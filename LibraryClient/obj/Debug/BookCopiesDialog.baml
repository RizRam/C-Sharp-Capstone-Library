   M S B A M L   `   `   `  ����H  DLibraryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LibraryClientLibraryClient  5   	   $    LibraryClient.BookCopiesDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LibraryClient     5   	     � Title$  BookCopiesDialog��5   	   $	��300��6"   $	��300��6/   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��170* �6   �� 5      $
��123* �6   .+�5   
   �� -   5         ��Name$ OKButton��5      $��OK��6#   $
��Left=�60   $��155,165,0,0q�6K   $	��Top=�6`   $��75��6x   $��2��6�   �� 5   
   -   5        $ CancelButton��5      $��Cancel��6'   $
��Left=�68   $��
65,165,0,0q�6S   $	��Top=�6g   $��75��6   6�   �� 5   
   -   5         ��Name$ 
ErrorLabel��5      $��Label��6$   $
��Left=�64   $��
65,200,0,0q�6O   $	��Top=�6c   $��40��6{   $	��165��6�   
���  ��$��2��6�   �� 5   
   -   5         ��Name$ NumberTextBox��5       �� TextAlignment$ Center=�6)   $
��Left=�6@   $��23��6[   $��
65,120,0,0q�6g    �� TextWrapping$
 Wrap=�6{   $��TextBox��6�   $	��Top=�6�   $	��165��6�   $��2��6�   6�   �� 5   
   -   5         ��Name$ QuestionTextBlock��5      $
��Left=�6/   $��	65,30,0,0q�6J   $
��Wrap=�6]   $��	TextBlock��6q   $	��Top=�6�   $��2��6�   $��85��6�   $	��165��6�   5      5      