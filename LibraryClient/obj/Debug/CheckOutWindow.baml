   M S B A M L   `   `   `  ����H  DLibraryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LibraryClientLibraryClient  5   	   "    LibraryClient.CheckOutWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LibraryClient     5   	     � Title$  CheckOutWindow��5   	   $	��466��6    $��578.333��6-   .��5	      � .+�5
   
   �� -   5         ��Name$ 
ErrorLabel��5
      $��Center=�6$   $��20,20,16.667,0q�6H   $��65��6`   $	��Top=�6l   
���  ��6�   �� 5   
   -   5        $ LibraryCardIDLabel��5      $��Libary Card ID:��6,   $��
20,100,0,0q�6F   $��26��6Z   $	��Top=�6f   $
��Left=�6~   $	��375��6�   �� 5   
   -   5         ��Name$ LibraryCardIDTextBox��5      $��120,104,0,0q�60    �� TextWrapping$
 Wrap=�6E   $��TextBox��6Y   $��23��6�   $	��Top=�6�   $
��Left=�6�   $	��350��6�   �� 5   
   -   5        $ 	ISBNLabel��5      $��ISBN:��6#   $��20,146,201.333,0q�63   $��26��6M   $	��Top=�6Y   �� 5   
   -   5        $ ISBNTextBox��5      $��120,150,0,0q�6'   $
 Wrap=�6<   $��TextBox��6P   $��23��6   $	��Top=�6�   $
��Left=�6�   $	��350��6�   �� 5   
   -   5         ��Name$ CheckOutButton��5      $��	Check Out��6)   $��425,375,0,0q�6=   $
��Left=�6o   $	��125��6�   $��35��6�   $	��Top=�6�   �� 5   
   -   5         ��Name$ BooksListView��5      $��30,195,21.333,0q�6*   $	��160��6C   $	��Top=�6P    �� View 5      �� 5      .P�5      �� $��ID��6%    �� Width$ 40��61    �� DisplayMemberBinding 6<   �� *ID+�� 5      $��Title��6%   $	 200��64    6@   �� *Title+�� 5      $��
First Name��6%   $	 100��69    6E   �� *Author.Person.FirstName+�� 5      $��	Last Name��6%   $	 100��68    6D   �� *Author.Person.LastName+�� 5      $��Copies��6%   $ 75��65    6@   �� *CopiesAvailable+5      5      �� 5   
   -   5        $ RemoveButton��5      $��Remove��6'   $
��Left=�68   $��250,375,0,0q�6S   $	��Top=�6h   $	��130��6�   $��35��6�   6�   5      5      