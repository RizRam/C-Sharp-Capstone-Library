   M S B A M L   `   `   `  ����H  DLibraryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:LibraryClientLibraryClient  5   	   #    LibraryClient.AddAuthorDialog   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:LibraryClient     5   	     � Title$  Create Author��5   	   $	��381��6   $	��300��6,   .��5	      � .+�5
   
   �� -   5         ��Name$ FirstNameLabel��5
      $��
First Name��6(   $
��Left=�6=   $��	40,65,0,0q�6X   $	��Top=�6k   $��80��6�   �� 5   
   -   5         ��Name$ FirstNameTextBox��5      $
��Left=�6,   $��23��6G   $��
135,69,0,0q�6S    �� TextWrapping$
 Wrap=�6g   $��TextBox��6{   $	��Top=�6�   $	��120��6�   �� 5   
   -   5        $ LastNameLabel��5      $��	Last Name��6'   $
��Left=�6;   $��
40,103,0,0q�6V   $	��Top=�6j   $��80��6�   �� 5   
   -   5        $ LastNameTextBox��5      $
��Left=�6+   $��23��6F   $��135,107,0,0q�6R   $
 Wrap=�6g   $��TextBox��6{   $	��Top=�6�   $	��120��6�   �� 5   
   -   5         ��Name$ CreateAuthorButton��5      $��Create Author��6-   $
��Left=�6E   $��165,245,0,0q�6`   $	��Top=�6u   $��90��6�   6�   �� 5   
   -   5        $ CancelButton��5      $��Cancel��6'   $
��Left=�68   $��
45,245,0,0q�6S   $	��Top=�6g   $��90��6   6�   �� 5   
   -   5        $ label��5      $��Enter author information��6   $��Center=�6B   $
��Left=�6f   $��	45,20,0,0q�6�   $	��Top=�6�   $	��210��6�   �� 5   
   -   5         ��Name$ ErrorTextBlock��5      
���  ��$
��Left=�6=   $��
45,285,0,0q�6X   $
��Wrap=�6l   $��	TextBlock��6�   $	��Top=�6�   $��40��6�   $	��210��6�   �� 5   
   -	   5        $ BioLabel��5      $	��Bio��6"   $
��Left=�60   $��
40,146,0,0q�6K   $	��Top=�6_   $��40��6w   �� 5   
   -
   5        $ 
BioTextBox��5      $
��Left=�6&   $��80��6A   $��
85,150,0,0q�6M   $
 Wrap=�6a   $��TextBox��6u   $	��Top=�6�   $	��170��6�   6�   5      5      